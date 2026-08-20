using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Common;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// タイトルシーン管理クラス.
/// </summary>
public class TitleManager : MonoBehaviour
{
    [Header("- effect -")]
    [SerializeField] GameObject objEffFadeIn;   //フェードイン演出.

    [Header("- panel -")]
    [SerializeField] GameObject panelSelect;    //譜面選択パネル.

    [Header("- object -")]
    [SerializeField] GameObject imgBack;        //背景画像.
    [SerializeField] GameObject objStartButton;

    [Header("- scene -")]
    [SerializeField] string     nextSceneName;

    [Header("- UI -")]
    [SerializeField] Slider          timingSlider;
    [SerializeField] TextMeshProUGUI textValue;

    bool isTransitioning = false; //シーン遷移中か.

    void Start()
    {
        // 保存されている補正値を取得.
        float value = PlayerPrefs.GetFloat("TimingOffset", 0.0f);
        // Sliderに現在の設定値を反映.
        timingSlider.value = value;
        // 表示を更新.
        UpdateValue(value);
        // Slider変更時の処理を登録.
        timingSlider.onValueChanged.AddListener(UpdateValue);
    }

    void Update()
    {
        // 開発者コマンド.
        CheckDeveloperCommand();
    }

    /// <summary>
    /// Sliderの値が変更された時.
    /// </summary>
    void UpdateValue(float value)
    {
        // 補正値を保存.
        PlayerPrefs.SetFloat("TimingOffset", value);
        PlayerPrefs.Save();

        // 現在の補正値を表示.
        textValue.text = $"{value:+0.00;-0.00;0.00} 秒";
    }

    /// <summary>
    /// 画面をタッチしたら譜面選択を表示.
    /// </summary>
    public void PushScreen()
    {
        //既に遷移中なら無視.
        if (isTransitioning)
        {
            return;
        }

        //SE再生.
        SoundManager.Inst.PlaySE("push_button");

        //譜面選択UIを表示.
        panelSelect.SetActive(true);
        //スタートボタン非表示.
        objStartButton.SetActive(false);
    }

    /// <summary>
    /// 通常譜面を選択.
    /// </summary>
    public void PushChartNormal()
    {
        // 通常譜面を選択.
        AllSceneData.Inst.ChartType = NoteChartType.Normal;

        // ゲーム開始.
        StartGame();
    }

    /// <summary>
    /// Extra譜面を選択.
    /// </summary>
    public void PushChartExtra()
    {
        // Extra譜面を選択.
        AllSceneData.Inst.ChartType = NoteChartType.Extra;

        // ゲーム開始.
        StartGame();
    }

    /// <summary>
    /// 譜面選択後にゲームを開始.
    /// </summary>
    private void StartGame()
    {
        //二重実行防止.
        if (isTransitioning)
        {
            return;
        }

        isTransitioning = true;

        //譜面選択UIを非表示.
        panelSelect.SetActive(false);

        //SE再生.
        SoundManager.Inst.PlaySE("title_button");
        //ズームする.
        imgBack.GetComponent<Animator>().SetTrigger("Start");

        //フェードイン開始.
        StartCoroutine(TransitionScene());
    }

    /// <summary>
    /// フェードイン演出後にシーンを移動.
    /// </summary>
    private IEnumerator TransitionScene()
    {
        // フェードイン演出を有効化.
        objEffFadeIn.SetActive(true);

        // Animatorを取得.
        Animator animator = objEffFadeIn.GetComponent<Animator>();

        // Animatorがある場合は終了まで待つ.
        if (animator != null)
        {
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
            );
        }

        // フェード終了後にゲームシーンへ移動.
        SceneManager.LoadScene(nextSceneName);
    }


    /// <summary>
    /// 開発者コマンドを確認.
    /// </summary>
    void CheckDeveloperCommand()
    {
        // ESC + Dを同時に押したらランキングをリセット.
        if (Input.GetKey(KeyCode.Escape) && Input.GetKey(KeyCode.D))
        {
            PlayerPrefs.DeleteKey("Ranking_Normal_1");
            PlayerPrefs.DeleteKey("Ranking_Normal_2");
            PlayerPrefs.DeleteKey("Ranking_Normal_3");

            PlayerPrefs.DeleteKey("Ranking_Extra_1");
            PlayerPrefs.DeleteKey("Ranking_Extra_2");
            PlayerPrefs.DeleteKey("Ranking_Extra_3");

            PlayerPrefs.Save();

            Debug.Log("ランキングをリセットしました.");
        }
    }
}