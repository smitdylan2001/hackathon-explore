using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame4PullDrawer : MonoBehaviour
{
    public GameObject MinigamePrefab;

    private void OnTriggerExit(Collider other)
    {

        GameManager.Instance.IncreaseScore();
        Destroy(MinigamePrefab);

    }
}
