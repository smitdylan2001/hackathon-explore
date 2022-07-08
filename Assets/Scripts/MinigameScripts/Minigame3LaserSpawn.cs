using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame3LaserSpawn : MonoBehaviour
{

    Camera _camera;
    public GameObject Laser;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        Laser.transform.SetParent(_camera.transform);
        Laser.transform.localPosition = new Vector3(0,-0.3f,0);
        Laser.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }


}
