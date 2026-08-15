using UnityEngine;
using System;

/// <summary>
/// ノーツデータ.
/// </summary>
[Serializable]
public class ImgPlayerData
{
    [Header("- 全身 -")]
    public Sprite main; //全身の画像.

    [Header("- 各部位 -")]
    public Sprite head; //頭.
    public Sprite armL; //左腕.
    public Sprite armR; //右腕.
    public Sprite legL; //左脚.
    public Sprite legR; //右脚.
}