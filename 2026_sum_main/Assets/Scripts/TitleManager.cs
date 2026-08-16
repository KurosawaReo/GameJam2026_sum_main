using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// タイトルシーン管理クラス.
/// </summary>
public class TitleManager : MonoBehaviour
{
    [Header("- effect -")]
    [SerializeField] GameObject objEffFadeIn; // フェードイン演出.

    [Header("- scene -")]
    [SerializeField] string nextSceneName;

    bool isTransitioning = false; // シーン遷移中か.

    void Update()
    {
        // 開発者コマンド.
        CheckDeveloperCommand();
    }

    /// <summary>
    /// 画面をタッチしたら.
    /// </summary>
    public void PushScreen()
    {
        // 既に遷移処理中なら無視.
        if (isTransitioning)
        {
            return;
        }

        // 遷移開始.
        StartCoroutine(TransitionScene());
    }

    /// <summary>
    /// フェードイン演出後にシーンを移動.
    /// </summary>
    private IEnumerator TransitionScene()
    {
        isTransitioning = true;

        // 有効にする.
        objEffFadeIn.SetActive(true);

        // Animatorを取得.
        Animator animator = objEffFadeIn.GetComponent<Animator>();

        // Animatorがある場合はアニメーション終了まで待つ.
        if (animator != null)
        {
            // 現在再生中のアニメーションが終了するまで待つ.
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
            );
        }

        // アニメーション終了後にシーン移動.
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
            PlayerPrefs.DeleteKey("First");
            PlayerPrefs.DeleteKey("Second");
            PlayerPrefs.DeleteKey("Third");
            PlayerPrefs.Save();

            Debug.Log("ランキングをリセットしました.");
        }
    }
}