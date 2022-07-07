using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame2StayZone : MonoBehaviour
{

    //public int Minigame1Hoops.Seconds;
    private bool isInArea;
    IEnumerator co;

    private void Start()
    {
        isInArea = false;
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ik raak blok aan en" +isInArea);
        isInArea = true;
        co = ZoneTimer();
        StartCoroutine(co);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Ik raak blok NIET meer aan en" + isInArea);
        isInArea = false;
        co = ZoneTimer();
        StopCoroutine(co);
    }

    IEnumerator ZoneTimer()
    {
        yield return new WaitForSeconds(3f);

        if(isInArea)
        Destroy(this.gameObject);
    }
}
