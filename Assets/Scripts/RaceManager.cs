using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    [SerializeField]
    private Checkpoint[] allCheckpoints;
    [SerializeField]
    private int totalLaps = 5;

    [SerializeField]
    private CarController playerCar;
    [SerializeField]
    private List<CarController> allAICars = new List<CarController>();
    private int playerPosition;
    [SerializeField]
    private float timeBetweenPosCheck = 0.2f;
    private float posCheckCounter;

    [SerializeField]
    private float aiDefaultSpeed = 30f, playerDefaultSpeed = 30f, rubberBandSpeedMod = 3.5f, rubberBandAcceleration = 0.5f;

    [SerializeField]
    private bool isStarting;
    [SerializeField]
    private float timeBetweenStartCount = 1f;
    private float startCounter;
    [SerializeField]
    private int countdownCurrent = 3;

    [SerializeField]
    private int playerStartPosition, aiNumberToSpawn;
    [SerializeField]
    private Transform[] startPoints;

    [SerializeField]
    private List<CarController> carsToSpawn = new List<CarController>();

    private bool raceCompleted;
    [SerializeField]
    private string raceCompleteScene;



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

        isStarting = true;
        startCounter = timeBetweenStartCount;
        UIManager.Instance.SetCountDownText(countdownCurrent.ToString());

        playerStartPosition = Random.Range(0, aiNumberToSpawn + 1);

        playerCar.SetCarPosition(startPoints[playerStartPosition].transform);

        for(int i = 0; i < aiNumberToSpawn + 1; i++)
        {
            if(i != playerStartPosition)
            {
                int selectedCar = Random.Range(0, carsToSpawn.Count);
                allAICars.Add(Instantiate(carsToSpawn[selectedCar], startPoints[i].position, startPoints[i].rotation));
                if(carsToSpawn.Count > aiNumberToSpawn - i)
                {
                    carsToSpawn.RemoveAt(selectedCar);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarting)
        {
            startCounter -= Time.deltaTime;
            if(startCounter <= 0)
            {
                countdownCurrent--;
                startCounter = timeBetweenStartCount;
                UIManager.Instance.SetCountDownText(countdownCurrent.ToString());

                if(countdownCurrent == 0)
                {
                    isStarting = false;
                    UIManager.Instance.ShowHideCountDownText(false);
                    UIManager.Instance.ShowHideGoText(true);
                }
            }
        }
        else
        {
            posCheckCounter -= Time.deltaTime;

            if (posCheckCounter <= 0)
            {
                playerPosition = 1;

                foreach (CarController aiCar in allAICars)
                {
                    if (aiCar.GetCurrentLap() > playerCar.GetCurrentLap())
                    {
                        playerPosition++;
                    }
                    else if (aiCar.GetCurrentLap() == playerCar.GetCurrentLap())
                    {
                        if (aiCar.GetNextCheckPoint() > playerCar.GetNextCheckPoint())
                        {
                            playerPosition++;
                        }
                        else if (aiCar.GetNextCheckPoint() == playerCar.GetNextCheckPoint())
                        {
                            if (Vector3.Distance(aiCar.transform.position, allCheckpoints[aiCar.GetNextCheckPoint()].transform.position) < Vector3.Distance(playerCar.transform.position, allCheckpoints[playerCar.GetNextCheckPoint()].transform.position))
                            {
                                playerPosition++;
                            }
                        }
                    }
                }
                posCheckCounter = timeBetweenPosCheck;
                UIManager.Instance.SetPlayerPositionText(playerPosition);
            }

            if(playerPosition == 1)
            {
                foreach(CarController aiCars in allAICars)
                {
                    aiCars.SetMaxSpeed(Mathf.MoveTowards(aiCars.GetMaxSpeed(), aiDefaultSpeed + rubberBandSpeedMod, rubberBandAcceleration * Time.deltaTime));
                }

                playerCar.SetMaxSpeed(Mathf.MoveTowards(playerCar.GetMaxSpeed(), playerDefaultSpeed - rubberBandSpeedMod, rubberBandAcceleration * Time.deltaTime));
            }
            else
            {
                foreach (CarController aiCars in allAICars)
                {
                    aiCars.SetMaxSpeed(Mathf.MoveTowards(aiCars.GetMaxSpeed(), aiDefaultSpeed - ((float)rubberBandSpeedMod / ((float)GetAllCarsCount())), rubberBandAcceleration * Time.deltaTime));
                }

                playerCar.SetMaxSpeed(Mathf.MoveTowards(playerCar.GetMaxSpeed(), playerDefaultSpeed + ((float)rubberBandSpeedMod / ((float)GetAllCarsCount())), rubberBandAcceleration * Time.deltaTime));
            }
        }
    }

    public Checkpoint[] GetAllCheckPoints()
    {
        return allCheckpoints;
    }

    public int GetTotalLaps()
    {
        return totalLaps;
    }

    public int GetAllCarsCount()
    {
        return allAICars.Count + 1;
    }

    public bool GetIsStarting()
    {
        return isStarting;
    }

    public void FinishRace()
    {
        raceCompleted = true;

        UIManager.Instance.SetRaceResultText(playerPosition);
        UIManager.Instance.ShowHideResultPanel(true);
    }

    public void ExitRace()
    {
        SceneManager.LoadScene(raceCompleteScene);
    }
}
