using UnityEngine;
using System.Collections.Generic;
using Common;

public class NoteManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject prfbNote;           //ノーツprefab.
    [SerializeField] GameObject InPrefab;

    [Header("- lane -")]
    [SerializeField] float[]    laneAngle;          //レーンごとの角度.

    [Header("- note -")]
    [SerializeField] NoteChart  noteChart;          //ノーツ譜面データ.
    [SerializeField] Vector3    goalPos;            //目標地点.
    [SerializeField] float      dist;               //距離.
    [SerializeField] float      speed;              //移動速度.

    [Header("- judge -")]
    [SerializeField] float      noteJudgeMaxDist;   //ノーツが届く最大範囲.
    [SerializeField] float      perfectDist;        //PERFECT判定になる距離.
    [SerializeField] float      goodDist;           //GOOD判定になる距離.

    //ノーツ配列.
    List<GameObject> noteList = new();

    int noteIndex; //次に生成するノーツの番号.

    void Start()
    {
        noteIndex = 0;
    }

    void Update()
    {
        //まだノーツが残っていれば.
        if (noteIndex < noteChart.noteDatas.Length)
        {
            //指定時間になったらノーツを生成.
            if (Time.time >= noteChart.noteDatas[noteIndex].time)
            {
                SpawnNote(noteChart.noteDatas[noteIndex]);
                noteIndex++;
            }
        }
    }

    /// <summary>
    /// ノーツを生成.
    /// </summary>
    private void SpawnNote(NoteData data)
    {
        //ノーツ生成.
        var objNote  = Instantiate(prfbNote, InPrefab.transform);
        var scptNote = objNote.GetComponent<Note>();
        //ノーツをリストに登録.
        noteList.Add(objNote);
        //速度と目標地点を設定.
        scptNote.Init(speed, goalPos);

        //レーンの角度を取得.
        float angle = laneAngle[data.laneNo];
        //角度から方向ベクトルを作成.
        Vector3 vec = new Vector3(
            Mathf.Cos(angle),
            Mathf.Sin(angle),
            0
        );

        //初期位置を設定.
        objNote.transform.position = goalPos + vec * dist;
    } 

    /// <summary>
    /// 最寄りのノーツを判定.
    /// </summary>
    public void JudgeNearestNote(Vector3 playerPos)
    {
        //最寄りのノーツobject.
        GameObject nearestNote = null;
        //距離計測用.
        float nearestDist = float.MaxValue;

        //全てのノーツループ.
        foreach (GameObject objNote in noteList)
        {
            //nullになったノーツは無視.
            if (objNote == null)
            {
                continue;
            }

            //プレイヤーとの距離を計算.
            float dist = Vector3.Distance(playerPos, objNote.transform.position);

            //現在の最短距離より近ければ更新.
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearestNote = objNote;
            }
        }

        Debug.Log("nearestDist:" + nearestDist);


        //一定距離内のノーツをタップしたら.
        if (nearestNote != null && nearestDist < noteJudgeMaxDist)
        {
            var ret = JudgeNote(nearestDist); //ノーツ判定.
            Debug.Log("ret:" + ret);

            var scptNote = nearestNote.GetComponent<Note>();
            scptNote.Destroy();     //ノーツ消滅.
        }
    }

    /// <summary>
    /// ノーツの判定を取得.
    /// </summary>
    public Result JudgeNote(float dist)
    {
        //距離が近いほど良い判定.
        if (dist <= perfectDist)
        {
            return Result.Perfect;
        }
        if (dist <= goodDist)
        {
            return Result.Good;
        }
        //ミスもBad判定.
        return Result.Bad;
    }
}
