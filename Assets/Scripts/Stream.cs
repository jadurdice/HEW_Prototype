using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
    public Transform flowPoint;
    public float streamForce;

    protected Vector3 streamDirection;


    // Start is called before the first frame update
    void Start()
    {
        streamDirection = Vector3.Normalize(flowPoint.position - transform.position);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().AddForce(streamDirection * streamForce);
        }

    }


}
