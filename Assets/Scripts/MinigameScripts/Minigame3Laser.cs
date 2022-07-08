using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame3Laser : MonoBehaviour
{
    LineRenderer _lineRenderer;
    Camera _camera;
    Vector3[] _lineVectors = new Vector3[2];
    public GameObject Laser;

    private void Start()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _camera = Camera.main;

    }

    void Update()
    {

        if (GameObject.FindGameObjectWithTag("Minigame3") == null)
        {
            GameManager.Instance.IncreaseScore();
            Destroy(Laser);
            Destroy(this.gameObject);
        }
    }

}
