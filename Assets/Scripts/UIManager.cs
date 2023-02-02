using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text currentLapNumberText, bestLapTimeText, currentLapTimeText;


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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLapCounterText(int currentLap)
    {
        currentLapNumberText.text = currentLap + "/" + RaceManager.Instance.GetTotalLaps();
    }

    public void SetCurrentLapTimeText(string text)
    {
        currentLapTimeText.text = text;
    }

    public void SetBestLapTimeText(string text)
    {
        bestLapTimeText.text = text;
    }
}
