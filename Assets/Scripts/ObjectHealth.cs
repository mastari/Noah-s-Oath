using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : MonoBehaviour {

    public float maxHealth;
    float health;

    void Start() {
        health = maxHealth;
    }

    public void UpdateHealth(float amount) {
        health += amount;
        health = Mathf.Max(health, 0);
    }

    public void SetHealth(float amount) {
        health = amount;
        health = Mathf.Max(health, 0);
    }

    public float GetHealth() {
        return health;
    }

    public float GetMaxHealth() {
        return maxHealth;
    }
}