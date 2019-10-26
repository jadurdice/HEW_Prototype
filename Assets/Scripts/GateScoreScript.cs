using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScoreScript : MonoBehaviour
{
    private bool plus;



    public GameObject Score_Controll;
    ScoreControll ScoreScript;
    
    // Start is called before the first frame update
    void Start()
    {
        plus = false;

        Score_Controll = GameObject.Find("Score_Controll");
        ScoreScript = Score_Controll.GetComponent<ScoreControll>();

    }

    // Update is called once per frame
    void Update()
    {


    }



    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.CompareTag("Player")&&!plus)
        {
           ScoreScript.Score += 200;

            plus = true;
        }
        

    }

}
