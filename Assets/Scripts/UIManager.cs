using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text currentLapNumberText, bestLapTimeText, currentLapTimeText, playerPositionText, countdownText, goText, raceResultText;

    [SerializeField]
    private GameObject resultScreen, pauseScreen;

    private bool isPaused;

    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("UIManager is Null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        isPaused = !isPaused;
        pauseScreen.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
        } else
        {
            Time.timeScale = 1f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ExitRace()
    {
        Time.timeScale = 1f;
        RaceManager.Instance.ExitRace();
    }

    public void SetLapCounterText(int currentLap)
    {
        if (currentLapNumberText != null)
        {
            currentLapNumberText.text = currentLap + "/" + RaceManager.Instance.GetTotalLaps();
        }
    }

    public void SetCurrentLapTimeText(string text)
    {
        if (currentLapTimeText != null)
        {
            currentLapTimeText.text = text;
        }
    }

    public void SetBestLapTimeText(string text)
    {
        if (bestLapTimeText != null)
        {
            bestLapTimeText.text = text;
        }
    }

    public void SetPlayerPositionText(int currentPosition)
    {
        if (playerPositionText != null)
        {
            playerPositionText.text = currentPosition + "/" + RaceManager.Instance.GetAllCarsCount();
        }
    }

    public void SetCountDownText(string text)
    {
        if (countdownText != null)
        {
            countdownText.text = text;
        }
    }

    public void ShowHideCountDownText(bool flag)
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(flag);
        }
    }

    public void ShowHideGoText(bool flag)
    {
        if (goText != null)
        {
            goText.gameObject.SetActive(flag);
        }
    }

    public void SetRaceResultText(int position)
    {
        string playerPosition;
        switch (position)
        {
            case 1: playerPosition = position + "st"; break;
            case 2: playerPosition = position + "nd"; break;
            case 3: playerPosition = position + "rd"; break;
            default: playerPosition = position + "th"; break;
        }
        raceResultText.text = "You finished " + playerPosition;
    }

    public void ShowHideResultPanel(bool flag)
    {
        if (resultScreen != null)
        {
            resultScreen.gameObject.SetActive(flag);
        }
    }

}
