using Common;
using System;
using UnityEditor;

/// <summary>
/// ノーツデータ.
/// </summary>
[Serializable]
public class NoteData
{
    public float     beatCount; //BGM開始から何拍で判定地点へ到達するか.
    public int       laneNo;    //ノーツのレーン番号.
    public BodyParts parts;     //どの部位を流すか.
}