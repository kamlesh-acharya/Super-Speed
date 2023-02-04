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
}
