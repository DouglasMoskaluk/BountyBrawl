using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
    PlayerBody player;

    [SerializeField] private float currMoney; //Holds the current money of the player
    [SerializeField] private float totalMoney; //Holds the total amount of money player made
    [SerializeField] private int playerKills; //Holds the total amount of enemy player kills
    [SerializeField] private int LostKills; //Holds the total amount of lost kills
    [SerializeField] private int EaterKills; //Holds the total amount of Eater kills
    [SerializeField] private float playerDamage; //Holds the total amount of enemy player damage made
    [SerializeField] private float enemyDamage; //Holds the total amount of enemy damage made

    private void Awake()
    {
        currMoney = 0;
        totalMoney = 0;
        playerKills = 0;
        LostKills = 0;
        EaterKills = 0;
        playerDamage = 0;
        enemyDamage = 0;
    }

    public void IncreaseCurrMoney(float moola)
    {
        currMoney += moola;
    }
    
    public void IncreaseTotalMoney(float moola)
    {
        totalMoney += moola;
    }

    public void IncreasePlayerKills()
    {
        playerKills++;
    }

    public void IncreaseLostKills()
    {
        LostKills++;
    }

    public void IncreaseEaterKills()
    {
        EaterKills++;
    }

    public void IncreasePlayerDamage(float inPlayerDam)
    {
        playerDamage += inPlayerDam;
    }

    public void IncreaseEnemyDamage(float inEnemyDam)
    {
        enemyDamage += inEnemyDam;
    }

    public float GetCurrMoney()
    {
        return currMoney;
    }
}
