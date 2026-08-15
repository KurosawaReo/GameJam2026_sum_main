using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルシーン管理クラス.
/// </summary>
public class TitleManager : MonoBehaviour
{
    [Header("- scene -")]
    [SerializeField] string nextSceneName;

    /// <summary>
    /// 画面をタッチしたら.
    /// </summary>
    public void PushScreen()
    {
        //シーン移動.
        SceneManager.LoadScene(nextSceneName);
    }
}
