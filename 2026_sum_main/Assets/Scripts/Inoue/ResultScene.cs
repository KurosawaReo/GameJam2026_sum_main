using JetBrains.Annotations;
using System.Collections;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// リザルトシーン管理クラス.
/// </summary>
public class ResultManager : MonoBehaviour
{

    int score;

    int perfect;
    int good;
    int bad;

    int firstScore;
    int secondScore;
    int thirdScore;

    PlayerResultScore playerResult;

    [Header("#は入れるべきTextやImageの名前")]
    [Header("#PlayerScore")]
    [SerializeField]private TextMeshProUGUI playerResultText;
    [Header("#RankingText")]
    [SerializeField]private TextMeshProUGUI RankingText;
    [Header("#RankingFirst|RankingSecond|RankingThird")]
    [SerializeField]private TextMeshProUGUI[] TopRankingText;
    [Header("#ResultCountText")]
    [SerializeField] private TextMeshProUGUI ResultCount;
    [Header("#titleText(titleボタンのText)")]
    [SerializeField] private TextMeshProUGUI titleText;
    [Header("#playText(playボタンのText)")]
    [SerializeField] private TextMeshProUGUI playText;
    [Header("#title(titleボタンのImage)")]
    [SerializeField] private Image titleButton;
    [Header("#playButton(playボタンのImage)")]
    [SerializeField] private Image playButton;

    void Start()
    {
        playerResult = GameObject.FindAnyObjectByType<PlayerResultScore>();
        playerResultText.enabled = false;

        RankingText.enabled = false;

        ResultCount.enabled = false;

        TopRankingText[0].enabled = false;
        TopRankingText[1].enabled = false;
        TopRankingText[2].enabled = false;

        titleButton.enabled = false;
        playButton.enabled = false;
        titleText.enabled = false;
        playText.enabled = false;

        if (ScoreManager.instance)
        {
            ScoreManager.instance.GetRanking(out score,out perfect,out good,out bad);
        }

        firstScore = PlayerPrefs.GetInt("First");
        secondScore = PlayerPrefs.GetInt("Second");
        thirdScore = PlayerPrefs.GetInt("Third");
        StartCoroutine(StartResult());

    }

    public void PushBackTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
    public void PushReplay()
    {
        SceneManager.LoadScene("GameScene");
    }

    IEnumerator StartResult()
    {
        yield return new WaitForSeconds(0.5f);
        playerResultText.enabled = true;
        playerResultText.text = "あなたのヤリラスコア";
        yield return new WaitForSeconds(1.15f);
        playerResultText.text = "あなたのヤリラスコア" + score;
        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => playerResult.PlayerScoreMove() == true);
        RankingText.enabled = true;
        ResultCount.enabled = true;

        titleButton.enabled = true;
        playButton.enabled = true;
        titleText.enabled = true;
        playText.enabled = true;

        ResultCount.text = $"<color=yellow>Perfect : {perfect}</color><color=green>  Good : {good}</color><color=red>  Bad : {bad}</color>";

        TopRankingText[0].enabled = true;
        TopRankingText[1].enabled = true;
        TopRankingText[2].enabled = true;
        TopRankingText[0].text = "1位 : " + firstScore;
        TopRankingText[1].text = "2位 : " + secondScore;
        TopRankingText[2].text = "3位 : " + thirdScore;
        
    }

}
