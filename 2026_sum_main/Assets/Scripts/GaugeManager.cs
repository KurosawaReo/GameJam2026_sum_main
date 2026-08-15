using UnityEngine;
using UnityEngine.UI;

public class GaugeManager : MonoBehaviour
{
    [Header("- Gauge Setting -")]
    [SerializeField] float maxGauge = 20f;          // ゲージ最大値.
    [SerializeField] float perfectGauge = 1f;       // PERFECT時の変動量.
    [SerializeField] float goodGauge = 0f;          // GOOD時の変動量.
    [SerializeField] float badGauge = -1f;          // BAD時の変動量.
    [SerializeField] float feverDuration = 5f;      // フィーバー効果時間(秒).
    [SerializeField] float gaugeUpSpeed = 2f;       // ゲージ増加の表示速度.

    [Header("- Gauge -")]
    [SerializeField] Image imageGauge;              // ゲージ画像.

    [Header("- effect -")]
    [SerializeField] GameObject prfbEffFever;       // エフェクト用prefab.
    [SerializeField] GameObject inPrfbEffFever;     // prefabを入れる所.

    float targetGauge = 0f; // ゲージの目標値.
    bool isFever = false;

    /// <summary>
    /// フィーバー中かどうか.
    /// </summary>
    public bool IsFever()
    {
        return isFever;
    }

    void Start()
    {
        // ゲージを初期化.
        targetGauge = 0f;
        imageGauge.fillAmount = 0f;
    }

    void Update()
    {
        // 通常時は表示ゲージを目標値へ滑らかに近づける.
        if (!isFever)
        {
            imageGauge.fillAmount = Mathf.MoveTowards(
                imageGauge.fillAmount,
                targetGauge,
                gaugeUpSpeed * Time.deltaTime
            );
        }

        // ゲージが最大まで溜まったらフィーバー開始.
        if (!isFever && targetGauge >= 1f)
        {
            imageGauge.fillAmount = 1f;
            FeverStart();
        }

        if (isFever)
        {
            // フィーバー中は滑らかにせず、時間経過で直接減らす.
            float downSpeed = 1f / feverDuration;
            imageGauge.fillAmount -= downSpeed * Time.deltaTime;

            // ゲージが0になったらフィーバー終了.
            if (imageGauge.fillAmount <= 0f)
            {
                FeverEnd();
            }
        }
    }

    /// <summary>
    /// フィーバー開始.
    /// </summary>
    void FeverStart()
    {
        isFever = true;

        // 目標値を最大にする.
        targetGauge = 1f;

        // 演出召喚.
        Instantiate(prfbEffFever, inPrfbEffFever.transform);
    }

    /// <summary>
    /// フィーバー終了.
    /// </summary>
    void FeverEnd()
    {
        imageGauge.fillAmount = 0f;
        targetGauge = 0f;
        isFever = false;
    }

    /// <summary>
    /// PERFECT判定時.
    /// </summary>
    public void OnPerfect()
    {
        if (!isFever)
        {
            // 設定した値だけゲージを変動させる.
            ChangeGauge(perfectGauge);
        }
    }

    /// <summary>
    /// GOOD判定時.
    /// </summary>
    public void OnGood()
    {
        if (!isFever)
        {
            // 設定した値だけゲージを変動させる.
            ChangeGauge(goodGauge);
        }
    }

    /// <summary>
    /// BAD判定時.
    /// </summary>
    public void OnBad()
    {
        if (!isFever)
        {
            // 設定した値だけゲージを変動させる.
            ChangeGauge(badGauge);
        }
    }

    /// <summary>
    /// ゲージを指定量変動させる.
    /// </summary>
    private void ChangeGauge(float amount)
    {
        // 最大値を基準にしてゲージ量へ変換.
        float change = amount / maxGauge;

        // 目標値を変更.
        targetGauge += change;
        targetGauge = Mathf.Clamp01(targetGauge);

        // 表示ゲージはUpdateで目標値へ滑らかに近づける.
    }
}