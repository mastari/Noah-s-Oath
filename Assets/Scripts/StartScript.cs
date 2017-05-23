using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour {

    public float jumpSpeed;
    public float bulletTime;
    public float movementSpeed;
    public bool lockCursor;
                                                                                                                      
    public float maxHealth;
                                                                                       
	void Start () {
        GameObject Player = Instantiate(Resources.Load("Player") as GameObject);

        var ac = new AnimalClass("Lion", Player);
        ac.Move(movementSpeed);
        ac.Jump(jumpSpeed);
        ac.Shoot(bulletTime);
        
        GameObject.Find("HUD").transform.SetParent(Player.transform);
    }
}
