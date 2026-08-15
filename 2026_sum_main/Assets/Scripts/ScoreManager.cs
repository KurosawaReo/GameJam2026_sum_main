using UnityEngine;
using Common;

public class ScoreManager : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] GaugeManager gaugeMng;

    int upScore; // スコア上昇量.

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
    /// 結果を送る.
    /// </summary>
    public void SendResult(Result result)
    {
        // フィーバー中ならスコア倍率を上げる.
        if (gaugeMng.IsFever())
        {
            upScore = 1000;
        }
        else
        {
            upScore = 100;
        }

        // Resultの結果でデータを更新.
        switch (result)
        {
            case Result.Perfect:
                AllSceneData.instance.YariraScore += upScore;
                AllSceneData.instance.CountPerfect++;
                break;

            case Result.Good:
                AllSceneData.instance.YariraScore += upScore / 2;
                AllSceneData.instance.CountGood++;
                break;

            case Result.Bad:
                AllSceneData.instance.CountBad++;
                break;
        }
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