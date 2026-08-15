using UnityEngine;

public class TextResultScore : MonoBehaviour
{
    [Header("- value -")]
    [SerializeField] Vector2 startPos; //スコアだけが表示される時の位置.

    RectTransform rectTransform;

    Vector3 initPos; //エディタ上で配置されていた初期位置.

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        initPos = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = startPos;
    }

    void Update()
    {
        
    }

    public bool PlayerScoreMove()
    {
        rectTransform.anchoredPosition = initPos;
        return true;
    }
}
