using UnityEngine;
using UnityEngine.SceneManagement;

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
