using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectHealth : MonoBehaviour {

    public float maxHealth;
    public float health;
    public RectTransform healthBar;

    void Start() {
        health = maxHealth;
        healthBar = GameObject.Find("HUD").transform.Find("Background").Find("Foreground").GetComponent<RectTransform>();
    }

    public void UpdateHealth(float amount) {
        health += amount;
        F();
    }

    public void SetHealth(float amount) {
        health = amount;
        F();
    }

    public void SetMaxHealth(float amount) {
        maxHealth = amount;
        F();
    }

    public void F() {
        health = Mathf.Max(health, 0);
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }

    public float GetHealth() {
        return health;
    }

    public float GetMaxHealth() {
        return maxHealth;
    }
}