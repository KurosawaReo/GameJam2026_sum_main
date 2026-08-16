using UnityEngine;

/// <summary>
/// スコア関係の設定.
/// </summary>
[CreateAssetMenu(fileName = "New File", menuName = "Game/Score Setting")]
public class ScoreSetting : ScriptableObject
{
    [Header("- スコア -")]
    public int perfectScore = 100;
    public int goodScore    = 50;
    public int badScore     = 0;
}