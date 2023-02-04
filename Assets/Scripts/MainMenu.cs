using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject raceSetupPanel, trackSelectPanel, carSelectPanel;

    [SerializeField]
    private Image trackSelectImage, carSelectImage;

    private static MainMenu _instance;


    public static MainMenu Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("MainMenu is Null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        if (RaceInfoManager.Instance.GetEnteredRace())
        {
            trackSelectImage.sprite = RaceInfoManager.Instance.GetTrackSprite();
            carSelectImage.sprite = RaceInfoManager.Instance.GetCarSprite();

            OpenRaceSetup();
        }
    }

    public void SetTrackSelectImage(Image trackImage)
    {
        trackSelectImage.sprite = trackImage.sprite;
    }

    public void SetCarSelectImage(Image carImage)
    {
        carSelectImage.sprite = carImage.sprite;
    }

    public void StartGame()
    {
        RaceInfoManager.Instance.SetEnteredRace(true);
        SceneManager.LoadScene(RaceInfoManager.Instance.GetTrackToLoad());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenRaceSetup()
    {
        raceSetupPanel.SetActive(true);
    }

    public void CloseRaceSetup()
    {
        raceSetupPanel.SetActive(false);
    }

    public void OpenTrackSelect()
    {
        trackSelectPanel.SetActive(true);
        CloseRaceSetup();
    }

    public void CloseTrackSelect()
    {
        trackSelectPanel.SetActive(false);
        OpenRaceSetup();
    }

    public void OpenCarSelect()
    {
        carSelectPanel.SetActive(true);
        CloseRaceSetup();
    }

    public void CloseCarSelect()
    {
        carSelectPanel.SetActive(false);
        OpenRaceSetup();
    }
}
