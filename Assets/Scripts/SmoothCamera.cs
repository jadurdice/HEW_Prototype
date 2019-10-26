using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform viewPoint;
    public float moveSpeed;

    protected float rotSpeed;
    protected float transSpeed;



    // Update is called once per frame
    void Update()
    {
        var originPos = viewPoint.position;
        var originRot = viewPoint.eulerAngles;

        transSpeed = Mathf.Clamp(1.0f + Vector3.Distance(originPos, transform.position),1.0f,3.0f);
        rotSpeed = Mathf.Clamp(1.0f + Vector3.Distance(originRot, transform.eulerAngles),1.0f,3.0f);

        this.transform.position = Vector3.Lerp(transform.position, viewPoint.position, Time.deltaTime*moveSpeed * transSpeed);
        this.transform.localRotation = Quaternion.Lerp(transform.localRotation, viewPoint.rotation, Time.deltaTime*moveSpeed*0.25f*rotSpeed);
    }
}
