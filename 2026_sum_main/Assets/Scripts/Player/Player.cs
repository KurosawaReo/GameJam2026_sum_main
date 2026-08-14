using UnityEngine;

/// <summary>
/// プレイヤークラス.
/// </summary>
public class Player : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] NoteManager noteManager;

    [Header("- setting -")]
    [SerializeField] PlayerSetting setting;

    float elapsed;    //経過時間.
    int   imageIndex; //現在の画像番号.

    SpriteRenderer spriteRenderer;

    /// <summary>
    /// 指定秒数後の画像を取得.
    /// </summary>
    public Sprite GetAfterImage(float _time)
    {
        // 指定時間から画像番号を計算.
        int index = Mathf.FloorToInt(_time / setting.changeInterval);

        // 画像枚数を超えたらループ.
        index %= setting.imgPlayer.Length;

        return setting.imgPlayer[index];
    }

    void Start()
    {
        // SpriteRendererを取得.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 最初の画像を設定.
        spriteRenderer.sprite = setting.imgPlayer[0];
    }

    void Update()
    {
        // 経過時間を加算.
        elapsed += Time.deltaTime;

        // 指定時間経過したら画像を切り替える.
        if (elapsed >= setting.changeInterval)
        {
            elapsed -= setting.changeInterval;

            // 次の画像へ.
            imageIndex++;

            // 最後まで行ったら最初に戻す.
            if (imageIndex >= setting.imgPlayer.Length)
            {
                imageIndex = 0;
            }

            // 画像を変更.
            spriteRenderer.sprite = setting.imgPlayer[imageIndex];
        }

        // 左クリックした瞬間.
        if (Input.GetMouseButtonDown(0))
        {
            // 最寄りのノーツ判定を行う.
            noteManager.JudgeNearestNote(transform.position);
        }
    }
}
