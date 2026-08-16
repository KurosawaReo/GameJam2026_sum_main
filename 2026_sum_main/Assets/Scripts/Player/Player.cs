using UnityEngine;
using Common;

/// <summary>
/// プレイヤークラス.
/// </summary>
public class Player : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] NoteManager   noteManager;
    [SerializeField] GaugeManager  gaugeMng;

    [Header("- setting -")]
    [SerializeField] LaneSetting   laneSetting;
    [SerializeField] PlayerSetting playerSetting;
    [SerializeField] SoundSetting  soundSetting;

    [Header("- fever effect -")]
    [SerializeField] float rainbowSpeed = 0.5f; // 虹色が変化する速度.
    [SerializeField][Range(0f, 1f)] float saturation = 0.8f; // 彩度.
    [SerializeField][Range(0f, 1f)] float brightness = 1f; // 明るさ.

    SpriteRenderer spriteRenderer;

    float elapsed;  //開始までの経過時間.
    float hue = 0f; //現在の色相.

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
    /// 現在の画像を取得.
    /// </summary>
    /// <returns></returns>
    public Sprite GetNowImage()
    {
        return spriteRenderer.sprite;
    }

    /// <summary>
    /// BGM再生時間から現在の画像番号を取得.
    /// </summary>
    private int GetImageIndex(float time)
    {
        // 現在何拍目かを計算.
        float beat = time / soundSetting.GetBeatSec();

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
        //開始待ち時間を計測.
        if (elapsed < playerSetting.startDelay)
        {
            elapsed += Time.deltaTime;
            return;
        }

        //BGMの再生時間から現在の画像番号を計算.
        float time  = SoundManager.Inst.GetTimeBGM();
        int   index = GetImageIndex(time);

        //画像枚数を超えたらループ.
        index %= playerSetting.image.Length;

        //現在の画像を設定.
        spriteRenderer.sprite = playerSetting.image[index].main;

        //フィーバー中のみ虹色にする.
        UpdateFeverColor();

        //左クリックした瞬間.
        if (Input.GetMouseButtonDown(0))
        {
            OnClickR();
        }
    }

    /// <summary>
    /// フィーバー中のプレイヤー色を更新.
    /// </summary>
    void UpdateFeverColor()
    {
        // フィーバー中でなければ白に戻す.
        if (!gaugeMng.IsFever())
        {
            spriteRenderer.color = Color.white;
            return;
        }

        // 色相を0～1の範囲で循環させる.
        hue += Time.deltaTime * rainbowSpeed;
        hue %= 1f;

        // HSVからRGBへ変換.
        spriteRenderer.color = Color.HSVToRGB(
            hue,
            saturation,
            brightness
        );
    }

    /// <summary>
    /// 右クリックした瞬間.
    /// </summary>
    void OnClickR()
    {
        //今回のクリックでの判定を開始.
        noteManager.BeginJudge();

        //全レーンを確認.
        for (int i = 0; i < laneSetting.laneAngle.Length; i++)
        {
            //最寄りのノーツ判定を行う.
            noteManager.JudgeNearestNote();
        }
    }
}
