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

    protected Rigidbody rb;
    protected Quaternion startRotation;
    protected int frameCnt;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startRotation = motor[0].localRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var forceDirection = transform.forward;
        var forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);


        if (Input.GetKeyDown(KeyCode.A))
        {
            rb.AddForceAtPosition(transform.right * steerPower / 100.0f, motor[0].position);
            frameCnt = fadeFrame;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.AddForceAtPosition(-1.0f * transform.right * steerPower / 100.0f, motor[1].position);
            frameCnt = fadeFrame;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rb.AddForceAtPosition(transform.right * steerPower / 100.0f, motor[2].position);
            frameCnt = -1 * fadeFrame;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            rb.AddForceAtPosition(-1.0f * transform.right * steerPower / 100.0f, motor[3].position);
            frameCnt = -1 * fadeFrame;
        }


        if (frameCnt != 0)
        {
            PhysicsHelper.ApplyForceToReachVelocity(rb, frameCnt * forward * maxSpeed * transform.localScale.y, power);
            frameCnt = frameCnt > 0 ? frameCnt - 1 : frameCnt + 1;

        }

    }
}
