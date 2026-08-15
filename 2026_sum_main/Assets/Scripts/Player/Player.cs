using Common;
using UnityEngine;
using static Unity.VisualScripting.Member;

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
    public Sprite GetAfterImage(BodyParts parts, float time)
    {
        //指定時間から画像番号を計算.
        int index = Mathf.FloorToInt(time / setting.changeInterval);

        //画像枚数を超えたらループ.
        index %= setting.image.Length;

#if false
        //【TODO】時間がなれけばプレイヤーの画像をそのまま返す.      
        return setting.imgPlayer[index];
#else
        //パーツ別の画像を返す.
        switch (parts)
        {
            case BodyParts.Head:
                return setting.image[index].head;
            case BodyParts.ArmL:
                return setting.image[index].armL;
            case BodyParts.ArmR:
                return setting.image[index].armR;
            case BodyParts.LegL:
                return setting.image[index].legL;
            case BodyParts.LegR:
                return setting.image[index].legR;

            default: Debug.Log("不正な値です"); break;
        }
#endif
        return null; //エラー.
    }

    void Start()
    {
        // SpriteRendererを取得.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 最初の画像を設定.
        spriteRenderer.sprite = setting.image[0].main;
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
            if (imageIndex >= setting.image.Length)
            {
                imageIndex = 0;
            }

            // 画像を変更.
            spriteRenderer.sprite = setting.image[imageIndex].main;
        }

        // 左クリックした瞬間.
        if (Input.GetMouseButtonDown(0))
        {
            // 最寄りのノーツ判定を行う.
            noteManager.JudgeNearestNote(transform.position);
        }
    }
}
