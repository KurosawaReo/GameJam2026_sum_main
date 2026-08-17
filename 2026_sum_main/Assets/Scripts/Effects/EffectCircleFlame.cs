using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField][Range(0f, 1f)] float startAlpha = 0.1f; // 開始時の透明度.
    [SerializeField][Range(0f, 1f)] float endAlpha = 1f;     // 終了時の透明度.

    [Header("- visual timing -")]
    [SerializeField] float timeOffset = 0f; // 視覚的な時間補正[秒].

    float targetBeat; // Scaleが終了値になる拍数.
    bool isInitialized = false;

    Image image;

    /// <summary>
    /// 初期化処理.
    /// </summary>
    public void Init(float beat)
    {
        // Scaleが終了値になる拍数を設定.
        targetBeat = beat;

        // Imageを取得.
        image = GetComponent<Image>();

        // 最初のScaleと透明度を設定.
        transform.localScale = Vector3.one * startScale;
        SetAlpha(startAlpha);

        isInitialized = true;
    }

    void Update()
    {
        // 初期化前なら処理しない.
        if (!isInitialized)
        {
            return;
        }

        // 現在のBGM時間を取得して視覚補正を適用.
        float currentTime = SoundManager.Inst.GetTimeBGM() + timeOffset;
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

        // Scaleを開始値から終了値まで変化させる.
        float scale = Mathf.Lerp(startScale, endScale, progress);
        transform.localScale = Vector3.one * scale;

        // Scaleに合わせて透明度も変化させる.
        float alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
        SetAlpha(alpha);
    }

    /// <summary>
    /// Imageの透明度を設定.
    /// </summary>
    void SetAlpha(float alpha)
    {
        // 現在の色を取得してAlphaだけ変更.
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}