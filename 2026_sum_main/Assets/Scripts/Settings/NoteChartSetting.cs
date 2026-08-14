using UnityEngine;

/// <summary>
/// ノーツ譜面データ.
/// </summary>
[CreateAssetMenu(fileName = "New File", menuName = "Game/Note Chart Setting")]
public class NoteChartSetting : ScriptableObject
{
    [Header(
        "- ノーツの配置 -\n\n" + 
        "Time : 単位は秒。ここで設定した時間でちょうど重なるように調整してくれる。\n" +
        "Lane No : どのレーンで流すか。LaneSetting の Lane Angle の番号を使う。\n"
    )]
    public NoteData[] noteDatas; //ノーツデータ.
}