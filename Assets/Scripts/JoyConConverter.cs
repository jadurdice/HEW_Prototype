using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JoyConConverter : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float limitForce;
    protected Vector3 angle;
    protected Vector3 originPoint;
    protected Rigidbody rb;

    // Update is called once per frame

    private void Start()
    {
        originPoint = new Vector3(0.0f, 0.0f, 90.0f);
    }

    void Update()
    {
        Vector3 show = new Vector3((Input.GetAxisRaw("1")) * -5.0f, Input.GetAxisRaw("y") * 0.3f, (Input.GetAxisRaw("2")) * -5.0f);



        if(show.sqrMagnitude < limitForce)
        {
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles,originPoint,Time.deltaTime);
        }
        else
        {
            transform.localEulerAngles += show;
        }

        text.text = "(" + show.ToString() + ")";



        if (Input.GetKey(KeyCode.Space))
        {
            transform.localEulerAngles = originPoint;
        }

        
    }
}
