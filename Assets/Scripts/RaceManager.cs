using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [SerializeField]
    private Checkpoint[] allCheckpoints;
    [SerializeField]
    private int totalLaps = 5;

    

    private static RaceManager _instance;
    public static RaceManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("RaceManager is NULL");
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
        for (int i = 0; i < allCheckpoints.Length; i++)
        {
            allCheckpoints[i].SetCheckPointNumber(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Checkpoint[] GetAllCheckPoints()
    {
        return allCheckpoints;
    }

    public int GetTotalLaps()
    {
        return totalLaps;
    }
}
