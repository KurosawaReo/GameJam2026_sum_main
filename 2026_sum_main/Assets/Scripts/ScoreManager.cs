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

        AllSceneData.Inst.YariraScore += add;
        AllSceneData.Inst.CountPerfect++;
    }

    /// <summary>
    /// グッド判定の処理.
    /// </summary>
    public void OnGood()
    {
        //加算量.
        int add = scoreSetting.goodScore * (gaugeMng.IsFever() ? scoreSetting.feverRate : 1);

        AllSceneData.Inst.YariraScore += add;
        AllSceneData.Inst.CountGood++;
    }

    /// <summary>
    /// バッド判定の処理.
    /// </summary>
    public void OnBad()
    {
        //加算量.
        int add = scoreSetting.badScore * (gaugeMng.IsFever() ? scoreSetting.feverRate : 1);

        AllSceneData.Inst.YariraScore += add;
        AllSceneData.Inst.CountBad++;
    }

    /// <summary>
    /// スコアをリセット.
    /// </summary>
    public void ResetScore()
    {
        AllSceneData.Inst.ResetData();
    }

    /// <summary>
    /// 現在のスコアをランキングに登録.
    /// </summary>
    public void RegisterRanking()
    {
        // 現在保存されているランキングを取得.
        RegisterTopScore();

        // 今回のスコアを取得.
        int score = AllSceneData.Inst.YariraScore;

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

        // 現在の譜面用のランキングとして保存.
        PlayerPrefs.SetInt(GetRankingKey(1), firstScore);
        PlayerPrefs.SetInt(GetRankingKey(2), secondScore);
        PlayerPrefs.SetInt(GetRankingKey(3), thirdScore);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 保存されているランキングを取得.
    /// </summary>
    private void RegisterTopScore()
    {
        //現在の譜面用のランキングを取得.
        firstScore = PlayerPrefs.GetInt(GetRankingKey(1), 0);
        secondScore = PlayerPrefs.GetInt(GetRankingKey(2), 0);
        thirdScore = PlayerPrefs.GetInt(GetRankingKey(3), 0);
    }

    /// <summary>
    /// ランキング保存用のキーを取得.
    /// </summary>
    private string GetRankingKey(int rank)
    {
        //譜面の種類をキーに含める.
        return $"Ranking_{AllSceneData.Inst.ChartType}_{rank}";
    }
}