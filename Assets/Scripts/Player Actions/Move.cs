using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public float mspeed = 10;
    public float speed = 0;
    public float sspeed = 20;
    Quaternion forward;
    public bool CanSprint = true;
    public bool IsSprinting;
    GameObject cam;

    // Use this for initialization
    void Start () {
        cam = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        forward = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
        if (Input.GetKey(KeyCode.W)) {
            if (CanSprint) {
                speed = mspeed;
                IsSprinting = Input.GetKey(KeyCode.LeftShift);
                if (IsSprinting) speed = sspeed; else speed = mspeed;
            }
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

                if (b.transform.tag != "Player") { //|| b.transform.tag != "Plane" || !b.transform.tag.Contains("Cube")

                    if (b.transform.GetComponent<Collider>() != null) {
                        Debug.Log(b.transform.name);
                        return i;
                    }
                }
            }
        }
        return 0;
    }
}
