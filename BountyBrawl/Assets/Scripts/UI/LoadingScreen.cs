using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    
    void OnEnable()
    {
        StartCoroutine(TheAssEater5000());
    }

    float randomNumber = Random.Range(2, 8);

    IEnumerator TheAssEater5000()
    {
        yield return new WaitForSeconds(randomNumber);
    }

    void Update()
    {
        
    }
}
