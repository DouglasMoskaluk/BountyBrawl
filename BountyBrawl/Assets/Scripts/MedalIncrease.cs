using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalIncrease : MonoBehaviour
{
    public void IncreaseStatsShown()
    {
        PodiumMoneyText[] podiums = FindObjectsOfType<PodiumMoneyText>();

        foreach(PodiumMoneyText p in podiums)
        {
            p.PlayMedalMove();
        }
    }
}