using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public ItemType type;

    public Item (ItemType type) {
        this.type = type;
    }

    public enum ItemType {
        STICK, ROCK, GOLD
    }
}
