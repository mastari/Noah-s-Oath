using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Instantiate(Resources.Load("Player") as GameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
