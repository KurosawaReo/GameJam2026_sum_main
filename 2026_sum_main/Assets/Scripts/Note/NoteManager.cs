using UnityEngine;
using System.Collections.Generic;
using Common;

public class NoteManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject         prfbNote;       //ノーツprefab.
    [SerializeField] GameObject         inPrfbNote;
    [Space]
    [SerializeField] GameObject         prfbEffPerfect; //演出prefab.
    [SerializeField] GameObject         prfbEffGood;
    [SerializeField] GameObject         prfbEffBad;
    [SerializeField] GameObject         inPrfbEff;

    [Header("- script -")]
    [SerializeField] GameObject         objPlayer; //プレイヤー.

    [Header("- setting -")]
    [SerializeField] LaneSetting        laneSetting;
    [SerializeField] NoteChartSetting   noteChartSetting;

    //ノーツ配列.
    List<GameObject> noteList = new();

    int noteIndex; //次に生成するノーツの番号.

    /// <summary>
    /// レーンのスタート座標を取得.
    /// </summary>
    private Vector3 GetLaneStartPos(int laneNo)
    {
        // レーンの角度を取得.
        float angle = laneSetting.laneAngle[laneNo] * Mathf.Deg2Rad;

        // 角度から方向ベクトルを作成.
        Vector3 vec = new Vector3(
            Mathf.Cos(angle),
            Mathf.Sin(angle),
            0
        );

        // ゴール地点から距離分離れた位置を返す.
        return laneSetting.goalPos + vec * laneSetting.dist;
    }

    void Start()
    {
        noteIndex = 0;
    }

    void Update()
    {
        // まだノーツが残っていれば.
        if (noteIndex < noteChartSetting.noteDatas.Length)
        {
            NoteData noteData = noteChartSetting.noteDatas[noteIndex];

            // 到達時刻から移動時間を引いて出現時刻を計算.
            float spawnTime = noteData.time - laneSetting.moveTime;

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
        var objNote  = Instantiate(prfbNote, inPrfbNote.transform);
        var scptNote = objNote.GetComponent<Note>();
        //ノーツをリストに登録.
        noteList.Add(objNote);
        //レーンのスタート位置を取得.
        Vector3 startPos = GetLaneStartPos(data.laneNo);

        //プレイヤーと重なる瞬間の画像が何になるかを計算.
        Sprite imgPlayer;
        {
            float time = Time.time + laneSetting.moveTime;                      //未来の時間.
            imgPlayer = objPlayer.GetComponent<Player>().GetAfterImage(time);   //未来の画像を求める.
        }

        //初期設定.
        scptNote.Init(
            imgPlayer, laneSetting.moveTime, laneSetting.destroyTime, startPos, laneSetting.goalPos
        );
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
        if (nearestNote != null && nearestDist < laneSetting.badDist)
        {
            ResultNote(nearestDist);

            //ノーツ消滅.
            nearestNote.GetComponent<Note>().Destroy();
        }
    }

    /// <summary>
    /// ノーツの結果処理.
    /// </summary>
    private void ResultNote(float nearestDist)
    {
        //ノーツ判定.
        Result ret = JudgeNote(nearestDist);

        //リザルト別演出.
        switch (ret)
        {
            case Result.Perfect:
                Instantiate(prfbEffPerfect, inPrfbEff.transform);
                break;

            case Result.Good:
                Instantiate(prfbEffGood, inPrfbEff.transform);
                break;

            case Result.Bad:
                Instantiate(prfbEffBad, inPrfbEff.transform);
                break;

            default: Debug.Log("不正な値です"); break;
        }

        //リザルトを送信.
        //TODO: ScoreManager.instance.SendResult(ret);
    }

    /// <summary>
    /// ノーツの判定を取得.
    /// </summary>
    public Result JudgeNote(float dist)
    {
        //距離が近いほど良い判定.
        if (dist <= laneSetting.perfectDist)
        {
            return Result.Perfect;
        }
        if (dist <= laneSetting.goodDist)
        {
            return Result.Good;
        }
        //ミスもBad判定.
        return Result.Bad;
    }

    /// <summary>
    /// 【デバッグ用】レーンの軌道をGizmoで表示.
    /// </summary>
    void OnDrawGizmos()
    {
        //エラー対策.
        if (!laneSetting) { return; }

        //色設定.
        Gizmos.color = new Color(0.1f, 1.0f, 1.0f);
        //目標地点を表示.
        Gizmos.DrawWireSphere(laneSetting.goalPos, 0.15f);

        //全レーンを表示.
        for (int i = 0; i < laneSetting.laneAngle.Length; i++)
        {
            //レーンのスタート位置を取得.
            Vector3 startPos = GetLaneStartPos(i);

            //スタート地点と軌道を表示.
            Gizmos.DrawWireSphere(startPos, 0.1f);
            Gizmos.DrawLine(startPos, laneSetting.goalPos);
        }
    }
}
