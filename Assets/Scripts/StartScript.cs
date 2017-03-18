using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour {

    public float jumpSpeed;
    public float bulletTime;
    public float movementSpeed;
    public bool lockCursor;

    public float maxHealth;

	// Use this for initialization
	void Start () {
        GameObject player = Instantiate(Resources.Load("Player") as GameObject);
        player.transform.position = new Vector3(0, 0.5f, 0);

        var rb = player.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        var health = player.AddComponent<ObjectHealth>();
        health.maxHealth = maxHealth;

        var controller = player.AddComponent<PlayerController>();
        controller.jumpSpeed = jumpSpeed;
        controller.bulletTime = bulletTime;
        controller.lockCursor = lockCursor;
        controller.movementspeed = movementSpeed;

        var cam = Camera.main;
        cam.transform.parent = player.transform;
        cam.transform.position = new Vector3(0, 1, 0);

        var inv = player.AddComponent<InventoryScript>();


        GameObject.Find("HUD").transform.SetParent(player.transform);
    }
}
