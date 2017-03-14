using UnityEngine;
using System.Runtime.InteropServices;
using System;

[AddComponentMenu("Camera/Simple Smooth Mouse Look ")]
public class PlayerController : MonoBehaviour {
    Vector2 _mouseAbsolute;
    Vector2 _smoothMouse;

    public Vector2 clampInDegrees = new Vector2(360, 180);
    public bool lockCursor;
    public Vector2 sensitivity = new Vector2(2, 2);
    public Vector2 smoothing = new Vector2(3, 3);
    public Vector2 targetDirection;
    public Vector2 targetCharacterDirection;
    public float movementspeed;
    Rigidbody rb;
    public float jumpSpeed;
    public int maxWall;

    public bool isJumping = false;
    GameObject cam;
    public Vector3 spawnpoint;

    void Start() {
        rb = GetComponent<Rigidbody>();
        cam = transform.Find("Main Camera").gameObject;
        targetDirection = cam.transform.localRotation.eulerAngles;
        targetCharacterDirection = transform.localRotation.eulerAngles;

        spawnpoint = cam.transform.position;
    }

    void Update() {
        Look();
        Move();
        Jump();
    }

    bool isGrounded() {
        RaycastHit[] below = Physics.BoxCastAll(transform.position, new Vector3(0.5f, 0.5f, 0.5f), Vector3.down, GetComponent<Transform>().rotation, 0.05f);
        if (Array.Exists(below, e => e.collider != null))
            return true;
        return false;
    }

    void Jump() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (isGrounded()) {
                rb.AddForce(Vector3.up * jumpSpeed * 4);
            }
        }
    }

    void Look() {
        Screen.lockCursor = lockCursor;
        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);
        _mouseAbsolute += _smoothMouse;
        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);
        var xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
        cam.transform.localRotation = xRotation;
        if (clampInDegrees.y < 360)
            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);
        cam.transform.localRotation *= targetOrientation;
        var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.up);
        transform.localRotation = yRotation;
        transform.localRotation *= targetCharacterOrientation;
    }

    void Move() {
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector3.forward * moveLength(cam.transform.forward));
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector3.left * moveLength(-cam.transform.right));
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(Vector3.back * moveLength(-cam.transform.forward));
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector3.right * moveLength(cam.transform.right));
        }
    }

    float moveLength(Vector3 direction) {
        for (var j = movementspeed * 5 * Time.deltaTime; j >= 0; j -= Time.deltaTime) {
            RaycastHit[] box = Physics.BoxCastAll(cam.transform.position, cam.transform.localScale / 2, direction, new Quaternion(0, 0, 0, 0), j);
            if (!Array.Exists(box, e => e.transform.tag == "Wall")) {
                return j;
            }
        }
        return 0;
    }

}