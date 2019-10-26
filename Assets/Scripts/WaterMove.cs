using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMove : MonoBehaviour
{

    public Transform[] motor;
    public float steerPower = 500.0f;
    public float power = 5.0f;
    public float maxSpeed = 10.0f;
    public float drag = 0.1f;
    public int fadeFrame;

    public int GateSpeed;

    public float boostMulti = 2.0f;
    public int boostFrame;

    protected Rigidbody rb;
    protected float airDrag;
    protected Quaternion startRotation;
    protected int frameCnt;
    protected FloatingObject floatObj;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startRotation = motor[0].localRotation;
        floatObj = gameObject.GetComponent<FloatingObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var forceDirection = transform.forward;
        var forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);

        if (floatObj.pointUnderWater)
        {
            airDrag = 1.0f;
        }
        else
        {
            airDrag = 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            rb.AddForceAtPosition(-1.0f * transform.right * steerPower / 100.0f * airDrag, motor[0].position);
            frameCnt = fadeFrame;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.AddForceAtPosition(transform.right * steerPower / 100.0f * airDrag, motor[1].position);
            frameCnt = fadeFrame;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rb.AddForceAtPosition(transform.right * steerPower / 100.0f * airDrag, motor[2].position);
            frameCnt = -1 * fadeFrame;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            rb.AddForceAtPosition(-1.0f * transform.right * steerPower / 100.0f * airDrag, motor[3].position);
            frameCnt = -1 * fadeFrame;
        }




        if (frameCnt != 0)
        {
            if(boostFrame != 0)
            {
                PhysicsHelper.ApplyForceToReachVelocity(rb, frameCnt * forward * maxSpeed * transform.localScale.y, power * boostMulti);
                boostFrame -= 1;
                Debug.Log(boostFrame);
            }
            else
            {
                PhysicsHelper.ApplyForceToReachVelocity(rb, frameCnt * forward * maxSpeed * transform.localScale.y, power);
            }

            if (floatObj.pointUnderWater)
            {
                frameCnt = frameCnt > 0 ? frameCnt - 1 : frameCnt + 1;
            }
            else
            {
                frameCnt = 0;
            }


        }

    }
    public void SetBoost(int boost)
    {
        boostFrame = boost;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rb.AddForceAtPosition(-1.0f * transform.right * GateSpeed / 100.0f * airDrag, motor[0].position);
            rb.AddForceAtPosition(transform.right * GateSpeed / 100.0f * airDrag, motor[1].position);
            frameCnt = -1 * fadeFrame;
        }
    }

}
