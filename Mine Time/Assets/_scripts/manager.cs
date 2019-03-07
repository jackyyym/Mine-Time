using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class manager : MonoBehaviour
{
    public GameObject controller;
    public GameObject title, game;
    // Update is called once per frame

    public void playbutton()
    {
        Time.timeScale = 1;
        title.SetActive(false);
        game.SetActive(true);
        controller.GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().Play();
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GetComponent<AudioSource>().Play();
    }
}
