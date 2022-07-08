using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame1TriggerFront : MonoBehaviour
{

    public static bool hasEnteredHoop;

    private void Start()
    {
        hasEnteredHoop = false;
    }

    private void Update()
    {
        //Debug.Log("I have entered Hoop" + hasEnteredHoop);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I have entered FRONT Hoop" + hasEnteredHoop);
        hasEnteredHoop = true;
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    hasEnteredHoop = false;
    //}
}
