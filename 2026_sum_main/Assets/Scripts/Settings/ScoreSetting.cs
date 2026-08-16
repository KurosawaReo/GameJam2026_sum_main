using UnityEngine;

/// <summary>
/// スコア関係の設定.
/// </summary>
[CreateAssetMenu(fileName = "New File", menuName = "Game/Score Setting")]
public class ScoreSetting : ScriptableObject
{
    [Header("- スコア -\n各評価で得られるスコアの量。")]
    public int perfectScore = 100;
    public int goodScore    = 50;
    public int badScore     = 0;

    [Header("- フィーバー倍率 -\nヤリラフィーバー中のスコアを何倍にするか。")]
    public int feverRate    = 10;
}