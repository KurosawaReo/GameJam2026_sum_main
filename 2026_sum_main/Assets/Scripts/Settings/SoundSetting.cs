using UnityEngine;

/// <summary>
/// サウンド関係の設定.
/// </summary>
[CreateAssetMenu(fileName = "New File", menuName = "Game/Sound Setting")]
public class SoundSetting : ScriptableObject
{
    [Header(
        "- サウンド設定(GameScene) -\n\n" +
        "Bgm Start Delay : BGMを再生するまでの時間\n" +
        "Bgm End Delay : BGMを終了した後、次の処理を行うまでの時間\n"
    )]
    public string bgmName;              //再生するBGM名.
    public float  bpm = 120.0f;         //BPM(曲のテンポ)
    [Space]
    public float  bgmStartDelay = 1.0f; //BGMを再生するまでの時間.
    public float  bgmEndDelay = 1.0f;   //BGMを終了した後、次の処理を行うまでの時間.

    /// <summary>
    /// 1拍の秒数を取得.
    /// </summary>
    public float GetBeatTime()
    {
        return 60.0f / bpm;
    }

    /// <summary>
    /// 拍数を秒数に変換.
    /// </summary>
    public float GetTime(float beat)
    {
        return beat * GetBeatTime();
    }
}