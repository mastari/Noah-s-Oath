using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public bool lockCursor = true;
    public float movementspeed = 10;
    public float jumpSpeed = 100;
    public float bulletTime = 1000;

    bool isShooting;
    float lastTick, millisecond;

    LineRenderer lr;
    Rigidbody rb;
    GameObject cam, hud, lowHealthOverlay;
    Vector2 sensitivity = new Vector2(1, 1);
    Vector2 smoothing = new Vector2(3, 3);
    Vector2 clampInDegrees = new Vector2(360, 180);
    Vector2 targetDirection, targetCharacterDirection, _mouseAbsolute, _smoothMouse;
    ObjectHealth health;
    Vector3 spawnpoint;
    Slider healthBar;
    Text healthText;
    GameObject player;
    Transform transform;

    void Start() {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        transform = player.transform;
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.gameObject;
        targetDirection = cam.transform.localRotation.eulerAngles;
        targetCharacterDirection = transform.localRotation.eulerAngles;
        lr = cam.GetComponent<LineRenderer>();
        health = GetComponent<ObjectHealth>();
        spawnpoint = cam.transform.position;
        isShooting = false;
        hud = transform.Find("HUD").gameObject;
        healthBar = hud.transform.FindChild("HealthBar").GetComponent<Slider>();
        healthText = healthBar.transform.gameObject.transform.FindChild("HealthText").GetComponent<Text>();
        lowHealthOverlay = hud.transform.FindChild("LowHealth").gameObject;
    }

    void Update() {
        ManageHealth();
        Look();
        Move();
        Jump();
        Shoot();
    }

    // FUNCTIONS

    void Jump() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (isGrounded()) {
                rb.AddForce(Vector3.up * jumpSpeed * 4);
            }
        }
    }

    void ManageHealth() {
        if (health.GetHealth() == 0) {
            Respawn();
        }
        SetHealthOverlay(health.GetHealth() < health.GetMaxHealth() / 10);
        UpdateHealthSlider();
    }

    void Look() {
        Cursor.visible = lockCursor;
        Cursor.lockState = CursorLockMode.Locked;
        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);
        _mouseAbsolute += _smoothMouse;
        if (clampInDegrees.x < 360) _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);
        var xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
        cam.transform.localRotation = xRotation;
        if (clampInDegrees.y < 360) _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);
        cam.transform.localRotation *= targetOrientation;
        var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.up);
        transform.localRotation = yRotation;
        transform.localRotation *= targetCharacterOrientation;
        if (cam.transform.position.y < -100) health.UpdateHealth(-10);
    }

    void Move() {
        if (Input.GetKey(KeyCode.W)) {
            MoveVec(Vector3.forward * Time.deltaTime * moveLength(cam.transform.forward));
        }
        if (Input.GetKey(KeyCode.A)) {
            MoveVec(Vector3.left * Time.deltaTime * moveLength(-cam.transform.right));
        }
        if (Input.GetKey(KeyCode.S)) {
            MoveVec(Vector3.back * Time.deltaTime * moveLength(-cam.transform.forward));
        }
        if (Input.GetKey(KeyCode.D)) {
            MoveVec(Vector3.right * Time.deltaTime * moveLength(cam.transform.right));
        }
    }

    void Shoot() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            ShootRay();
            isShooting = true;
        }
        lr.enabled = isShooting;
        if (isShooting) millisecond++;
        if (millisecond > bulletTime && isShooting) {
            isShooting = false;
            millisecond = 0;
        }
    }

    // UTILITIES

    void Respawn() {
        transform.position = spawnpoint;
        health.SetHealth(1000);
    }

    void MoveVec(Vector3 pos) {
        transform.Translate(pos);
    }

    float moveLength(Vector3 direction) {
        return movementspeed; //COMMENT OUT WHEN FIXING
        for (var j = movementspeed * Time.deltaTime; j >= 0; j -= Time.deltaTime) {
            RaycastHit[] box = Physics.BoxCastAll(transform.position, transform.localScale / 2, direction, new Quaternion(0, 0, 0, 0), j);
            //if (!Array.Exists(box, e => e.transform.tag != "Player")) {
                
            //    return j;
            //}
            foreach(var i in box) {
                if (i.transform.tag != "Player")
                    if (i.transform.GetComponent<Collider>() != null)
                        return j;
            }
        }
        return 0;
    }

    void ShootRay() {
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            ObjectHealth health = hit.transform.GetComponent<ObjectHealth>();
            if (health != null) {
                health.UpdateHealth(-1);
            }
        }
    }

    bool isGrounded() {
        RaycastHit[] below = Physics.BoxCastAll(transform.position, new Vector3(0.5f, 0.5f, 0.5f), Vector3.down, GetComponent<Transform>().rotation, 0.05f);
        return Array.Exists(below, e => e.transform.GetComponent<Collider>().name != "Player");
    }

    void UpdateHealthSlider() {
        healthBar.value = health.GetHealth() / health.GetMaxHealth();
        healthText.text = health.GetHealth() + " / " + health.GetMaxHealth();
    }

    void SetHealthOverlay(bool on) {
        lowHealthOverlay.SetActive(on);
    }

}