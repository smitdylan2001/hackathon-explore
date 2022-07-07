using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame1Hoops : MonoBehaviour
{
    public int Seconds;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ik raak hoop aan");
        Seconds += 30;
        Destroy(this.gameObject);
    }
}
