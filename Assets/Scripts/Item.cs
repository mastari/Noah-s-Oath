using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public ItemType type;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public Item(ItemType type) {
        this.type = type;
    }

    public enum ItemType {
        STICK, ROCK, GOLD
    }
}
