using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//修正点19.10.26

//1
//項目：public関数AddScoreを追加
//理由：他スクリプトから直接アクセスすることを防ぐ、保守性を向上させるため
//2
//項目：各変数名の改変
//理由：コーディング条約に合わせるため

public class ScoreControll : MonoBehaviour
{
    public Text score_Text; //テキストオブジェクトを取得
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        score_Text.text = score.ToString();
    }

    public void AddScore(int add)
    {
        score += add;
    }
}
