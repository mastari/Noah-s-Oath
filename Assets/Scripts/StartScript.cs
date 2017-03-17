using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject player = Instantiate(Resources.Load("Player") as GameObject);
        //transform = player.transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
