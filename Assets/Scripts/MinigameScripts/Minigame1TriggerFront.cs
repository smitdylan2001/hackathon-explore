using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame1TriggerFront : MonoBehaviour
{

    public static bool hasEnteredHoop;
    public GameObject MinigamePrefab;

    private void Start()
    {
        hasEnteredHoop = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I have entered BACK Hoop" + Minigame1TriggerBack.HasEnteredHoop);
        if (Minigame1TriggerBack.HasEnteredHoop == true)
        {
            GameManager.Instance.IncreaseScore();
            Destroy(MinigamePrefab);
        }
        else
        {
            hasEnteredHoop = true;
        }
    }

}
