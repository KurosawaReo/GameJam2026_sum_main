using UnityEngine;

/// <summary>
/// ノーツ譜面の設定.
/// </summary>
[CreateAssetMenu(fileName = "New File", menuName = "Game/Note Chart Setting")]
public class NoteChartSetting : ScriptableObject
{
    [Header(
        "- ノーツの配置 -\n\n" + 
        "Beat Count : 1で1拍分。ここで設定した時間でちょうど重なるように調整してくれる。\n" +
        "Lane No : どのレーンで流すか。LaneSetting の Lane Angle の番号を使う。\n" +
        "Parts : 体のどの部位を流すか。\n"
    )]
    public NoteData[] noteDatas; //ノーツデータ.   
}