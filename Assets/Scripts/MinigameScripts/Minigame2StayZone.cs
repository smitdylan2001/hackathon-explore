using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame2StayZone : MonoBehaviour
{


    private bool isInArea;
    public GameObject MinigamePrefab;


    private void Start()
    {
        isInArea = false;
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ik raak blok aan en" +isInArea);
        isInArea = true;
        StartCoroutine(ZoneTimer());
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Ik raak blok NIET meer aan en" + isInArea);
        isInArea = false;
        StopAllCoroutines();
    }

    IEnumerator ZoneTimer()
    {
        yield return new WaitForSeconds(3f);

        if (isInArea)
        {
            GameManager.Instance.IncreaseScore();
            Destroy(MinigamePrefab);
        }
    }
}
