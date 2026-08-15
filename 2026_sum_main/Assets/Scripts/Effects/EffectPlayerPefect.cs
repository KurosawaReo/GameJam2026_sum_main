using UnityEngine;

public class EffectPlayerPefect : MonoBehaviour
{
    [Header("- object -")]
    [SerializeField] GameObject image;

    SpriteRenderer spriteRenderer;

    /// <summary>
    /// オブジェクト生成時に初期化.
    /// </summary>
    void Awake()
    {
        // 子オブジェクトのSpriteRendererを取得.
        spriteRenderer = image.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 初期化処理.
    /// </summary>
    public void Init(Vector3 pos, Sprite sprite)
    {
        // 表示位置を設定.
        transform.position = pos;

        // SpriteRendererが取得できていなければ終了.
        if (spriteRenderer == null)
        {
            return;
        }

        // 表示する画像を設定.
        spriteRenderer.sprite = sprite;
    }
}
