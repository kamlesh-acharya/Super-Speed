using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceInfoManager : MonoBehaviour
{
    [SerializeField]
    private string trackToLoad;
    [SerializeField]
    private CarController racerToUse;
    [SerializeField]
    private int noOfAI, noOfLaps;

    private bool enteredRace;
    [SerializeField]
    private Sprite trackSprite, carSprite;

    private static RaceInfoManager _instance;

    public static RaceInfoManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("RaceInfoManager is Null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;

            DontDestroyOnLoad(gameObject);
        } 
        else
        {
            Destroy(gameObject);
        }
    }

    public Sprite GetCarSprite()
    {
        return carSprite;
    }

    public Sprite GetTrackSprite()
    {
        return trackSprite;
    }

    public bool GetEnteredRace()
    {
        return enteredRace;
    }

    public void SetEnteredRace(bool value)
    {
        enteredRace = value;
    }

    public void SetTrackSprite(Sprite _trackSprite)
    {
        trackSprite = _trackSprite;
    }

    public void SetCarSprite(Sprite _carSprite)
    {
        carSprite = _carSprite;
    }
    
    public string GetTrackToLoad()
    {
        return trackToLoad;
    }

    public CarController GetRacerToUse()
    {
        return racerToUse;
    }

    public int GetNoOfAI()
    {
        return noOfAI;
    }

    public int GetNoOfLaps()
    {
        return noOfLaps;
    }
    public void SetTrackToLoad(string trackSceneName)
    {
        trackToLoad = trackSceneName;
    }

    public void SetCarToUse(CarController carToSet)
    {
        racerToUse = carToSet;
    }

    public void SetNoOfLaps(int raceLaps)
    {
        noOfLaps = raceLaps;
    }
}
