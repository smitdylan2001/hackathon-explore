using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame4PullDrawer : MonoBehaviour
{
    public GameObject realParent;
    private bool isHoldingObject;

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Ik hou hem vast" + isHoldingObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetMouseButton(0))
        {
            isHoldingObject = true;
            Debug.Log("Ik hou het object vast");

        }
        else
        {
            isHoldingObject = false;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            isHoldingObject = false;
            Debug.Log("Ik hou het object NITvast");
        }


        if (isHoldingObject)
        {
            other.transform.parent = this.gameObject.transform;
        }
        else
        {
            other.transform.parent = realParent.transform;
        }


    }
}
