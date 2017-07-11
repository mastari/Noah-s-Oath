using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    public static bool paused;
    public GameObject menu;
    public Button back, options, leave, quit;
    public Button changeAnimal;

    public static GameObject currentAnimal;

 
	void Start () {
        currentAnimal = StartScript.currentAnimal;
        Pause(false);
        back.onClick.AddListener(Back);
        options.onClick.AddListener(Options);
        leave.onClick.AddListener(Leave);
        quit.onClick.AddListener(Quit);
        changeAnimal.onClick.AddListener(ChangeAnimal);
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Pause(!paused);
        }
        Cursor.visible = false;
        if (paused) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Pause(bool isPause) {
        paused = isPause;
        menu.SetActive(isPause);
    }

    public void Back() {
        Pause(false);
    }

    public void Options() {

    }

    public void Leave() {
        SceneManager.LoadScene("StartScene");
    }

    public void Quit() {

        var s = new SaveFile();
        s.HealthData = new HealthData();
        s.HealthData.Health = currentAnimal.GetComponent<ObjectHealth>().GetHealth();

        s.LocationData = new LocationData();
        s.LocationData.Location = currentAnimal.transform.position;

        SaveFile.Save(s, "save.xml");

        Application.Quit();
    }

    public static void ChangeAnimal() {
        
        List<string> AnimalTypes = AnimalClass.Animals;
        
        currentAnimal = null;
        var num = 0;

        foreach (string type in AnimalTypes) {
            var animal = GameObject.Find(type);
            if (animal != null && animal.active) {
                currentAnimal = GameObject.Find(type);
                break;
            }
            num++;
        }
        //currentAnimal = Camera.main.gameObject.transform.parent.gameObject;
        if (currentAnimal == null) {
            return;
        }

        //change num
        if (num == AnimalTypes.Count-1) {
            num = 0;
        } else {
            num++;
        }

        //GameObject nextAnimal = GameObject.Find(AnimalTypes[num]);
        GameObject nextAnimal = StartScript.Animals.Find(i => i.gameObject.name == AnimalTypes[num]);

        var cam = StartScript.camera;
        cam.SetActive(true);
        cam.name = cam.name.Substring(0, cam.name.Length);

        //cam.gameObject.AddComponent<CameraController>();
        cam.transform.parent = nextAnimal.transform;
        cam.transform.position = new Vector3(0, 1, 0);

        var cc = cam.GetComponent<CameraController>();
        cc.target = nextAnimal.transform;
        cc.speed = 100f;
        cc.lockCursor = true;
        
        currentAnimal.SetActive(false);
        nextAnimal.SetActive(true);
        nextAnimal.transform.position = currentAnimal.transform.position + new Vector3(0, 1, 0);
        
    } 
}
