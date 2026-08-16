using UnityEngine;

/// <summary>
/// タイミングのヘルプ演出.
/// 指定した拍数で指定したScaleになる.
/// </summary>
public class EffectCircleFlame : MonoBehaviour
{
    [Header("- setting -")]
    [SerializeField] SoundSetting soundSetting;
    [SerializeField] float startScale = 2f; // 開始時のScale.
    [SerializeField] float endScale = 1f;   // 終了時のScale.
    [SerializeField] float startBeat = 4f;  // 目標拍の何拍前から開始するか.

    float targetBeat; // Scaleが終了値になる拍数.
    bool isInitialized = false;

    /// <summary>
    /// 初期化処理.
    /// </summary>
    public void Init(float beat)
    {
        // Scaleが終了値になる拍数を設定.
        targetBeat = beat;

        // 最初のScaleを設定.
        transform.localScale = Vector3.one * startScale;

        isInitialized = true;
    }

    void Update()
    {
        // 初期化前なら処理しない.
        if (!isInitialized)
        {
            return;
        }

        // 現在の拍数を取得.
        float currentTime = SoundManager.Inst.GetTimeBGM();
        float currentBeat = soundSetting.GetBeat(currentTime);

        // 目標拍を過ぎたら削除.
        if (currentBeat >= targetBeat)
        {
            Destroy(gameObject);
            return;
        }

        // 開始拍から目標拍までの進行度を計算.
        float startBeatTime = targetBeat - startBeat;
        float progress = Mathf.InverseLerp(startBeatTime, targetBeat, currentBeat);

        // 開始Scaleから終了Scaleまで縮小・拡大.
        float scale = Mathf.Lerp(startScale, endScale, progress);

        transform.localScale = Vector3.one * scale;
    }
}