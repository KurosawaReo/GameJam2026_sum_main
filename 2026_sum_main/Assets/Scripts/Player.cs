using UnityEngine;

/// <summary>
/// プレイヤークラス.
/// </summary>
public class Player : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] NoteManager noteManager;

    [Header("- image -")]
    [SerializeField] Sprite[] imgPlayer;
    [SerializeField] float changeInterval = 1.0f; //画像切り替え間隔.

    float elapsed;    //経過時間.
    int   imageIndex; //現在の画像番号.

    SpriteRenderer spriteRenderer;

    /// <summary>
    /// 画像切り替え間隔の取得.
    /// </summary>
    public float GetChangeInterval()
    {
        return changeInterval;
    }

    void Start()
    {
        // SpriteRendererを取得.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 最初の画像を設定.
        spriteRenderer.sprite = imgPlayer[0];
    }

    void Update()
    {
        // 経過時間を加算.
        elapsed += Time.deltaTime;

        // 指定時間経過したら画像を切り替える.
        if (elapsed >= changeInterval)
        {
            elapsed -= changeInterval;

            // 次の画像へ.
            imageIndex++;

            // 最後まで行ったら最初に戻す.
            if (imageIndex >= imgPlayer.Length)
            {
                imageIndex = 0;
            }

            // 画像を変更.
            spriteRenderer.sprite = imgPlayer[imageIndex];
        }

        // 左クリックした瞬間.
        if (Input.GetMouseButtonDown(0))
        {
            // 最寄りのノーツ判定を行う.
            noteManager.JudgeNearestNote(transform.position);
        }
    }
}
