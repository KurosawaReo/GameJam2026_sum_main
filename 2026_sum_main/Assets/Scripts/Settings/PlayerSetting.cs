using UnityEngine;

/// <summary>
/// プレイヤーデータ.
/// </summary>
[CreateAssetMenu(fileName = "New File", menuName = "Game/Player Setting")]
public class PlayerSetting : ScriptableObject
{
    [Header("- 画像 -")]
    public Sprite[] imgPlayer;

    [Header("- 切り替え間隔 -")]
    public float changeInterval = 1.0f;
}