using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageHealth : MonoBehaviour {

    ObjectHealth health;
    Vector3 spawnpoint;

    // Use this for initialization
    void Start () {
        health = GetComponent<ObjectHealth>();
        spawnpoint = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (health.GetHealth() == 0) {
            Respawn();
        }
    }

    void Respawn() {
        transform.position = spawnpoint;
        health.SetHealth(100);
    }
}
