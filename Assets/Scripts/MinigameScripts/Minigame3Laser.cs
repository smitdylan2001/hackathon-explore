using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame3Laser : MonoBehaviour
{
    public GameObject Laser;

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
