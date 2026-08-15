using UnityEngine;
using TMPro;

public class TextYariraScore : MonoBehaviour
{
    TextMeshProUGUI textScore;

    void Start()
    {
        textScore = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (AllSceneData.instance != null && textScore != null)
        {
            // 5桁になるように0埋めして表示.
            textScore.text = "ヤリラスコア : " + AllSceneData.instance.YariraScore.ToString("D5");
        }
    }
}