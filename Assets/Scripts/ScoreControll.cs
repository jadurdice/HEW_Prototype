using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreControll : MonoBehaviour
{
    public Text Score_Text; //テキストオブジェクトを取得
   public int Score;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Score_Text.text = Score.ToString();
    }
}
