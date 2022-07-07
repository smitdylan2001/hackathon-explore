using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame3Laser : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitCheck, 10f))
        {
            Debug.Log("I hit something");
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitCheck.distance, Color.red);
            Destroy(hitCheck.transform.gameObject);
        }
        else 
        {
            Debug.Log("I hit nothing");
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 30f, Color.green);
        }
    }
}
