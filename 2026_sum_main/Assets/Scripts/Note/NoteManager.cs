using UnityEngine;
using System.Collections.Generic;
using Common;

public class NoteManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject         prfbNote;               //ノーツprefab.
    [SerializeField] GameObject         inPrfbNote;
    [Space]
    [SerializeField] GameObject         prfbEffPerfect;         //演出prefab.
    [SerializeField] GameObject         prfbEffGood;
    [SerializeField] GameObject         prfbEffBad;
    [SerializeField] GameObject         prfbEffPlayerPerfect;   //パーフェクト演出.
    [SerializeField] GameObject         inPrfbEff;
    [Space]
    [SerializeField] GameObject         prfbEffCircleFlame;     //タイミング補助円.
    [SerializeField] GameObject         inPrfbEffCircleFlame;

    [Header("- script -")]
    [SerializeField] GameManager        gameMng;
    [SerializeField] GaugeManager       gaugeMng;
    [SerializeField] ScoreManager       scoreMng;

    [Header("- object -")]
    [SerializeField] GameObject         objPlayer;              //プレイヤー.

    [Header("- note chart -")]
    [SerializeField] NoteChartSetting   noteChartNormal;
    [SerializeField] NoteChartSetting   noteChartExtra;

    [Header("- setting -")]
    [SerializeField] LaneSetting        laneSetting;
    [SerializeField] SoundSetting       soundSetting;

    //ノーツ配列.
    List<GameObject> noteList = new();
    //次に生成するノーツの番号.
    int noteIndex;

    //最後に生成した円の拍.
    float lastCircleBeat = -1f;

    //今回の入力ですでにノーツを判定したか.
    bool isJudgedThisInput = false;
    //ゲーム開始時の初期化が完了したか.
    bool isInitialized = false;

    /// <summary>
    /// 使用する譜面を取得.
    /// </summary>
    private NoteChartSetting UseNoteChart
    {
        get
        {
            //譜面を返す.
            switch (AllSceneData.Inst.ChartType)
            {
                case NoteChartType.Normal:
                    return noteChartNormal;

                case NoteChartType.Extra:
                    return noteChartExtra;

                default: 
                    Debug.Log("不正な値です。");
                    return noteChartNormal;
            }
        }
    }

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
        // ノーツ生成位置を初期化.
        noteIndex = 0;
        // 初期化待ち.
        isInitialized = false;
    }

    void Update()
    {
        if (!SoundManager.Inst) { return; }

        //BGMが開始されるまでノーツ処理を開始しない.
        if (!gameMng.IsBgmStarted) { return; }

        //ノーツ生成処理.
        if (noteIndex < UseNoteChart.noteDatas.Length)
        {
            NoteData noteData = UseNoteChart.noteDatas[noteIndex];

            //ノーツの判定時間を取得.
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

        //判定時間を過ぎたノーツをMiss処理.
        JudgeMissNotes();
    }

    /// <summary>
    /// 1回の入力での判定開始.
    /// Playerからクリックされた時に呼ぶ.
    /// </summary>
    public void BeginJudge()
    {
        //今回の入力ではまだ判定していない状態にする.
        isJudgedThisInput = false;
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
            imgPlayer, data.laneNo, noteTime, laneSetting.moveTime, startPos, laneSetting.goalPos
        );

        // 同じ拍の円がまだ生成されていない場合だけ生成.
        if (!Mathf.Approximately(lastCircleBeat, data.beatCount))
        {
            var objCircle = Instantiate(prfbEffCircleFlame, inPrfbEffCircleFlame.transform);

            // ノーツと同じ拍数を設定.
            objCircle.GetComponent<EffectCircleFlame>().Init(data.beatCount);
            // 生成した円の拍を記録.
            lastCircleBeat = data.beatCount;
        }
    }

    /// <summary>
    /// ノーツの判定を取得.
    /// </summary>
    public Result JudgeNote(float diffBeat)
    {
        //拍のズレが小さいほど良い判定.
        if (diffBeat <= laneSetting.perfectBeat)
        {
            return Result.Perfect;
        }

        if (diffBeat <= laneSetting.goodBeat)
        {
            return Result.Good;
        }

        //BAD判定.
        return Result.Bad;
    }

    /// <summary>
    /// 全レーンから時間的に最も近いノーツを判定.
    /// 同じタイミングのノーツは1クリックでまとめて判定する.
    /// </summary>
    public void JudgeNearestNote()
    {
        //今回の入力ですでに判定済みなら何もしない.
        if (isJudgedThisInput)
        {
            return;
        }

        //全レーンから最も近いノーツを取得.
        GameObject baseNote = GetNearestNote(out float baseDiffBeat);

        //ノーツがない、または判定範囲外なら終了.
        if (baseNote == null || baseDiffBeat > laneSetting.badBeat)
        {
            return;
        }

        //この入力では判定済みにする.
        isJudgedThisInput = true;

        //基準となるノーツの拍を取得.
        Note baseNoteComponent = baseNote.GetComponent<Note>();
        float baseBeat = soundSetting.GetBeat(baseNoteComponent.GetNoteTime());

        //判定用の現在拍を取得.
        float currentBeat = soundSetting.GetBeat(
            SoundManager.Inst.GetTimeBGM()
        );

        //拍のズレから判定結果を1回だけ決定.
        Result result = JudgeNote(baseDiffBeat);
#if false
        //判定時の詳細を表示.
        Debug.Log(
            $"Judge / Current:{currentBeat:F3} / Target:{baseBeat:F3} / Diff:{currentBeat - baseBeat:F3} / Result:{result}"
        );
#endif

        //基準ノーツと同じタイミングのノーツを全レーンから取得.
        List<GameObject> judgeNotes = GetSameTimeNotes(baseBeat);

        //判定対象をリストから削除.
        foreach (GameObject objNote in judgeNotes)
        {
            if (objNote != null)
            {
                noteList.Remove(objNote);
            }
        }

        //ノーツの数だけスコア・ゲージを処理.
        foreach (GameObject objNote in judgeNotes)
        {
            if (objNote == null)
            {
                continue;
            }

            Note note = objNote.GetComponent<Note>();

            //判定済みなので通常のDestroy.
            note.Destroy();

            //ノーツ1個分としてスコア・ゲージを加算.
            switch (result)
            {
                case Result.Perfect:
                    OnPerfect();
                    break;

                case Result.Good:
                    OnGood();
                    break;

                case Result.Bad:
                    OnBad();
                    break;
            }
        }

        //演出は同時押し全体で1回だけ.
        switch (result)
        {
            case Result.Perfect:
                OnPerfectEffect();
                break;

            case Result.Good:
                OnGoodEffect();
                break;

            case Result.Bad:
                OnBadEffect();
                break;
        }
    }

    /// <summary>
    /// 全レーンから時間的に最も近いノーツを取得.
    /// </summary>
    private GameObject GetNearestNote(out float nearestDiffBeat)
    {
        //最寄りノーツを初期化.
        GameObject nearestNote = null;

        //最小の拍ズレを最大値で初期化.
        nearestDiffBeat = float.MaxValue;

        //現在のBGM時間を現在の拍に変換.
        float currentTime = SoundManager.Inst.GetTimeBGM();
        float currentBeat = soundSetting.GetBeat(currentTime);

        //全ノーツを確認.
        foreach (GameObject objNote in noteList)
        {
            //既に削除されたノーツは無視.
            if (objNote == null)
            {
                continue;
            }

            Note note = objNote.GetComponent<Note>();

            //ノーツの判定時間を取得.
            float noteTime = note.GetNoteTime();

            //ノーツの判定時間を拍に変換.
            float noteBeat = soundSetting.GetBeat(noteTime);

            //現在の拍とノーツの拍のズレを取得.
            float diffBeat = Mathf.Abs(currentBeat - noteBeat);

            //現在の入力時刻に最も近いノーツを保存.
            if (diffBeat < nearestDiffBeat)
            {
                nearestDiffBeat = diffBeat;
                nearestNote = objNote;
            }
        }

        return nearestNote;
    }

    /// <summary>
    /// 指定した拍と同じタイミングのノーツを取得.
    /// </summary>
    private List<GameObject> GetSameTimeNotes(float baseBeat)
    {
        //同時押し対象のノーツ.
        List<GameObject> sameTimeNotes = new();

        //全ノーツを確認.
        foreach (GameObject objNote in noteList)
        {
            //既に削除されたノーツは無視.
            if (objNote == null)
            {
                continue;
            }

            Note note = objNote.GetComponent<Note>();

            //ノーツの判定時間を取得.
            float noteTime = note.GetNoteTime();

            //ノーツの判定時間を拍に変換.
            float noteBeat = soundSetting.GetBeat(noteTime);

            //同じ拍なら同時押し対象に追加.
            if (Mathf.Approximately(noteBeat, baseBeat))
            {
                sameTimeNotes.Add(objNote);
            }
        }

        return sameTimeNotes;
    }

    /// <summary>
    /// 判定可能時間を過ぎたノーツをMiss処理する.
    /// </summary>
    private void JudgeMissNotes()
    {
        //現在のBGM時間を現在の拍に変換.
        float currentTime = SoundManager.Inst.GetTimeBGM();
        float currentBeat = soundSetting.GetBeat(currentTime);

        //削除対象のノーツ.
        List<GameObject> missNotes = new();

        //全ノーツを確認.
        foreach (GameObject objNote in noteList)
        {
            //既に削除されたノーツは無視.
            if (objNote == null)
            {
                continue;
            }

            Note note = objNote.GetComponent<Note>();

            //ノーツの判定時間を拍に変換.
            float noteBeat = soundSetting.GetBeat(note.GetNoteTime());

            //判定可能な時間を過ぎているか確認.
            if (currentBeat > noteBeat + laneSetting.badBeat)
            {
                missNotes.Add(objNote);
            }
        }

        //Missしたノーツを処理.
        foreach (GameObject objNote in missNotes)
        {
            //リストから削除.
            noteList.Remove(objNote);

            //ノーツを消す.
            if (objNote != null)
            {
                objNote.GetComponent<Note>().Destroy();
            }

            //Missとしてスコア・ゲージを処理.
            OnMiss();
        }
    }

    /// <summary>
    /// パーフェクト判定の処理.
    /// </summary>
    public void OnPerfect()
    {
        //他クラスの処理.
        gaugeMng.OnPerfect();
        scoreMng.OnPerfect();
    }

    /// <summary>
    /// パーフェクト判定の演出.
    /// </summary>
    public void OnPerfectEffect()
    {
        //PERFECT文字演出.
        Instantiate(prfbEffPerfect, inPrfbEff.transform);
        //SE再生.
        SoundManager.Inst.PlaySE("perfect");

        //プレイヤー残像演出.
        var obj = Instantiate(prfbEffPlayerPerfect, inPrfbEff.transform);
        //初期化.
        obj.GetComponent<EffectPlayerPefect>().Init(
            objPlayer.transform.position,
            objPlayer.GetComponent<Player>().GetNowImage()
        );
    }

    /// <summary>
    /// グッド判定の処理.
    /// </summary>
    public void OnGood()
    {
        //他クラスの処理.
        gaugeMng.OnGood();
        scoreMng.OnGood();
    }

    /// <summary>
    /// グッド判定の演出.
    /// </summary>
    public void OnGoodEffect()
    {
        //GOOD演出.
        Instantiate(prfbEffGood, inPrfbEff.transform);
        //SE再生.
        SoundManager.Inst.PlaySE("good");
    }

    /// <summary>
    /// バッド判定の処理.
    /// </summary>
    public void OnBad()
    {
        //他クラスの処理.
        gaugeMng.OnBad();
        scoreMng.OnBad();
    }

    /// <summary>
    /// バッド判定の演出.
    /// </summary>
    public void OnBadEffect()
    {
        //BAD演出.
        Instantiate(prfbEffBad, inPrfbEff.transform);
        //SE再生.
        SoundManager.Inst.PlaySE("bad");
    }

    /// <summary>
    /// ノーツを叩かなかった場合のMiss処理.
    /// </summary>
    private void OnMiss()
    {
        //スルーした時もBAD判定.
        OnBad();
        OnBadEffect();
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
