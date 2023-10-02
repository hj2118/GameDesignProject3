using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI livesInterface;
    public TextMeshProUGUI timeInterface;
    public TextMeshProUGUI bestTimeInterface;
    public TextMeshProUGUI highestInterface;

    private void Awake()
    {
        if (GameObject.FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        print(publicvar.bestTime);
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            publicvar.timePlayed += Time.deltaTime;

            if (publicvar.playerDead)
            {
                SceneManager.LoadScene("LoseScene");
            }

            livesInterface.text = "Lives: " + publicvar.lives;
            timeInterface.text = string.Format("Time: {0:00}:{1:00}", Mathf.FloorToInt(publicvar.timePlayed / 60), Mathf.FloorToInt(publicvar.timePlayed % 60));
        }

        else if (SceneManager.GetActiveScene().name == "TutorialScene")
        {
            publicvar.timePlayed += Time.deltaTime;

            if (publicvar.playerDead)
            {
                publicvar.playerDead = false;
                publicvar.lives = 4;
            }

            livesInterface.text = "Lives: " + publicvar.lives;
            timeInterface.text = string.Format("Time: {0:00}:{1:00}", Mathf.FloorToInt(publicvar.timePlayed / 60), Mathf.FloorToInt(publicvar.timePlayed % 60));
        }

        else if (SceneManager.GetActiveScene().name == "WinScene")
        {
            checkBestTime();

            if (publicvar.best) {
                highestInterface.text = "HIGHEST SCORE";
            }
            else
            {
                highestInterface.text = "GAME COMPLETE";

            }
            timeInterface.text = string.Format("Current: {0:00}:{1:00}", Mathf.FloorToInt(publicvar.timePlayed / 60), Mathf.FloorToInt(publicvar.timePlayed % 60));
            bestTimeInterface.text = string.Format("Highest: {0:00}:{1:00}", Mathf.FloorToInt(publicvar.bestTime / 60), Mathf.FloorToInt(publicvar.bestTime % 60));
        }

        /*
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (SceneManager.GetActiveScene().name == "StartScene")
            {
                SceneManager.LoadScene("TutorialScene");
                publicvar.timePlayed = 0;
                publicvar.complete = false;
            }
            else
            {
                SceneManager.LoadScene("StartScene");
                publicvar.complete = false;
                publicvar.timePlayed = 0;
            }
        }
        */
        if (publicvar.complete)
        {
            if (SceneManager.GetActiveScene().name == "TutorialScene")
            {
                publicvar.timePlayed = 0;
                publicvar.lives = 3;
                publicvar.complete = false;
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                publicvar.complete = false;
                SceneManager.LoadScene("WinScene");
            }
        }

        /*
#if !UNITY_WEBGL
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
#endif
        */
    }

    public void StartButton()
    {
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            SceneManager.LoadScene("TutorialScene");
            publicvar.timePlayed = 0;
            publicvar.complete = false;
        }
        else if (SceneManager.GetActiveScene().name == "TutorialScene")
        {
            SceneManager.LoadScene("GameScene");
            publicvar.timePlayed = 0;
            publicvar.complete = false;
            publicvar.lives = 3;
        }
        else
        {
            SceneManager.LoadScene("GameScene");
            publicvar.complete = false;
            publicvar.timePlayed = 0;
            publicvar.lives = 3;
        }

        Destroy(gameObject);
    }

    public void QuitButton()
    {
#if !UNITY_WEBGL
        Application.Quit();
#endif
        SceneManager.LoadScene("StartScene");
        publicvar.complete = false;
        publicvar.timePlayed = 0;
        publicvar.lives = 3;
        publicvar.bestTime = 0;

        Destroy(gameObject);
    }

    public void checkBestTime()
    {
        if ((publicvar.bestTime == 0) || (publicvar.bestTime >= publicvar.timePlayed))
        {
            publicvar.bestTime = publicvar.timePlayed;
            publicvar.best = true;
        }
        else
        {
            publicvar.best = false;
        }
    }
}
