using UnityEngine;
using System.Collections.Generic;
using Common;

public class NoteManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject prfbNote;           //ノーツprefab.
    [SerializeField] GameObject InPrefab;

    [Header("- レーン -")]
    [SerializeField] float[]    laneAngle;          //レーンごとの角度.

    [Header("- ノーツデータ -")]
    [SerializeField] NoteChart  noteChart;          //ノーツ譜面データ.

    [Header("- 挙動設定 -")]
    [SerializeField] Vector3    goalPos;            //目標地点.
    [SerializeField] float      dist = 1;           //距離.
    [SerializeField] float      moveTime = 1;       //何秒で中心に移動するか.
    [SerializeField] float      destroyTime = 1;    //中心到達後、何秒で消滅するか.

    [Header("- 判定設定 -")]
    [SerializeField] float      badDist = 1;        //BAD判定になる距離.
    [SerializeField] float      goodDist = 1;       //GOOD判定になる距離.
    [SerializeField] float      perfectDist = 1;    //PERFECT判定になる距離.

    //ノーツ配列.
    List<GameObject> noteList = new();

    int noteIndex; //次に生成するノーツの番号.

    void Start()
    {
        noteIndex = 0;
    }

    void Update()
    {
        // まだノーツが残っていれば.
        if (noteIndex < noteChart.noteDatas.Length)
        {
            NoteData noteData = noteChart.noteDatas[noteIndex];

            // 到達時刻から移動時間を引いて出現時刻を計算.
            float spawnTime = noteData.time - moveTime;

            // 出現時刻になったらノーツを生成.
            if (Time.time >= spawnTime)
            {
                SpawnNote(noteData);
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

        //レーンの角度を取得.
        float angle = laneAngle[data.laneNo];
        //角度から方向ベクトルを作成.
        Vector3 vec = new Vector3(
            Mathf.Cos(angle),
            Mathf.Sin(angle),
            0
        );
        //スタート位置の計算.
        Vector3 startPos = goalPos + vec * dist;

        //初期設定.
        scptNote.Init(moveTime, destroyTime, startPos, goalPos);
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

        //ノーツをタップしたら(BAD判定になる距離以内なら)
        if (nearestNote != null && nearestDist < badDist)
        {
            //ノーツ判定.
            Result ret = JudgeNote(nearestDist);

            //リザルト別処理.
            switch (ret)
            {
                case Result.Perfect:
                    //TODO
                    break;
                case Result.Good:
                    //TODO
                    break;
                case Result.Bad:
                    //TODO
                    break;

                default: Debug.Log("不正な値です"); break;
            }

            //ノーツ消滅.
            nearestNote.GetComponent<Note>().Destroy();
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
