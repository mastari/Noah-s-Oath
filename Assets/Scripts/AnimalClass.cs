using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalClass : MonoBehaviour {

    public string AnimalType;
    public GameObject Player;
    public static List<string> Animals = new List<string>();

    public AnimalClass (string AnimalType) {
        this.AnimalType = AnimalType;
        Animals.Add(this.AnimalType);
        Player = Instantiate(Resources.Load(this.AnimalType) as GameObject);

        Player.name = AnimalType.Substring(0, AnimalType.Length);

        Player.transform.position = new Vector3(0, 0.5f, 0);

        var rb = Player.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        ManageHealth();
        ManageInventory();
        SetupCamera(true);
    }

    public GameObject GetPlayer () {
        return Player;
    }

    public void Move (float MovementSpeed) {
        var Move = Player.AddComponent<Move>();
        Move.mspeed = MovementSpeed;
        Move.sspeed = MovementSpeed * 2;
    }

    public void Jump (float JumpSpeed) {
        var JumpController = Player.AddComponent<Jump>();
        JumpController.jumpSpeed = JumpSpeed;
    }

    public void ManageHealth () {
        var health = Player.AddComponent<ObjectHealth>();
        var ManageHealth = Player.AddComponent<ManageHealth>();
    }

    public void SetupCamera (bool LockCursor) {
        //var cam = Camera.main;
        //var cam = new GameObject("Camera");
        //cam.AddComponent<Transform>();
        //cam.AddComponent<Camera>();
        //cam.AddComponent<GUILayer>();
        //cam.AddComponent<FlareLayer>();
        //cam.AddComponent<AudioListener>();
        //var lr = cam.AddComponent<LineRenderer>();
        //lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        //lr.receiveShadows = false;
        //lr.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;
        //lr.size
        //var cam = Instantiate(StartScript.camera);
        //cam.SetActive(true);
        //cam.name = cam.name.Substring(0, cam.name.Length);

        ////cam.gameObject.AddComponent<CameraController>();
        //cam.transform.parent = Player.transform;
        //cam.transform.position = new Vector3(0, 1, 0);

        //var cc = cam.GetComponent<CameraController>();
        //cc.target = Player.transform;
        //cc.speed = 100f;
        //cc.lockCursor = LockCursor;

        Camera.main.GetComponent<CameraController>().target = Player.transform;
        Camera.main.transform.parent = Player.transform;
        Camera.main.transform.position = new Vector3(0, 1, 0);
    }

    public void ManageInventory () {
        var inv = Player.AddComponent<InventoryScript>();
    }

    public void Shoot (float BulletTime) {
        var Shoot = Player.AddComponent<Shoot>();
        Shoot.bulletTime = BulletTime;
    }

}
