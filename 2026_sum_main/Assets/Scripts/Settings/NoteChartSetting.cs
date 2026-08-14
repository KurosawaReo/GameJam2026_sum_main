using UnityEngine;

/// <summary>
/// ノーツ譜面データ.
/// </summary>
[CreateAssetMenu(fileName = "NoteChart", menuName = "Game/Note Chart Setting")]
public class NoteChartSetting : ScriptableObject
{
    [Header("- ノーツの配置 -")]
    public NoteData[] noteDatas; //ノーツデータ.
}