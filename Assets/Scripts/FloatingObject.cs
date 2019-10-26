using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FloatingObject : MonoBehaviour
{

    public float airDrag = 1;
    public float waterDrag = 10;
    public Transform[] floatPoint;
    public bool attachToSurface;

    protected Rigidbody rb;
    protected Wave wave;

    protected float waterLine;
    protected Vector3[] waterLinePoint;

    public Vector3 centerOffset;
    public Vector3 smoothVectorRotation;
    public Vector3 targetUp;

    public bool pointUnderWater = false;


    public Vector3 center { get { return transform.position + centerOffset; } }

    private void Awake()
    {
        wave = FindObjectOfType<Wave>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        waterLinePoint = new Vector3[floatPoint.Length];
        for (int i = 0; i < floatPoint.Length; i++)
        {
            waterLinePoint[i] = floatPoint[i].position;
        }
        centerOffset = PhysicsHelper.GetCenter(waterLinePoint) - transform.position;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var newWaterLine = 0f;
        pointUnderWater = false;

        for (int i = 0; i < floatPoint.Length; i++)
        {
            waterLinePoint[i] = floatPoint[i].position;
            waterLinePoint[i].y = wave.GetHeight(floatPoint[i].position);
            newWaterLine += waterLinePoint[i].y / floatPoint.Length;
            if(waterLinePoint[i].y > floatPoint[i].position.y)
            {
                pointUnderWater = true;
            }
        }

        var waterLineDelta = newWaterLine - waterLine;
        waterLine = newWaterLine;

        var grv = Physics.gravity;
        rb.drag = airDrag;
        if(waterLine > center.y)
        {
            rb.drag = waterDrag;

            if (attachToSurface)
            {
                rb.position = new Vector3(
                rb.position.x,
                waterLine - centerOffset.y,
                rb.position.z);
            }
            else
            {
                grv = -Physics.gravity;
                transform.Translate(Vector3.up * waterLineDelta * 0.9f);
            }

        }
        rb.AddForce(grv * Mathf.Clamp(Mathf.Abs(waterLine - center.y), 0, 1));

        targetUp = PhysicsHelper.GetNormal(waterLinePoint);

        if (pointUnderWater)
        {
            targetUp = Vector3.SmoothDamp(transform.up,targetUp,ref smoothVectorRotation,0.2f);
            rb.rotation = Quaternion.FromToRotation(transform.up,targetUp) * rb.rotation;
        }

    }

    private void OnDrawGizmos()
    {
        if(floatPoint == null)
        {
            return;
        }

        for (int i = 0; i < floatPoint.Length; i++)
        {
            if(floatPoint[i] == null)
            {
                continue;
            }

            if(wave != null)
                {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(waterLinePoint[i], Vector3.one * 0.3f);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(floatPoint[i].position,0.1f);

        }

        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(center.x,waterLine,center.z), Vector3.one * 1f);
        }

    }
}
