using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    TextMeshProUGUI textScore;

    void Start()
    {
        textScore = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (ScoreManager.instance != null && textScore != null)
        {
            textScore.text = "ƒ„ƒŠƒ‰ƒXƒRƒA : " + ScoreManager.instance.YariraScore;
        }
    }
    
}
