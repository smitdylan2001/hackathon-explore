using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame3Laser : MonoBehaviour
{
    LineRenderer _lineRenderer;
    Camera _camera;
    Vector3[] _lineVectors = new Vector3[2];
    private void Start()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _camera = Camera.main;

        UpdateLine();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLine();

        if (GameObject.FindGameObjectWithTag("Minigame3") == null)
        {
            GameManager.Instance.IncreaseScore();
            Destroy(this.gameObject);
        }
    }

    void UpdateLine()
    {
        _lineVectors[0] = _camera.transform.position;
        _lineVectors[1] = _camera.transform.transform.position + _camera.transform.transform.forward * 5;

        _lineRenderer.SetPositions(_lineVectors);
    }
}
