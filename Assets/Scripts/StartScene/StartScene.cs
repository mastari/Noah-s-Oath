using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour {

    public GameObject camera;
    public float moveSpeed;
    public Button start, options, quit;

	// Use this for initialization
	void Start () {
        camera = Camera.main.gameObject;
        
        start.onClick.AddListener(StartGame);
        options.onClick.AddListener(Options);
        quit.onClick.AddListener(QuitGame);


	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject == camera) {
            camera.transform.rotation = Quaternion.Euler(new Vector3(0, camera.transform.rotation.eulerAngles.y + moveSpeed, 0));
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
	}

    public void StartGame() {
        SceneManager.LoadScene("MainScene");
    }

    public void Options () {

    }

    public void QuitGame() {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
