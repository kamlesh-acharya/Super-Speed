using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cameras;
    private int currentCam;

    [SerializeField]
    private CameraController topDownCam;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera cineCam;

    private static CameraSwitcher _instance;
    public static CameraSwitcher Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("CameraSwitcher is Null");
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentCam++;
            if(currentCam >= cameras.Length)
            {
                currentCam = 0;
            }

            for(int i = 0; i < cameras.Length; i++)
            {
                if(i == currentCam)
                {
                    cameras[i].SetActive(true);
                } else
                {
                    cameras[i].SetActive(false);
                }
            }
        }
    }

    public void SetTarget(CarController playerCar)
    {
        topDownCam.SetTarget(playerCar);
        cineCam.m_Follow = playerCar.transform;
        cineCam.m_LookAt = playerCar.transform;
    }
}
