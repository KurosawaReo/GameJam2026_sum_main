using Common;
using UnityEngine;
using static Unity.VisualScripting.Member;

/// <summary>
/// プレイヤークラス.
/// </summary>
public class Player : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] NoteManager   noteManager;

    [Header("- setting -")]
    [SerializeField] PlayerSetting playerSetting;
    [SerializeField] SoundSetting  soundSetting;

    SpriteRenderer spriteRenderer;

    float elapsed; //開始までの経過時間.

    /// <summary>
    /// 指定秒数後の画像を取得.
    /// </summary>
    public Sprite GetAfterImage(BodyParts parts, float time)
    {
        //画像1枚あたりの拍数から画像番号を計算.
        int index = GetImageIndex(time);
        //画像枚数を超えたらループ.
        index %= playerSetting.image.Length;

        //パーツ別の画像を返す.
        switch (parts)
        {
            case BodyParts.Main:
                return playerSetting.image[index].main;
            case BodyParts.Head:
                return playerSetting.image[index].head;
            case BodyParts.ArmL:
                return playerSetting.image[index].armL;
            case BodyParts.ArmR:
                return playerSetting.image[index].armR;
            case BodyParts.LegL:
                return playerSetting.image[index].legL;
            case BodyParts.LegR:
                return playerSetting.image[index].legR;

            default: Debug.Log("不正な値です"); break;
        }
        return null; //エラー.
    }

    /// <summary>
    /// BGM再生時間から現在の画像番号を取得.
    /// </summary>
    private int GetImageIndex(float time)
    {
        // 現在何拍目かを計算.
        float beat = time / soundSetting.GetBeatTime();

        // 画像番号を計算.
        int index = Mathf.FloorToInt(beat / playerSetting.changeBeat);

        // 画像枚数を超えたらループ.
        return index % playerSetting.image.Length;
    }

    void Start()
    {
        // SpriteRendererを取得.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 最初の画像を設定.
        spriteRenderer.sprite = playerSetting.image[0].main;
    }

    void Update()
    {
        // 開始待ち時間を計測.
        if (elapsed < playerSetting.startDelay)
        {
            elapsed += Time.deltaTime;
            return;
        }

        // BGMの再生時間から現在の画像番号を計算.
        float time  = SoundManager.Inst.GetTimeBGM();
        int   index = GetImageIndex(time);

        // 画像枚数を超えたらループ.
        index %= playerSetting.image.Length;

        // 現在の画像を設定.
        spriteRenderer.sprite = playerSetting.image[index].main;

        // 左クリックした瞬間.
        if (Input.GetMouseButtonDown(0))
        {
            // 最寄りのノーツ判定を行う.
            noteManager.JudgeNearestNote(transform.position);
        }
    }
}
