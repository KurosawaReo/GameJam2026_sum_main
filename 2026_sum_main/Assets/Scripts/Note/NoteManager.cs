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
    [SerializeField] GaugeManager       gaugeMng;

    [Header("- setting -")]
    [SerializeField] LaneSetting        laneSetting;
    [SerializeField] NoteChartSetting   noteChartSetting;
    [SerializeField] SoundSetting       soundSetting;

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
        //まだノーツが残っていれば.
        if (noteIndex < noteChartSetting.noteDatas.Length)
        {
            NoteData noteData = noteChartSetting.noteDatas[noteIndex];

            //BPMからノーツの到達時間を計算.
            float noteTime = soundSetting.GetTime(noteData.beatCount);
            //現在の曲の再生位置を取得.
            float currentTime = SoundManager.Inst.GetTimeBGM();
            //到達時間から移動時間を引いて、ノーツの出現時間を計算.
            float spawnTime = noteTime - laneSetting.moveTime;

            //曲の再生位置が出現時間に達したらノーツを生成.
            if (currentTime >= spawnTime)
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
        var objNote = Instantiate(prfbNote, inPrfbNote.transform);
        var scptNote = objNote.GetComponent<Note>();
        //ノーツをリストに登録.
        noteList.Add(objNote);

        //レーンのスタート位置を取得.
        Vector3 startPos = GetLaneStartPos(data.laneNo);
        //ノーツが判定位置に到達するBGM時間を取得.
        float noteTime = soundSetting.GetTime(data.beatCount);
        //到達時のプレイヤー画像を取得.
        Sprite imgPlayer = objPlayer.GetComponent<Player>().GetAfterImage(
            data.parts,
            noteTime
        );

        //初期設定.
        scptNote.Init(
            imgPlayer, data.laneNo, noteTime, laneSetting.moveTime, laneSetting.destroyTime, startPos, laneSetting.goalPos
        );
    }

    /// <summary>
    /// 指定レーンを基準に、同時押しのノーツをまとめて判定.
    /// </summary>
    public void JudgeNearestNote(int laneNo, Vector3 playerPos)
    {
        // 指定されたレーンの最寄りノーツを取得.
        GameObject baseNote = GetNearestNote(laneNo, playerPos, out float baseDist);

        // ノーツがない、または判定範囲外なら終了.
        if (baseNote == null || baseDist >= laneSetting.badDist)
        {
            return;
        }

        // 基準ノーツの判定時間を取得.
        Note baseNoteComponent = baseNote.GetComponent<Note>();
        float baseTime = baseNoteComponent.GetNoteTime();

        // 同時押しとして判定するノーツを取得.
        List<GameObject> judgeNotes = GetSameTimeNotes(baseTime);

        // 判定結果をまとめて処理.
        ResultNote(judgeNotes);
    }

    /// <summary>
    /// 指定レーンの最寄りノーツを取得.
    /// </summary>
    private GameObject GetNearestNote(int laneNo, Vector3 playerPos, out float nearestDist)
    {
        // 最寄りノーツを初期化.
        GameObject nearestNote = null;

        // 最短距離を最大値で初期化.
        nearestDist = float.MaxValue;

        // 全ノーツを確認.
        foreach (GameObject objNote in noteList)
        {
            // 既に削除されたノーツは無視.
            if (objNote == null)
            {
                continue;
            }

            Note note = objNote.GetComponent<Note>();

            // 指定レーン以外は無視.
            if (note.GetLaneNo() != laneNo)
            {
                continue;
            }

            // プレイヤーとの距離を計算.
            float dist = Vector3.Distance(playerPos, objNote.transform.position);

            // より近いノーツなら更新.
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearestNote = objNote;
            }
        }

        return nearestNote;
    }

    /// <summary>
    /// 指定した時間と同時押し扱いにするノーツを取得.
    /// </summary>
    private List<GameObject> GetSameTimeNotes(float baseTime)
    {
        // 同時押し対象のノーツ.
        List<GameObject> sameTimeNotes = new();

        // 同時押しとみなす時間差.
        const float sameTimeRange = 0.01f;

        // 全ノーツを確認.
        foreach (GameObject objNote in noteList)
        {
            // 既に削除されたノーツは無視.
            if (objNote == null)
            {
                continue;
            }

            Note note = objNote.GetComponent<Note>();

            // ノーツの判定時間を取得.
            float noteTime = note.GetNoteTime();

            // 基準ノーツとの時間差を計算.
            float timeDiff = Mathf.Abs(noteTime - baseTime);

            // 同じタイミングなら同時押し対象に追加.
            if (timeDiff <= sameTimeRange)
            {
                sameTimeNotes.Add(objNote);
            }
        }

        return sameTimeNotes;
    }

    /// <summary>
    /// 同時押しノーツの結果処理.
    /// </summary>
    private void ResultNote(List<GameObject> judgeNotes)
    {
        // 判定済みノーツを先にリストから削除.
        // Destroy()はフレーム終了時までGameObjectが残るため、
        // 同じフレーム内で再判定されるのを防ぐ.
        foreach (GameObject objNote in judgeNotes)
        {
            if (objNote != null)
            {
                noteList.Remove(objNote);
            }
        }

        // グループ内で最も良い判定を保存.
        Result bestResult = Result.Bad;

        // 判定対象のノーツを1つずつ処理.
        foreach (GameObject objNote in judgeNotes)
        {
            if (objNote == null)
            {
                continue;
            }

            Note note = objNote.GetComponent<Note>();

            // プレイヤーとの距離を取得.
            float dist = Vector3.Distance(
                objPlayer.transform.position,
                objNote.transform.position
            );

            // ノーツ単体の判定を取得.
            Result result = JudgeNote(dist);

            // より良い判定なら更新.
            if (result < bestResult)
            {
                bestResult = result;
            }

            // スコアはノーツごとに送信.
            ScoreManager.instance.SendResult(result);

            Debug.Log("result:" + result);

            // 判定したノーツを消滅.
            note.Destroy();
        }

        // グループ内で最も良い判定の演出だけ出す.
        switch (bestResult)
        {
            case Result.Perfect:
                Instantiate(prfbEffPerfect, inPrfbEff.transform);
                gaugeMng.OnPerfect();
                break;

            case Result.Good:
                Instantiate(prfbEffGood, inPrfbEff.transform);
                gaugeMng.OnGood();
                break;

            case Result.Bad:
                Instantiate(prfbEffBad, inPrfbEff.transform);
                gaugeMng.OnBad();
                break;

            default:
                Debug.Log("不正な値です");
                break;
        }
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
