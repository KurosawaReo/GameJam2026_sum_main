using UnityEngine;

/// <summary>
/// ゲージ関係の設定.
/// </summary>
[CreateAssetMenu(fileName = "New File", menuName = "Game/Gauge Setting")]
public class GaugeSetting : ScriptableObject
{
    [Header("- ゲージ最大値 -")]
    public float maxGauge = 20f;          // ゲージ最大値.

    [Header("- 評価変動量 -")]
    public float perfectGauge = 1f;       // PERFECT時の変動量.
    public float goodGauge = 0f;          // GOOD時の変動量.
    public float badGauge = -1f;          // BAD時の変動量.

    [Header("- フィーバー -")]
    public float feverDuration = 5f;      // フィーバー効果時間(秒).

    [Header("- アニメーション -")]
    public float gaugeUpSpeed = 2f;       // ゲージ増加の表示速度.
}