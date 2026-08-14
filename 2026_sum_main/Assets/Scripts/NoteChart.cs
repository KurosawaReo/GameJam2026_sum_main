using UnityEngine;

/// <summary>
/// ノーツ譜面データ.
/// </summary>
[CreateAssetMenu(fileName = "NoteChart", menuName = "Game/Note Chart")]
public class NoteChart : ScriptableObject
{
    [Header("- note -")]
    public NoteData[] noteDatas; // ノーツデータ.
}