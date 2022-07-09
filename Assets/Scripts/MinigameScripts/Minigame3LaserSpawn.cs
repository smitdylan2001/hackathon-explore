using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Minigame3LaserSpawn : MonoBehaviour
{
    Camera _camera;
    public GameObject Laser;
    public GameObject LaserVFXReference;
    private GameObject LaserVFX;

    void Start()
    {
        _camera = Camera.main;
        Laser.transform.SetParent(_camera.transform);
        Laser.transform.localPosition = new Vector3(0, -0.3f, 0);
        Laser.transform.localRotation = Quaternion.Euler(Vector3.zero);

        LaserVFX = Instantiate(LaserVFXReference, Laser.transform);
    }

    private void Update()
    {
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(Laser.transform.position, Laser.transform.forward, out RaycastHit hit))
        {
            LaserVFX.SetActive(true);
            LaserVFX.transform.position = hit.point + (Laser.transform.position - hit.point).normalized * 0.05f;

            if (hit.transform.CompareTag("Minigame3"))
            {
                DestroyGameObject(hit.transform.gameObject);
            }
        }
        else
        {
            LaserVFX.SetActive(false);
        }
    }

    async void DestroyGameObject(GameObject balloon)
    {
        await Task.Delay(250);
        if (balloon)
        {
            Destroy(balloon);
        }

    }

}
