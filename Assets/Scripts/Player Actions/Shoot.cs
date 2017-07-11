using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    public float bulletTime = 1000;
    bool IsShooting;
    float lastTick, millisecond;
    LineRenderer lr;
    GameObject cam;

    // Use this for initialization
    void Start () {
        IsShooting = false;
        cam = Camera.main.gameObject;
        lr = cam.GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            ShootRay();
            IsShooting = true;
        }
        lr.enabled = IsShooting;
        if (IsShooting) millisecond++;
        if (millisecond > bulletTime && IsShooting) {
            IsShooting = false;
            millisecond = 0;
        }
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
}
