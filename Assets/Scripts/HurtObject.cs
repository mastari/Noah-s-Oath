using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter(Collision collision) {
        
        GameObject collider = collision.collider.gameObject;
        
        if (collider.GetComponent<ObjectHealth>() != null) {
            collider.GetComponent<ObjectHealth>().UpdateHealth(-10);
        }
    }
}
