using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GaugeManager : MonoBehaviour
{
    [Header("- value -")]
    [SerializeField] float maxGauge  = 20;
    [SerializeField] float upGauge   = 1;
    [SerializeField] float downGauge = 5;

    [Header("- gauge -")]
    [SerializeField] Image imageGauge;

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

    private void Update()
    {
        if (imageGauge.fillAmount >= 1)
        {
            isFever = true;
        }

        if (isFever)
        {
            imageGauge.fillAmount += (-downGauge / maxGauge) * Time.deltaTime; //0になるまでゲージを減らす.

            if (imageGauge.fillAmount <= 0)
            {
                imageGauge.fillAmount = 0;
                isFever = false;
            }
        }
    }

    //PERFECT判定だったら.
    public void OnPerfect()
    {
        if (!isFever)
        {
            imageGauge.fillAmount += upGauge / maxGauge; //ゲージを増加する.
        }
    }

    //GOOD判定だったら.
    public void OnGood()
    {
        //変動なし.
    }

    //BAD判定だったら.
    public void OnBad()
    {
        if (!isFever)
        {
            imageGauge.fillAmount -= upGauge / maxGauge; //ゲージを減らす.
        }
    }
}
