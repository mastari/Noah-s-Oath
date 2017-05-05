using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    public static bool paused;
    public GameObject menu;
    public Button back, options, leave, quit;

 
	void Start () {
        Pause(false);
        back.onClick.AddListener(Back);
        options.onClick.AddListener(Options);
        leave.onClick.AddListener(Leave);
        quit.onClick.AddListener(Quit);
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Pause(!paused);
            Debug.Log("Paused");
        }
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
        Application.Quit();
    }
}
