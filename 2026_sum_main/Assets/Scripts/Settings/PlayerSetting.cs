using UnityEngine;

/// <summary>
/// プレイヤーの設定.
/// </summary>
[CreateAssetMenu(fileName = "New File", menuName = "Game/Player Setting")]
public class PlayerSetting : ScriptableObject
{
    [Header(
        "- 画像 -\n\n" +
        "ここでセットした画像を使って順番に切り替わる。\n"
    )]
    public ImgPlayerData[] image;

    [Header("- 切り替え間隔(秒) -")]
    public float changeInterval = 1.0f;
}