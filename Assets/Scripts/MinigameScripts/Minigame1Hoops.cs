using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame1Hoops : MonoBehaviour
{

    public GameObject MinigamePrefab;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ik raak hoop aan");
        //GameManager.Instance.IncreaseScore();
        Destroy(MinigamePrefab);
    }
}
