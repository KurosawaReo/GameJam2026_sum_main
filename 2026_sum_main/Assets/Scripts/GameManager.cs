using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ゲーム管理クラス.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("- Setting -")]
    [SerializeField] SoundSetting soundSetting;     //サウンド設定.

    [Header("- Scene -")]
    [SerializeField] string       titleSceneName;   //遷移先シーン名.
    [SerializeField] string       resultSceneName;  //遷移先シーン名.

    [Header("- Script -")]
    [SerializeField] GaugeManager gaugeMng;
    [SerializeField] ScoreManager scoreMng;

    [Header("- Effect -")]
    [SerializeField] GameObject   effRainbow;       //虹色演出.
    [SerializeField] GameObject   effFadeOut;       //開始時のフェードイン.

    [Header("- Debug -")]
    [SerializeField] Text         debugBeatCount;

    float elapsed;          //現在の経過時間.
    bool  isBgmStarted;     //BGMを再生したか.
    bool  isBgmFinished;    //BGM終了を検知したか.

    /// <summary>
    /// BGMが開始済みか.
    /// </summary>
    public bool IsBgmStarted => isBgmStarted;

    private void Awake()
    {
        // シーン読み込み完了イベントを登録.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // シーン読み込み完了イベントを解除.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// シーンの読み込みが完了した時.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // GameScene以外では処理しない.
        if (scene.name != gameObject.scene.name)
        {
            return;
        }

        // Fade演出を有効化.
        effFadeOut.GetComponent<Animator>().SetTrigger("FadeOut");
    }

    void Update()
    {
        //デバッグ表示.
        UpdateDebugText();

        //BGM開始前なら開始処理.
        if (!isBgmStarted)
        {
            StartBGM();
        }
        //BGM開始後なら終了判定.
        else
        {
            JudgeEndBGM();
        }
         
        //フィーバー演出.
        effRainbow.SetActive(gaugeMng.IsFever());
    }

    // デバッグ用：BGM時間と拍数を表示.
    void UpdateDebugText()
    {
        if (!debugBeatCount || !SoundManager.Inst)
        {
            return;
        }

        // 現在のBGM再生時間を取得.
        float currentTime = soundSetting.GetGameTime();

        // BGM時間から現在の拍数を計算.
        float currentBeat = soundSetting.GetBeat(currentTime);

        // 画面に表示.
        debugBeatCount.text =
            $"BGM Time : {currentTime:F3}\n" +
            $"Beat     : {currentBeat:F3}";
    }

    /// <summary>
    /// BGM開始処理.
    /// </summary>
    void StartBGM()
    {
        // 経過時間を加算.
        elapsed += Time.deltaTime;

        // 指定時間経過していなければ待機.
        if (elapsed < soundSetting.bgmStartDelay)
        {
            return;
        }

        // BGMを再生.
        SoundManager.Inst.PlayBGM(soundSetting.bgmName, false);

        // BGM再生済みにする.
        isBgmStarted = true;
        elapsed = 0.0f;
    }

    /// <summary>
    /// BGM終了判定.
    /// </summary>
    void JudgeEndBGM()
    {
        // まだBGM終了を検知していなければ.
        if (!isBgmFinished)
        {
            // BGMが終了したらタイマーを開始.
            if (SoundManager.Inst.IsBGMFinished())
            {
                isBgmFinished = true;
                elapsed = 0.0f;
            }

            return;
        }

        // BGM終了後の経過時間を加算.
        elapsed += Time.deltaTime;

        // 指定時間経過したらシーン遷移.
        if (elapsed >= soundSetting.bgmEndDelay)
        {
            GameEnd();
        }
    }

    /// <summary>
    /// タイトルへ戻る.
    /// </summary>
    public void PushBackTitle()
    {
        SoundManager.Inst.StopBGM(); //BGM停止.
        SceneManager.LoadScene(titleSceneName);
    }

    /// <summary>
    /// ゲーム終了処理.
    /// </summary>
    void GameEnd()
    {
        //現在のスコアをランキングに登録.
        scoreMng.RegisterRanking();
        //次のシーンへ.
        SceneManager.LoadScene(resultSceneName);
    }
}