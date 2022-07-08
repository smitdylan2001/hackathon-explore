using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame1TriggerBack : MonoBehaviour
{
    public static bool HasEnteredHoop;
    public GameObject MinigamePrefab;

    private void Start()
    {
        HasEnteredHoop = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I have entered BACK Hoop" + Minigame1TriggerFront.hasEnteredHoop);
        if (Minigame1TriggerFront.hasEnteredHoop == true)
        {
            GameManager.Instance.IncreaseScore();
            Destroy(MinigamePrefab);
        }
        else
        {
            HasEnteredHoop = true;
        }
    }
}
