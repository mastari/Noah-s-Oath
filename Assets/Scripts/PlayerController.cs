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
        for (var i = movementspeed * 1; i >= 0; i -= 1) {
            
            RaycastHit[] box = Physics.BoxCastAll(transform.position, transform.localScale, direction, new Quaternion(), i);
            ExtDebug.DrawBoxCastBox(transform.position + new Vector3(0, 0.1f, 0), transform.localScale / 2, new Quaternion(), direction, i, Color.cyan);
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














public static class ExtDebug {
    //Draws just the box at where it is currently hitting.
    public static void DrawBoxCastOnHit(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float hitInfoDistance, Color color) {
        origin = CastCenterOnCollision(origin, direction, hitInfoDistance);
        DrawBox(origin, halfExtents, orientation, color);
    }

    //Draws the full box from start of cast to its end distance. Can also pass in hitInfoDistance instead of full distance
    public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, Color color) {
        direction.Normalize();
        Box bottomBox = new Box(origin, halfExtents, orientation);
        Box topBox = new Box(origin + (direction * distance), halfExtents, orientation);

        Debug.DrawLine(bottomBox.backBottomLeft, topBox.backBottomLeft, color);
        Debug.DrawLine(bottomBox.backBottomRight, topBox.backBottomRight, color);
        Debug.DrawLine(bottomBox.backTopLeft, topBox.backTopLeft, color);
        Debug.DrawLine(bottomBox.backTopRight, topBox.backTopRight, color);
        Debug.DrawLine(bottomBox.frontTopLeft, topBox.frontTopLeft, color);
        Debug.DrawLine(bottomBox.frontTopRight, topBox.frontTopRight, color);
        Debug.DrawLine(bottomBox.frontBottomLeft, topBox.frontBottomLeft, color);
        Debug.DrawLine(bottomBox.frontBottomRight, topBox.frontBottomRight, color);

        DrawBox(bottomBox, color);
        DrawBox(topBox, color);
    }

    public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color) {
        DrawBox(new Box(origin, halfExtents, orientation), color);
    }
    public static void DrawBox(Box box, Color color) {
        Debug.DrawLine(box.frontTopLeft, box.frontTopRight, color);
        Debug.DrawLine(box.frontTopRight, box.frontBottomRight, color);
        Debug.DrawLine(box.frontBottomRight, box.frontBottomLeft, color);
        Debug.DrawLine(box.frontBottomLeft, box.frontTopLeft, color);

        Debug.DrawLine(box.backTopLeft, box.backTopRight, color);
        Debug.DrawLine(box.backTopRight, box.backBottomRight, color);
        Debug.DrawLine(box.backBottomRight, box.backBottomLeft, color);
        Debug.DrawLine(box.backBottomLeft, box.backTopLeft, color);

        Debug.DrawLine(box.frontTopLeft, box.backTopLeft, color);
        Debug.DrawLine(box.frontTopRight, box.backTopRight, color);
        Debug.DrawLine(box.frontBottomRight, box.backBottomRight, color);
        Debug.DrawLine(box.frontBottomLeft, box.backBottomLeft, color);
    }

    public struct Box {
        public Vector3 localFrontTopLeft { get; private set; }
        public Vector3 localFrontTopRight { get; private set; }
        public Vector3 localFrontBottomLeft { get; private set; }
        public Vector3 localFrontBottomRight { get; private set; }
        public Vector3 localBackTopLeft { get { return -localFrontBottomRight; } }
        public Vector3 localBackTopRight { get { return -localFrontBottomLeft; } }
        public Vector3 localBackBottomLeft { get { return -localFrontTopRight; } }
        public Vector3 localBackBottomRight { get { return -localFrontTopLeft; } }

        public Vector3 frontTopLeft { get { return localFrontTopLeft + origin; } }
        public Vector3 frontTopRight { get { return localFrontTopRight + origin; } }
        public Vector3 frontBottomLeft { get { return localFrontBottomLeft + origin; } }
        public Vector3 frontBottomRight { get { return localFrontBottomRight + origin; } }
        public Vector3 backTopLeft { get { return localBackTopLeft + origin; } }
        public Vector3 backTopRight { get { return localBackTopRight + origin; } }
        public Vector3 backBottomLeft { get { return localBackBottomLeft + origin; } }
        public Vector3 backBottomRight { get { return localBackBottomRight + origin; } }

        public Vector3 origin { get; private set; }

        public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents) {
            Rotate(orientation);
        }
        public Box(Vector3 origin, Vector3 halfExtents) {
            this.localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
            this.localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

            this.origin = origin;
        }


        public void Rotate(Quaternion orientation) {
            localFrontTopLeft = RotatePointAroundPivot(localFrontTopLeft, Vector3.zero, orientation);
            localFrontTopRight = RotatePointAroundPivot(localFrontTopRight, Vector3.zero, orientation);
            localFrontBottomLeft = RotatePointAroundPivot(localFrontBottomLeft, Vector3.zero, orientation);
            localFrontBottomRight = RotatePointAroundPivot(localFrontBottomRight, Vector3.zero, orientation);
        }
    }

    //This should work for all cast types
    static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance) {
        return origin + (direction.normalized * hitInfoDistance);
    }

    static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation) {
        Vector3 direction = point - pivot;
        return pivot + rotation * direction;
    }
}