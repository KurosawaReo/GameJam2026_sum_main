using UnityEngine;
using System.Collections.Generic;
using Common;

public class NoteManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject         prfbNote;           //ノーツprefab.
    [SerializeField] GameObject         InPrefab;

    [Header("- script -")]
    [SerializeField] GameObject         objPlayer;          //プレイヤー.

    [Header("- レーン -")]
    [SerializeField] float[]            laneAngle;          //レーンごとの角度.
    [SerializeField] Vector3            goalPos;            //目標地点.
    [SerializeField] float              dist = 1;           //距離.
    [SerializeField] float              moveTime = 1;       //何秒で中心に移動するか.
    [SerializeField] float              destroyTime = 1;    //中心到達後、何秒で消滅するか.

    [Header("- ノーツデータ -")]
    [SerializeField] NoteChartSetting   noteChart;          //ノーツ譜面データ.

    [Header("- 判定設定 -")]
    [SerializeField] float              badDist = 1;        //BAD判定になる距離.
    [SerializeField] float              goodDist = 1;       //GOOD判定になる距離.
    [SerializeField] float              perfectDist = 1;    //PERFECT判定になる距離.

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
        //レーンのスタート位置を取得.
        Vector3 startPos = GetLaneStartPos(data.laneNo);

        //プレイヤーと重なる瞬間の画像が何になるかを計算.
        Sprite imgPlayer;
        {
            float time = Time.time + moveTime;                                  //未来の時間.
            imgPlayer = objPlayer.GetComponent<Player>().GetAfterImage(time);   //未来の画像を求める.
        }

        //初期設定.
        scptNote.Init(imgPlayer, moveTime, destroyTime, startPos, goalPos);
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

    /// <summary>
    /// レーンのスタート座標を取得.
    /// </summary>
    private Vector3 GetLaneStartPos(int laneNo)
    {
        // レーンの角度を取得.
        float angle = laneAngle[laneNo] * Mathf.Deg2Rad;

        // 角度から方向ベクトルを作成.
        Vector3 vec = new Vector3(
            Mathf.Cos(angle),
            Mathf.Sin(angle),
            0
        );

        // ゴール地点から距離分離れた位置を返す.
        return goalPos + vec * dist;
    }

    /// <summary>
    /// 【デバッグ用】レーンの軌道をGizmoで表示.
    /// </summary>
    void OnDrawGizmos()
    {
        //色設定.
        Gizmos.color = new Color(0.1f, 1.0f, 1.0f);
        //目標地点を表示.
        Gizmos.DrawWireSphere(goalPos, 0.15f);

        //全レーンを表示.
        for (int i = 0; i < laneAngle.Length; i++)
        {
            //レーンのスタート位置を取得.
            Vector3 startPos = GetLaneStartPos(i);

            //スタート地点と軌道を表示.
            Gizmos.DrawWireSphere(startPos, 0.1f);
            Gizmos.DrawLine(startPos, goalPos);
        }
    }
}
