using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    
    public float mspeed = 10;
    public float speed = 0;
    float smultiplier = 2f;

    public float jumpSpeed = 100;
    public float bulletTime = 1000;

    bool isShooting;
    float lastTick, millisecond;
    public bool isSprinting;

    LineRenderer lr;
    Rigidbody rb;
    GameObject cam, hud, lowHealthOverlay;
    ObjectHealth health;
    Vector3 spawnpoint;
    Slider healthBar;
    Text healthText;
    GameObject player;
    InventoryScript inv;
    Quaternion forward;

    void Start() {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.gameObject;
        lr = cam.GetComponent<LineRenderer>();
        health = GetComponent<ObjectHealth>();
        spawnpoint = cam.transform.position;
        isShooting = false;
        hud = transform.Find("HUD").gameObject;
        healthBar = hud.transform.FindChild("HealthBar").GetComponent<Slider>();
        healthText = healthBar.transform.gameObject.transform.FindChild("HealthText").GetComponent<Text>();
        lowHealthOverlay = hud.transform.FindChild("LowHealth").gameObject;
        inv = transform.GetComponent<InventoryScript>();
    }

    void Update() {
        ManageHealth();
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

    void Move() {
        forward = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
        if (Input.GetKey(KeyCode.W)) {
            speed = mspeed;
            isSprinting = Input.GetKey(KeyCode.LeftShift);
            if (isSprinting) speed *= smultiplier; else speed = mspeed;
            MovePlayer(Vector3.forward * GetMoveLength(cam.transform.forward));
        }
        if (Input.GetKey(KeyCode.A)) {
            MovePlayer(Vector3.left * GetMoveLength(-cam.transform.right));
        }
        if (Input.GetKey(KeyCode.S)) {
            MovePlayer(Vector3.back * GetMoveLength(-cam.transform.forward));
        }
        if (Input.GetKey(KeyCode.D)) {
            MovePlayer(Vector3.right * GetMoveLength(cam.transform.right));
        }
        if (cam.transform.position.y < -100) health.UpdateHealth(-10);

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("StartScene");
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

    void MovePlayer(Vector3 pos) {
        transform.rotation = forward;
        transform.Translate(pos * Time.deltaTime);
    }

    float GetMoveLength(Vector3 direction) {
        return speed; //COMMENT OUT WHEN FIXING
        for (var i = mspeed * 1; i >= 0; i -= 1) {
            
            RaycastHit[] box = Physics.BoxCastAll(transform.position, transform.localScale, direction, new Quaternion(), i);
            //if (!Array.Exists(box, e => e.transform.tag != "Player")) {

            //    return i;
            //}
            foreach (var b in box) {

                if (b.transform.tag != "Player" ) { //|| b.transform.tag != "Plane" || !b.transform.tag.Contains("Cube")

                    if (b.transform.GetComponent<Collider>() != null) {
                        Debug.Log(b.transform.name);
                        return i;
                    }
                }
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
        return Array.Exists(below, e => !e.transform.GetComponent<Collider>().name.Contains("Player"));
    }

    void UpdateHealthSlider() {
        healthBar.value = health.GetHealth() / health.GetMaxHealth();
        healthText.text = health.GetHealth() + " / " + health.GetMaxHealth();
    }

    void SetHealthOverlay(bool on) {
        lowHealthOverlay.SetActive(on);
    }

}