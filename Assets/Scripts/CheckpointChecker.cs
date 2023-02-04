using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointChecker : MonoBehaviour
{
    [SerializeField]
    private CarController theCar;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Checkpoint")
        {
            theCar.CheckPointHit(other.GetComponent<Checkpoint>().GetCheckPointNumber());
        }
    }
}
