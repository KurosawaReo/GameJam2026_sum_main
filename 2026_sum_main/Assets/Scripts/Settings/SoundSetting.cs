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
    public float  bgmStartDelay = 1.0f; //BGMを再生するまでの時間.
    public float  bgmEndDelay = 1.0f;   //BGMを終了した後、次の処理を行うまでの時間.
}