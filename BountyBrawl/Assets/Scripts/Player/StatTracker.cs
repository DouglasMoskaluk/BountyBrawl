using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
    private PlayerBody player;
    private int playerCharacter;
    private int playerSkin;

    [SerializeField] private float totalMoney; //Holds the total amount of money player made
    [SerializeField] private int playerKills; //Holds the total amount of enemy player kills
    [SerializeField] private int LostKills; //Holds the total amount of lost kills
    [SerializeField] private int EaterKills; //Holds the total amount of Eater kills
    [SerializeField] private float playerDamage; //Holds the total amount of enemy player damage made
    [SerializeField] private float enemyDamage; //Holds the total amount of enemy damage made

    private void Awake()
    {
        Reset();
    }

    public void Reset()
    {
        totalMoney = 0;
        playerKills = 0;
        LostKills = 0;
        EaterKills = 0;
        playerDamage = 0;
        enemyDamage = 0;
        player = null;
    }

    public void SetPlayer(PlayerBody p)
    {
        player = p;
        playerCharacter = p.getCharacter();
        playerSkin = p.playerSkin;
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

    public float GetMoney() { return totalMoney; }
    public int GetKills() { return playerKills; }
    public int GetLost() { return LostKills; }
    public int GetEater() { return EaterKills; }
    public float GetDamage() { return playerDamage; }
    public float GetLostDamage() { return enemyDamage; }

}
