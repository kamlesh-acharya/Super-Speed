using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private int checkPointNumber;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCheckPointNumber(int cPNumber)
    {
        checkPointNumber = cPNumber;
    }

    public int GetCheckPointNumber()
    {
        return checkPointNumber;
    }
}
