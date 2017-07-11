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
    public static GameObject currentAnimal;

    bool testLoad = false;
    SaveFile s;


    public float maxHealth;

    void Start() {
        s = SaveFile.Load("save.xml");
        
        camera = GameObject.Find("Main Camera");

        var ac = new AnimalClass("Lion");
        ac.Move(movementSpeed);
        ac.Jump(jumpSpeed);
        GameObject acgo = ac.GetPlayer();
        Animals.Add(acgo);

        var leemur = new AnimalClass("Leemur");
        //leemur.Move(movementSpeed);
        leemur.Jump(jumpSpeed);
        GameObject leemurthing = leemur.GetPlayer();
        Animals.Add(leemurthing);
        
        var cc = new AnimalClass("Cube");
        cc.Move(movementSpeed);
        cc.Jump(jumpSpeed);
        GameObject ccgo = cc.GetPlayer();
        Animals.Add(ccgo);
        

        acgo.SetActive(false);
        leemurthing.SetActive(false);

        currentAnimal = ccgo;

        //GameObject.Find("HUD").transform.SetParent(Player.transform);
    }

    void Update() {
        if (!testLoad) {
            load(currentAnimal, s);
            testLoad = true;
        }
        
    }

    public void load(GameObject ca, SaveFile s) {
        ca.GetComponent<ObjectHealth>().SetHealth(s.HealthData.Health);
        ca.transform.position = s.LocationData.Location;
    }
}
