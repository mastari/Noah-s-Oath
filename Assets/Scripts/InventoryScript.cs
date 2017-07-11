using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour {

    public List<Item.ItemType> items = new List<Item.ItemType>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Item>() != null) {
            var item = collision.gameObject.GetComponent<Item>();
            items.Add(item.type);
            Destroy(collision.gameObject);
        }
        
    }
}
