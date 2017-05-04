using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour {

    public GameObject camera;
    public float moveSpeed;
    public GameObject startButton;

	// Use this for initialization
	void Start () {
        camera = Camera.main.gameObject;

        Button btn = startButton.GetComponent<Button>();
        btn.onClick.AddListener(StartGame);
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject == camera) {
            camera.transform.rotation = Quaternion.Euler(new Vector3(0, camera.transform.rotation.eulerAngles.y + moveSpeed, 0));
        }
	}

    public void StartGame() {
        SceneManager.LoadScene("MainScene");
    }
}
