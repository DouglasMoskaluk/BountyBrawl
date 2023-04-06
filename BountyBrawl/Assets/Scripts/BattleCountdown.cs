using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCountdown : MonoBehaviour
{
    [SerializeField] private Sprite[] numbers;
    [SerializeField] private GameObject countdownEnd;
    [SerializeField] private GameObject countdownNumbers;

    [SerializeField] private float countdownSpeed = 0.5f;

    private Image numberDisplayer;
    private Animator numberAnimator;

    // Start is called before the first frame update
    void Start()
    {
        numberDisplayer = countdownNumbers.GetComponent<Image>();
        numberAnimator = countdownNumbers.GetComponent<Animator>();
        numberDisplayer.sprite = numbers[0];
        countdownEnd.SetActive(false);

        Time.timeScale = 0f;

        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        int countdownLength = numbers.Length;

        for(int i = 1; i < countdownLength; i++)
        {
            yield return new WaitForSecondsRealtime(countdownSpeed);
            countdownNumbers.SetActive(false);
            countdownNumbers.SetActive(true);
            numberDisplayer.sprite = numbers[i];
        }

        yield return new WaitForSecondsRealtime(countdownSpeed);
        countdownNumbers.SetActive(false);
        countdownEnd.SetActive(true);
        yield return new WaitForSecondsRealtime(countdownSpeed);
        Time.timeScale = 1f;

        PlayerBody[] players = FindObjectsOfType<PlayerBody>();

        foreach(PlayerBody p in players)
        {
            p.canPause = true;
        }

        gameObject.SetActive(false);

    }
}
