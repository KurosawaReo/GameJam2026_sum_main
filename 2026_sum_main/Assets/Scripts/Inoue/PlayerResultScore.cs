using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class PlayerResultScore : MonoBehaviour
{
    RectTransform rectTransform;
    public Vector2 targetPosition = new Vector2(0,-200);

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 100f);
    }

    void Update()
    {
        
    }

    public bool PlayerScoreMove()
    {
        rectTransform.anchoredPosition = targetPosition;
        return true;
    }
}
