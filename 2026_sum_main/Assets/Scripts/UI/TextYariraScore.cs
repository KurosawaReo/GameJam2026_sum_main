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
        if (AllSceneData.Inst != null && textScore != null)
        {
            // 5桁になるように0埋めして表示.
            textScore.text = "ヤリラスコア : " + AllSceneData.Inst.YariraScore.ToString("D5");
        }
    }
}