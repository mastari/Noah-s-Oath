using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Jump : MonoBehaviour {

    public float jumpSpeed = 100;
    Rigidbody rb;
    
    void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (isGrounded()) {
                rb.AddForce(Vector3.up * jumpSpeed * 4);
            }
        }
    }

    bool isGrounded() {
        RaycastHit[] below = Physics.BoxCastAll(transform.position, new Vector3(0.5f, 0.5f, 0.5f), Vector3.down, GetComponent<Transform>().rotation, 0.05f);
        return Array.Exists(below, e => !e.transform.GetComponent<Collider>().name.Contains("Player"));
    }
}
