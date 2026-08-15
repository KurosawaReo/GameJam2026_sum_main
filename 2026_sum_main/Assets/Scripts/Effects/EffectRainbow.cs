using UnityEngine;
using UnityEngine.UI;

public class EffectRainbow : MonoBehaviour
{
    [Header("- value -")]
    [SerializeField] float rainbowSpeed = 0.5f; // 虹色が変化する速度.
    [SerializeField][Range(0f, 1f)] float saturation = 0.8f; // 彩度.
    [SerializeField][Range(0f, 1f)] float brightness = 1f; // 明るさ.
    [SerializeField][Range(0f, 1f)] float alpha = 1f; // 透明度.

    float hue = 0f;

    Image cmpImage; // コンポーネント.

    void Start()
    {
        cmpImage = GetComponent<Image>();
    }

    void Update()
    {
        // 色相を0～1の範囲で循環させる.
        hue += Time.deltaTime * rainbowSpeed;
        hue %= 1f;

        // HSVからRGBへ変換.
        Color rainbow = Color.HSVToRGB(hue, saturation, brightness);

        // 透明度を設定.
        rainbow.a = alpha;

        cmpImage.color = rainbow;
    }
}