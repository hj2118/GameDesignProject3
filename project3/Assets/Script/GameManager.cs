using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI livesInterface;
    public TextMeshProUGUI timeInterface;

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            publicvar.timePlayed += Time.deltaTime;

            if (publicvar.playerDead)
            {
                SceneManager.LoadScene("LoseScene");
            }

            livesInterface.text = "Lives: " + publicvar.lives;
            //timeInterface.text = "Time: " + Mathf.FloorToInt(publicvar.timePlayed / 60) + ":" + Mathf.FloorToInt(publicvar.timePlayed % 60);
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
            //timeInterface.text = "Time: " + Mathf.FloorToInt(publicvar.timePlayed / 60) + ":" + Mathf.FloorToInt(publicvar.timePlayed % 60);
            timeInterface.text = string.Format("Time: {0:00}:{1:00}", Mathf.FloorToInt(publicvar.timePlayed / 60), Mathf.FloorToInt(publicvar.timePlayed % 60));
        }

        else if (SceneManager.GetActiveScene().name == "WinScene")
        {
            timeInterface.text = string.Format("Time: {0:00}:{1:00}", Mathf.FloorToInt(publicvar.timePlayed / 60), Mathf.FloorToInt(publicvar.timePlayed % 60));
            //timeInterface.text = "Time: " + Mathf.FloorToInt(publicvar.timePlayed / 60) + ":" + Mathf.FloorToInt(publicvar.timePlayed % 60);
        }

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

#if !UNITY_WEBGL
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
#endif
    }
}
