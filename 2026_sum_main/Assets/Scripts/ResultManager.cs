using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Common;

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

    [Header("- object -")]
    [SerializeField] GameObject objectsResult;

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
    [SerializeField] private TextMeshProUGUI textNoteChartName; //譜面名.

    void Start()
    {
        //最初は無効.
        objectsResult.SetActive(false);

        //AllSceneDataからゲーム結果を取得.
        if (AllSceneData.Inst != null)
        {
            score = AllSceneData.Inst.YariraScore;
            countPerfect = AllSceneData.Inst.CountPerfect;
            countGood = AllSceneData.Inst.CountGood;
            countBad = AllSceneData.Inst.CountBad;
        }

        //現在の譜面のランキングを取得.
        firstScore  = PlayerPrefs.GetInt($"Ranking_{AllSceneData.Inst.ChartType}_1", 0);
        secondScore = PlayerPrefs.GetInt($"Ranking_{AllSceneData.Inst.ChartType}_2", 0);
        thirdScore  = PlayerPrefs.GetInt($"Ranking_{AllSceneData.Inst.ChartType}_3", 0);

        //譜面名変更.
        switch (AllSceneData.Inst.ChartType) 
        {
            case NoteChartType.Normal:
                textNoteChartName.text = "ノーマル";
                break;
            case NoteChartType.Extra:
                textNoteChartName.text = "エクストラ";
                break;

            default: Debug.Log("不正な値です"); break;
        }

        StartCoroutine(StartResult());
    }

    public void PushBackTitle()
    {
        //SE再生.
        SoundManager.Inst.PlaySE("push_button");

        SceneManager.LoadScene("TitleScene");
    }
    public void PushReplay()
    {
        //SE再生.
        SoundManager.Inst.PlaySE("push_button");

        SceneManager.LoadScene("GameScene");
    }

    IEnumerator StartResult()
    {
        playerResultText.text = "あなたのヤリラスコア";

        yield return new WaitForSeconds(0.5f);
        playerResultText.enabled = true;

        yield return new WaitForSeconds(1.15f);

        //SE再生.
        SoundManager.Inst.PlaySE("result");

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
        objectsResult.SetActive(true);
    }
}
