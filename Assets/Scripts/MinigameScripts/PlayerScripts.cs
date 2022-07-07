using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScripts : MonoBehaviour
{
    public GameObject realParent;
    private bool isHoldingObject;
    private Camera _cam;



    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Minigame 3 mechanics
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitCheck, 10f) && hitCheck.transform.CompareTag ("Minigame3"))
        {
            Debug.Log("I hit something");
            Debug.DrawRay(transform.position, transform.forward * hitCheck.distance, Color.red);
            Destroy(hitCheck.transform.gameObject);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 30f, Color.green);
        }
    }

    private void OnTriggerStay(Collider other)
    {

        //Minigame 4 mechanics
        if (Input.GetMouseButton(0) && other.CompareTag("Minigame4"))
        {
            isHoldingObject = true;
            Debug.Log("Ik hou het object vast");

        }
        else
        {
            isHoldingObject = false;
        }

        if (Input.GetMouseButtonUp(0) && other.CompareTag("Minigame4"))
        {
            isHoldingObject = false;
            Debug.Log("Ik hou het object NITvast");
        }


        if (isHoldingObject)
        {
            //other.transform.parent = this.gameObject.transform;
        }
        else
        {
            //other.transform.parent = realParent.transform;
        }


    }
}
