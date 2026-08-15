using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム管理クラス.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("- setting -")]
    [SerializeField] SoundSetting soundSetting; //サウンド設定.

    [Header("- Scene -")]
    [SerializeField] string nextSceneName;      //遷移先シーン名.

    [Header("- Script -")]
    [SerializeField] GaugeManager gaugeMng;

    [Header("- Effect -")]
    [SerializeField] GameObject effectRainbow;  //虹色演出.

    float elapsed;          //現在の経過時間.
    bool  isBgmStarted;     //BGMを再生したか.
    bool  isBgmFinished;    //BGM終了を検知したか.

    private void Update()
    {
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
        effectRainbow.SetActive(gaugeMng.IsFever());
    }

    /// <summary>
    /// BGM開始処理.
    /// </summary>
    private void StartBGM()
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
    private void JudgeEndBGM()
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
            SceneManager.LoadScene(nextSceneName);
        }
    }
}