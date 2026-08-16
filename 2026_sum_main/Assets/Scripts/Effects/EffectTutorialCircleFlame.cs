using UnityEngine;

/// <summary>
/// タイミングのヘルプ演出.
/// 指定した拍数でScaleが1になる.
/// </summary>
public class EffectTutorialCircleFlame : MonoBehaviour
{
    [Header("- setting -")]
    [SerializeField] SoundSetting soundSetting;
    [SerializeField] float startScale = 0f; // 開始時のScale.
    [SerializeField] float scalePerBeat = 1f; // 1拍あたりのScale変化量.

    float targetBeat; // Scaleが1になる拍数.
    bool isInitialized = false;

    /// <summary>
    /// 初期化処理.
    /// </summary>
    public void Init(float beat)
    {
        // Scaleが1になる拍数を設定.
        targetBeat = beat;

        // 初期Scaleを設定.
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

        // 現在のBGM時間から拍数を取得.
        float currentTime = SoundManager.Inst.GetTimeBGM();
        float currentBeat = soundSetting.GetBeat(currentTime);

        // 目標拍までの残り拍数を取得.
        float remainBeat = targetBeat - currentBeat;

        // 目標拍に近づくほどScaleを大きくする.
        float scale = 1f - remainBeat * scalePerBeat;

        // 0～1の範囲に収める.
        scale = Mathf.Clamp01(scale);

        // Scaleを更新.
        transform.localScale = Vector3.one * scale;
    }
}