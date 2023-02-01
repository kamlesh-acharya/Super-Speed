﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody theRB;
    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private float forwardAccelaration = 8f, reverseAccelaration = 4f;
    private float speedInput;

    [SerializeField]
    private float turnStregth = 180f;
    private float turnInput;

    private bool isGrounded;
    [SerializeField]
    private Transform groundRayPoint, groundRayPoint2;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private float groundRayLength = 0.75f;

    private float dragOnGround;
    [SerializeField]
    private float gravityMod = 10f;

    [SerializeField]
    private Transform leftFrontWheel, rightFrontWheel;
    [SerializeField]
    private float maxWheelTurn = 25f;

    [SerializeField]
    private ParticleSystem[] dustTrail;
    [SerializeField]
    private float maxEmission = 25f, emissionFadeSpeed = 20f;
    private float emissionRate;

    // Start is called before the first frame update
    void Start()
    {
        theRB.transform.parent = null;
        dragOnGround = theRB.drag;

    }

    // Update is called once per frame
    void Update()
    {
        speedInput = 0f;

        //Moving forward Backward
        if(Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccelaration;
        } else if(Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * reverseAccelaration;

        }

        //Turning Left and Right
        turnInput = Input.GetAxis("Horizontal");

        if(isGrounded && Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStregth * Time.deltaTime * Mathf.Sign(speedInput) * (theRB.velocity.magnitude / maxSpeed), 0f));
        }
        transform.position = theRB.transform.position;

        //Turning Wheels
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn), rightFrontWheel.localRotation.eulerAngles.z);


        //Control Particle Emission
        emissionRate = Mathf.MoveTowards(emissionRate, 0f, emissionFadeSpeed * Time.deltaTime);

        if ((isGrounded && Mathf.Abs(turnInput) > 0.5f) || (theRB.velocity.magnitude < maxSpeed * 0.5f && theRB.velocity.magnitude != 0))
        {
            emissionRate = maxEmission;
        }

        if(theRB.velocity.magnitude <= 0.5f)
        {
            emissionRate = 0;
        }

        for(int i = 0; i < dustTrail.Length; i++)
        {
            var emissionModule = dustTrail[i].emission;

            emissionModule.rateOverTime = emissionRate;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = false;

        RaycastHit hit;
        Vector3 normalTarget = Vector3.zero;

        if(Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            isGrounded = true;

            normalTarget = hit.normal;
        }

        if (Physics.Raycast(groundRayPoint2.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            isGrounded = true;

            normalTarget = (normalTarget + hit.normal) / 2f;
        }

        //When on ground rotate to match the normal which is calculated above from 2 points
        if (isGrounded)
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, normalTarget) * transform.rotation;
        }

        //accelerates the car and adding gravity mod when car is in the air
        if (isGrounded)
        {
            theRB.drag = dragOnGround;
            theRB.AddForce(transform.forward * speedInput * 1000f);
        } 
        else
        {
            theRB.drag = 0.1f;
            theRB.AddForce(-Vector3.up * gravityMod * 100f);
        }

        //Locking max speed
        if(theRB.velocity.magnitude > maxSpeed)
        {
            theRB.velocity = theRB.velocity.normalized * maxSpeed;
        }
        Debug.Log("Speed: " + theRB.velocity.magnitude);
    }
}