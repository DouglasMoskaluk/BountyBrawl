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
    [SerializeField] private float brawlStay = 1f;

    private Image numberDisplayer;
    private Animator numberAnimator;

    [SerializeField] private AudioSource numberDown;
    [SerializeField] private AudioSource brawlNoise;

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
        //numberDown.Play();

        for (int i = 1; i < countdownLength; i++)
        {
            yield return new WaitForSecondsRealtime(countdownSpeed);
            countdownNumbers.SetActive(false);
            countdownNumbers.SetActive(true);
            numberDisplayer.sprite = numbers[i];
        }

        yield return new WaitForSecondsRealtime(countdownSpeed);
        countdownNumbers.SetActive(false);
        countdownEnd.SetActive(true);
        //brawlNoise.Play();
        yield return new WaitForSecondsRealtime(brawlStay);
        Time.timeScale = 1f;

        PlayerBody[] players = FindObjectsOfType<PlayerBody>();

        foreach(PlayerBody p in players)
        {
            p.canPause = true;
        }

        Camera.main.GetComponent<AudioSource>().Play();

        gameObject.SetActive(false);

    }
}
