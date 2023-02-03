using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CarController target;
    private Vector3 offSetDir;

    [SerializeField]
    private float minDistance = 20, maxDistance = 50;
    private float actionDistance;

    [SerializeField]
    private Transform startTargetOffser;

    [SerializeField]
    private Rigidbody targetRB;

    // Start is called before the first frame update
    void Start()
    {
        //offSetDir = transform.position - target.transform.position;
        offSetDir = transform.position - startTargetOffser.position;

        actionDistance = minDistance;

        offSetDir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        actionDistance = minDistance + ((maxDistance - minDistance) * (targetRB.velocity.magnitude / target.GetMaxSpeed()));
        transform.position = target.transform.position + (offSetDir * actionDistance);
    }

}
