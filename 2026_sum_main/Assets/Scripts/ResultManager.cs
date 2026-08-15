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

    int countPerfect;
    int countGood;
    int countBad;

    int firstScore;
    int secondScore;
    int thirdScore;

    /*
    [Header("#は入れるべきTextやImageの名前")]
    [Header("#PlayerScore")]
    [SerializeField]private TextMeshProUGUI playerResultText;
    [Header("#RankingText")]
    [SerializeField]private TextMeshProUGUI RankingText;
    [Header("#RankingFirst|RankingSecond|RankingThird")]
    [SerializeField]private TextMeshProUGUI[] TopRankingText;
    [Header("#titleText(titleボタンのText)")]
    [SerializeField] private TextMeshProUGUI titleText;
    [Header("#playText(playボタンのText)")]
    [SerializeField] private TextMeshProUGUI playText;
    [Header("#title(titleボタンのImage)")]
    [SerializeField] private Image titleButton;
    [Header("#playButton(playボタンのImage)")]
    [SerializeField] private Image playButton;
    */

    [Header("- object -")]
    [SerializeField] GameObject objectsRanking;
    [SerializeField] GameObject objectsResultCount;
    [SerializeField] GameObject buttonBackTitle;
    [SerializeField] GameObject buttonReplay;

    [Header("- script -")]
    [SerializeField] private TextResultScore scptTextResultScore;

    [Header("- text -")]
    [SerializeField] private TextMeshProUGUI playerResultText;
    [SerializeField] private TextMeshProUGUI textCountPerfect;
    [SerializeField] private TextMeshProUGUI textCountGood;
    [SerializeField] private TextMeshProUGUI textCountBad;
    [SerializeField] private TextMeshProUGUI topRankingText1;
    [SerializeField] private TextMeshProUGUI topRankingText2;
    [SerializeField] private TextMeshProUGUI topRankingText3;

    void Start()
    {
        //最初は無効.
        objectsRanking.SetActive(false);
        objectsResultCount.SetActive(false);
        buttonBackTitle.SetActive(false);
        buttonReplay.SetActive(false);

        //AllSceneDataからゲーム結果を取得.
        if (AllSceneData.instance != null)
        {
            score = AllSceneData.instance.YariraScore;
            countPerfect = AllSceneData.instance.CountPerfect;
            countGood = AllSceneData.instance.CountGood;
            countBad = AllSceneData.instance.CountBad;
        }

        // 保存されているランキングを取得.
        firstScore = PlayerPrefs.GetInt("First", 0);
        secondScore = PlayerPrefs.GetInt("Second", 0);
        thirdScore = PlayerPrefs.GetInt("Third", 0);

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
        playerResultText.text = "あなたのヤリラスコア\n" + score;
        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => scptTextResultScore.PlayerScoreMove() == true);

        textCountPerfect.text = countPerfect.ToString("D3");
        textCountGood.text    = countGood.ToString("D3");
        textCountBad.text     = countBad.ToString("D3");

        topRankingText1.text = "1位 : " + firstScore.ToString("D5");
        topRankingText2.text = "2位 : " + secondScore.ToString("D5");
        topRankingText3.text = "3位 : " + thirdScore.ToString("D5");   
        
        //有効にする.
        objectsRanking.SetActive(true);
        objectsResultCount.SetActive(true);
        buttonBackTitle.SetActive(true);
        buttonReplay.SetActive(true);
    }
}
