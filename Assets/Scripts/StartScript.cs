using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour {

    public float jumpSpeed;
    public float bulletTime;
    public float movementSpeed;
    public bool lockCursor;
    public static GameObject camera;
    public static List<GameObject> Animals = new List<GameObject>();


    public float maxHealth;

    void Start() {
        camera = GameObject.Find("Main Camera");

        var ac = new AnimalClass("Lion");
        ac.Move(movementSpeed);
        ac.Jump(jumpSpeed);
        GameObject acgo = ac.GetPlayer();
        Animals.Add(acgo);


        var cc = new AnimalClass("Cube");
        cc.Move(movementSpeed);
        cc.Jump(jumpSpeed);
        GameObject ccgo = cc.GetPlayer();
        Animals.Add(ccgo);
        acgo.SetActive(false);

        //GameObject.Find("HUD").transform.SetParent(Player.transform);
    }
}
