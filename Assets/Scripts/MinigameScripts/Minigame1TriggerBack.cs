using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame1TriggerBack : MonoBehaviour
{

    public GameObject MinigamePrefab;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I have entered BACK Hoop" + Minigame1TriggerFront.hasEnteredHoop);
        if (Minigame1TriggerFront.hasEnteredHoop == true)
        {
            GameManager.Instance.IncreaseScore();
            //Minigame1TriggerFront.hasEnteredHoop = false;
            Destroy(MinigamePrefab);
        }
    }
}
