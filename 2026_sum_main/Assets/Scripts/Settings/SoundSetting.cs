using UnityEngine;

/// <summary>
/// サウンド関係の設定.
/// </summary>
[CreateAssetMenu(fileName = "New File", menuName = "Game/Sound Setting")]
public class SoundSetting : ScriptableObject
{
    [Header(
        "- サウンド設定(GameScene) -\n\n" +
        "Bgm Start Delay : BGMを再生するまでの時間(秒)\n" +
        "Bgm End Delay : BGMを終了した後、次の処理を行うまでの時間(秒)\n" +
        "Beat Start Time : 0拍目になるBGM上の時間(秒)\n"
    )]
    public string bgmName;                  //再生するBGM名.
    public float bpm = 120.0f;              //BPM(曲のテンポ).
    [Space]
    public float bgmStartDelay = 1.0f;      //BGMを再生するまでの時間.
    public float bgmEndDelay = 1.0f;        //BGMを終了した後、次の処理を行うまでの時間.
    public float beatStartTime = 0.0f;      //0拍目になるBGM上の時間(秒).

    /// <summary>
    /// 1拍の秒数を取得.
    /// </summary>
    public float GetBeatSec()
    {
        return 60.0f / bpm;
    }

    /// <summary>
    /// 拍数を秒数に変換.
    /// </summary>
    public float GetTime(float beat)
    {
        // 0拍目の位置を基準に秒数へ変換.
        return beatStartTime + beat * GetBeatSec();
    }

    /// <summary>
    /// 秒数を拍数に変換.
    /// </summary>
    public float GetBeat(float time)
    {
        // 0拍目の位置を基準に拍数へ変換.
        return (time - beatStartTime) / GetBeatSec();
    }

    /// <summary>
    /// タイミング補正済みのゲーム時間を取得.
    /// </summary>
    public float GetGameTime()
    {
        // タイトルで設定した補正値を取得.
        float timingOffset = PlayerPrefs.GetFloat("TimingOffset", 0.0f);

        // 補正を反映したゲーム時間を返す.
        return SoundManager.Inst.GetTimeBGM() - timingOffset;
    }
}