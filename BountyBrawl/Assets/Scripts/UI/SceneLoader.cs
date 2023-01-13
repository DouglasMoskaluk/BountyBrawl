using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    //public float randomNumber;
    public void Start()
    {
        //float randomNumber = Random.Range(2, 8);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    
    IEnumerator TheAssEater5000(int index)
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + index);
    }

    public void Map1()
    {
        StartCoroutine(TheAssEater5000(1));
    }

    public void Map2()
    {
        StartCoroutine(TheAssEater5000(2));
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }    
}
