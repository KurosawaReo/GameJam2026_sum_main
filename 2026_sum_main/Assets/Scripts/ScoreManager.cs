using Unity.VisualScripting;
using UnityEngine;
using Common;
using System.Net.Sockets;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    //シングルトン用.
    public static ScoreManager instance;

    [Header("- script -")]
    [SerializeField] GaugeManager gaugeMng;

    public int YariraScore { get; set; }

    int upScore; //スコア上昇量.
    
    int firstScore = 0;
    int secondScore = 0;
    int thirdScore = 0;

    int countPerfect = 0;
    int countGood = 0;
    int countBad = 0;

    // スコア初期化処理
    public void ResetScore()
    {
        YariraScore = 0;
        countPerfect = 0;
        countGood = 0;
        countBad = 0;
    }

    void Awake()
    {
        //1度のみ実行.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this); //シーン遷移で消滅しないようにする.
            
            ResetScore(); //リセット処理.
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        RegisterTopScore();
    }

    void Update()
    {
    }

    /// <summary>
    /// 結果を送る.
    /// </summary>
    public void SendResult(Result result)
    {
        //スコア加算量.
        if (gaugeMng.IsFever())
        {
            upScore = 1000;
        }
        else
        {
            upScore = 100;
        }

        //Resultの結果でスコアを変動させる.
        switch (result)
        {
           case Result.Perfect:
                YariraScore += upScore;
                countPerfect++;
                break;
           case Result.Good:
                YariraScore += upScore / 2;
                countGood++;
                break;
           case Result.Bad:
                countBad++;
                break;
        }
    }

    /// <summary>
    /// ランキング登録.
    /// </summary>
    public void RegisterRanking()
    {
        RegisterTopScore();

        if (YariraScore > firstScore)
        {
            thirdScore = secondScore;
            secondScore = firstScore;
            firstScore = YariraScore;
        }
        else if (YariraScore > secondScore)
        {
            thirdScore = secondScore;
            secondScore = YariraScore;
        }
        else if (YariraScore > thirdScore)
        {
            thirdScore = YariraScore;
        }
        
        // 更新された値を保存
        PlayerPrefs.SetInt("First", firstScore);
        PlayerPrefs.SetInt("Second", secondScore);
        PlayerPrefs.SetInt("Third", thirdScore);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ハイスコア登録.
    /// </summary>
    void RegisterTopScore()
    {
        //順位TOP3のスコアを変数に代入する.
        if (PlayerPrefs.HasKey("First"))
        {
            firstScore = PlayerPrefs.GetInt("First");
        }
        if (PlayerPrefs.HasKey("Second"))
        {
            secondScore = PlayerPrefs.GetInt("Second");
        }
        if (PlayerPrefs.HasKey("Third"))
        {
            thirdScore = PlayerPrefs.GetInt("Third");
        }
    }

    /// <summary>
    /// ランキング取得.
    /// </summary>
    public void GetRanking(out int _score, out int _countPerfect, out int _countGood, out int _countBad)
    {
        _score = YariraScore;
        _countPerfect = countPerfect;
        _countGood = countGood;
        _countBad = countBad;
    }
}
