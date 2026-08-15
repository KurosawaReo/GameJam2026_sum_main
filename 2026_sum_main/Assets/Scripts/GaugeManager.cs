using UnityEngine;
using UnityEngine.UI;

public class GaugeManager : MonoBehaviour
{
    [Header("- Gauge Setting -")]
    [SerializeField] float maxGauge = 20f;          //ゲージ最大値.
    [SerializeField] float perfectGauge = 1f;       //PERFECT時の増加量.
    [SerializeField] float badGauge = 1f;           //BAD時の減少量.
    [SerializeField] float feverDuration = 5f;      //フィーバー効果時間(秒).

    [Header("- effect -")]
    [SerializeField] GameObject prfbEffFever;       //エフェクト用prefab.
    [SerializeField] GameObject inPrfbEffFever;     //prefabを入れる所.

    [Header("- Gauge -")]
    [SerializeField] Image imageGauge;              //ゲージ画像.

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
        imageGauge.fillAmount = 0;
    }

    void Update()
    {
        //ゲージが最大まで溜まったらフィーバー開始.
        if (!isFever && imageGauge.fillAmount >= 1f)
        {
            FeverStart();
        }

        if (isFever)
        {
            //指定した効果時間でゲージが0になるように減少させる.
            float downSpeed = 1f / feverDuration;
            imageGauge.fillAmount -= downSpeed * Time.deltaTime;

            //ゲージが0になったらフィーバー終了.
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
        //演出召喚.
        Instantiate(prfbEffFever, inPrfbEffFever.transform);
    }

    /// <summary>
    /// フィーバー終了.
    /// </summary>
    void FeverEnd()
    {
        imageGauge.fillAmount = 0f;
        isFever = false;
    }

    /// <summary>
    /// PERFECT判定時.
    /// </summary>
    public void OnPerfect()
    {
        if (!isFever)
        {
            // 指定した増加量を最大値で正規化.
            imageGauge.fillAmount += perfectGauge / maxGauge;
        }
    }

    /// <summary>
    /// GOOD判定時.
    /// </summary>
    public void OnGood()
    {
        // 変動なし.
    }

    /// <summary>
    /// BAD判定時.
    /// </summary>
    public void OnBad()
    {
        if (!isFever)
        {
            // 指定した減少量を最大値で正規化.
            imageGauge.fillAmount -= badGauge / maxGauge;
        }
    }
}