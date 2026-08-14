using System;

/// <summary>
/// ノーツデータ.
/// </summary>
[Serializable]
public class NoteData
{
    public float time;   //曲開始から何秒後に判定地点へ到達するか.
    public int   laneNo; //ノーツのレーン番号.
}