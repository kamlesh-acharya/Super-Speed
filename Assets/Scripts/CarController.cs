using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private bool isAI;

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

    [SerializeField]
    private AudioSource engineSound;
    [SerializeField]
    private AudioSource skidSound;
    [SerializeField]
    private float skidFadeTime = 2f;

    private int nextCheckPoint;
    [SerializeField]
    private int currentLap = 1;

    [SerializeField]
    private float lapTime, bestLapTime;

    [SerializeField]
    private float resetCoolDown = 2f;
    private float resetCounter;

    //AI variables
    [SerializeField]
    private int currentTarget;
    private Vector3 targetPoint;
    [SerializeField]
    private float aiAccelerateSpeed = 1f, aiTurnSpeed = 0.8f, aiReachPointRange = 5f, aiPointVariance = 3f, aiMaxTurn = 15f;
    private float aiSpeedInput, aiSpeedMod;


    // Start is called before the first frame update
    void Start()
    {
        theRB.transform.parent = null;
        dragOnGround = theRB.drag;
        UIManager.Instance.SetLapCounterText(currentLap);

        //AI related code
        if (isAI)
        {
            targetPoint = RaceManager.Instance.GetAllCheckPoints()[currentTarget].transform.position;
            RandomiseAITarget();

            aiSpeedMod = Random.Range(0.8f, 1.1f);
        }

        resetCounter = resetCoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (!RaceManager.Instance.GetIsStarting())
        {
            lapTime += Time.deltaTime;

            if (!isAI)
            {
                var ts = System.TimeSpan.FromSeconds(lapTime);
                UIManager.Instance.SetCurrentLapTimeText(string.Format("{0:00}m{1:00}.{2:000}s", ts.Minutes, ts.Seconds, ts.Milliseconds));

                speedInput = 0f;

                //Moving forward Backward
                if (Input.GetAxis("Vertical") > 0)
                {
                    speedInput = Input.GetAxis("Vertical") * forwardAccelaration;
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    speedInput = Input.GetAxis("Vertical") * reverseAccelaration;

                }

                //Turning Left and Right
                turnInput = Input.GetAxis("Horizontal");

                /* if(isGrounded && Input.GetAxis("Vertical") != 0)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStregth * Time.deltaTime * Mathf.Sign(speedInput) * (theRB.velocity.magnitude / maxSpeed), 0f));
                } */
                //transform.position = theRB.transform.position;

                if(resetCounter > 0)
                {
                    resetCounter -= Time.deltaTime;
                }

                if (Input.GetKeyDown(KeyCode.R) && resetCounter <= 0)
                {
                    ResetToTrack();
                }
            }
            else
            {
                targetPoint.y = transform.position.y;

                if (Vector3.Distance(transform.position, targetPoint) < aiReachPointRange)
                {
                    SetNextAITarget();
                }

                Vector3 targetDir = targetPoint - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);

                Vector3 localPos = transform.InverseTransformPoint(targetPoint);

                if (localPos.x < 0f)
                {
                    angle = -angle;
                }

                turnInput = Mathf.Clamp(angle / aiMaxTurn, -1f, 1f);

                if (Mathf.Abs(angle) < aiMaxTurn)
                {
                    aiSpeedInput = Mathf.MoveTowards(aiSpeedInput, 1f, aiAccelerateSpeed);
                }
                else
                {
                    aiSpeedInput = Mathf.MoveTowards(aiSpeedInput, aiTurnSpeed, aiAccelerateSpeed);
                }

                //aiSpeedInput = 1f;
                speedInput = aiSpeedInput * forwardAccelaration * aiSpeedMod;
            }

            //Turning Wheels
            leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, leftFrontWheel.localRotation.eulerAngles.z);
            rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn), rightFrontWheel.localRotation.eulerAngles.z);


            //Control Particle Emission
            emissionRate = Mathf.MoveTowards(emissionRate, 0f, emissionFadeSpeed * Time.deltaTime);

            if ((isGrounded && Mathf.Abs(turnInput) > 0.5f) || (theRB.velocity.magnitude < maxSpeed * 0.5f && theRB.velocity.magnitude != 0))
            {
                emissionRate = maxEmission;
            }

            if (theRB.velocity.magnitude <= 0.5f)
            {
                emissionRate = 0;
            }

            for (int i = 0; i < dustTrail.Length; i++)
            {
                var emissionModule = dustTrail[i].emission;

                emissionModule.rateOverTime = emissionRate;
            }

            if (engineSound != null)
            {
                engineSound.pitch = 1f + ((theRB.velocity.magnitude / maxSpeed) * 2f);
            }

            if (skidSound != null)
            {
                if (Mathf.Abs(turnInput) > 0.5f && theRB.velocity.magnitude > maxSpeed * 0.2f)
                {
                    skidSound.volume = 1f;
                }
                else
                {
                    skidSound.volume = Mathf.MoveTowards(skidSound.volume, 0f, skidFadeTime * Time.deltaTime);
                }
            }
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

        //Debug.Log("Speed: " + theRB.velocity.magnitude);

        transform.position = theRB.transform.position;

        if(isGrounded && speedInput != 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStregth * Time.deltaTime * Mathf.Sign(speedInput) * (theRB.velocity.magnitude / maxSpeed), 0f));

        }

        //if (turnInput != 0)
        //{
        //    if (!skidSound.isPlaying && isGrounded)
        //    {
        //        skidSound.Play();
        //    }
        //}
        //else
        //{
        //    skidSound.Stop();
        //}
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public void SetMaxSpeed(float _maxSpeed)
    {
        maxSpeed = _maxSpeed;
    }

    public int GetNextCheckPoint()
    {
        return nextCheckPoint;
    }

    public int GetCurrentLap()
    {
        return currentLap;
    }

    public Rigidbody GetTheRB()
    {
        return theRB;
    }

    public void SetIsAI(bool value)
    {
        isAI = value;
    }

    public void SetCarPosition(Transform _transform)
    {
        transform.position = _transform.position;
        theRB.transform.position = transform.position;
    }

    public void CheckPointHit(int cpNumber)
    {
        if(cpNumber == nextCheckPoint)
        {
            nextCheckPoint++;

            if(nextCheckPoint == RaceManager.Instance.GetAllCheckPoints().Length)
            {
                nextCheckPoint = 0;
                LapCompleted();
            }
        }

        if(isAI)
        {
            if(cpNumber == currentTarget)
            {
                SetNextAITarget();
            }
        }
    }

    public void SetNextAITarget()
    {
        currentTarget++;
        if (currentTarget >= RaceManager.Instance.GetAllCheckPoints().Length)
        {
            currentTarget = 0;
        }
        targetPoint = RaceManager.Instance.GetAllCheckPoints()[currentTarget].transform.position;
        RandomiseAITarget();
    }

    public void RandomiseAITarget()
    {
        targetPoint += new Vector3(Random.Range(-aiPointVariance, aiPointVariance), 0f, Random.Range(-aiPointVariance, aiPointVariance));
    }

    public void LapCompleted()
    {
        currentLap++;

        if(lapTime < bestLapTime || bestLapTime == 0)
        {
            bestLapTime = lapTime;
        }

        if(currentLap <= RaceManager.Instance.GetTotalLaps())
        {
            lapTime = 0;

            if (!isAI)
            {
                var ts = System.TimeSpan.FromSeconds(bestLapTime);
                UIManager.Instance.SetBestLapTimeText(string.Format("{0:00}m{1:00}.{2:000}s", ts.Minutes, ts.Seconds, ts.Milliseconds));
                UIManager.Instance.SetLapCounterText(currentLap);
            }
        } 
        else
        {
            if (!isAI)
            {
                isAI = true;
                aiSpeedMod = 1f;

                targetPoint = RaceManager.Instance.GetAllCheckPoints()[currentTarget].transform.position;
                RandomiseAITarget();

                var ts = System.TimeSpan.FromSeconds(bestLapTime);
                UIManager.Instance.SetBestLapTimeText(string.Format("{0:00}m{1:00}.{2:000}s", ts.Minutes, ts.Seconds, ts.Milliseconds));

                RaceManager.Instance.FinishRace();
            }
        }

    }

    private void ResetToTrack()
    {
        int pointToGo = nextCheckPoint - 1;
        if(pointToGo < 0)
        {
            pointToGo = RaceManager.Instance.GetAllCheckPoints().Length - 1;
        }

        transform.position = RaceManager.Instance.GetAllCheckPoints()[pointToGo].transform.position;
        theRB.transform.position = transform.position;
        theRB.velocity = Vector3.zero;

        speedInput = 0f;
        turnInput = 0f;

        resetCounter = resetCoolDown;
    }
}
