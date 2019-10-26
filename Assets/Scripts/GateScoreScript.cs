using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//修正点19.10.26

//1
//項目：通過判定boolをintにして、通過したら増加スコアを0にする
//理由：難易度により通過する時のスコアを直接設定できるようにして、判定状態を省略
//2
//項目：GameObjectからスコア制御スクリプトを探すから直接探すようにする
//理由：ソースファイルの簡略化、一回アドレスアクセスに減少する
//3
//項目：スコア制御スクリプトのスコア変数に直接アクセスからpublic関数にする
//理由：スコア制御スクリプトの保守性向上するため
//4
//項目：ゲートオブジェクトをprefab化
//理由：カプセル化してから繰り返し利用するため
//5
//項目：通過確認コライダーをOnTriggerに変更して独立オブジェクトし、衝突判定をOnTriggerEnterに変更
//理由：プレイヤーオブジェクトが真中に通れない、左のポールに突っ込むとスコアが上がる


public class GateScoreScript : MonoBehaviour
{
    //通過時に増加するスコア
    public int plusScore;

    //スコア制御参照先
    ScoreControll ScoreScript;

    void Start()
    {
        //スコア制御参照先指定
        ScoreScript = FindObjectOfType<ScoreControll>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //衝突判定
        if (other.gameObject.CompareTag("Player"))
        {
            //スコア増加と増加設定値を減少
            ScoreScript.AddScore(plusScore);
            other.GetComponent<WaterMove>().SetBoost(plusScore/2);
            plusScore = 0;
        }
    }
}
