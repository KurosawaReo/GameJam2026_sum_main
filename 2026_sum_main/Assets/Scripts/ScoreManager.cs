using UnityEngine;
using Common;

public class ScoreManager : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] GaugeManager gaugeMng;

    [Header("- setting -")]
    [SerializeField] ScoreSetting scoreSetting;

    int firstScore = 0;
    int secondScore = 0;
    int thirdScore = 0;

    void Start()
    {
        //スコアをリセット.
        ResetScore();
        //保存されているランキングを取得.
        RegisterTopScore();
    }

    /// <summary>
    /// パーフェクト判定の処理.
    /// </summary>
    public void OnPerfect()
    {
        //加算量.
        int add = scoreSetting.perfectScore * (gaugeMng.IsFever() ? scoreSetting.feverRate : 1);

        AllSceneData.instance.YariraScore += add;
        AllSceneData.instance.CountPerfect++;
    }

    /// <summary>
    /// グッド判定の処理.
    /// </summary>
    public void OnGood()
    {
        //加算量.
        int add = scoreSetting.goodScore * (gaugeMng.IsFever() ? scoreSetting.feverRate : 1);

        AllSceneData.instance.YariraScore += add;
        AllSceneData.instance.CountGood++;
    }

    /// <summary>
    /// バッド判定の処理.
    /// </summary>
    public void OnBad()
    {
        //加算量.
        int add = scoreSetting.badScore * (gaugeMng.IsFever() ? scoreSetting.feverRate : 1);

        AllSceneData.instance.YariraScore += add;
        AllSceneData.instance.CountBad++;
    }

    /// <summary>
    /// スコアをリセット.
    /// </summary>
    public void ResetScore()
    {
        AllSceneData.instance.ResetData();
    }

    /// <summary>
    /// 現在のスコアをランキングに登録.
    /// </summary>
    public void RegisterRanking()
    {
        // 現在保存されているランキングを取得.
        RegisterTopScore();

        // 今回のスコアを取得.
        int score = AllSceneData.instance.YariraScore;

        // 1位に入る場合.
        if (score > firstScore)
        {
            thirdScore = secondScore;
            secondScore = firstScore;
            firstScore = score;
        }
        // 2位に入る場合.
        else if (score > secondScore)
        {
            thirdScore = secondScore;
            secondScore = score;
        }
        // 3位に入る場合.
        else if (score > thirdScore)
        {
            thirdScore = score;
        }

        // ランキングを保存.
        PlayerPrefs.SetInt("First", firstScore);
        PlayerPrefs.SetInt("Second", secondScore);
        PlayerPrefs.SetInt("Third", thirdScore);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 保存されているランキングを取得.
    /// </summary>
    private void RegisterTopScore()
    {
        // 保存されていない場合は0.
        firstScore = PlayerPrefs.GetInt("First", 0);
        secondScore = PlayerPrefs.GetInt("Second", 0);
        thirdScore = PlayerPrefs.GetInt("Third", 0);
    }
}