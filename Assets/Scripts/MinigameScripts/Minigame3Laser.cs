using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame3Laser : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Minigame3") == null)
        {
            //GameManager.Instance.IncreaseScore();
            Debug.Log("I hit EVERYTHINGs");
            Destroy(this.gameObject);
        }
    }
}
