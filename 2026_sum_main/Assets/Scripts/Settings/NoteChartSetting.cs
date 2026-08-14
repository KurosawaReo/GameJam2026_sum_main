using UnityEngine;

/// <summary>
/// ノーツ譜面データ.
/// </summary>
[CreateAssetMenu(fileName = "New File", menuName = "Game/Note Chart Setting")]
public class NoteChartSetting : ScriptableObject
{
    [Header("- ノーツの配置 -")]
    public NoteData[] noteDatas; //ノーツデータ.
}