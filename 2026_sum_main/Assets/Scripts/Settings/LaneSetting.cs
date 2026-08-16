using UnityEngine;

/// <summary>
/// レーン関係の設定.
/// </summary>
[CreateAssetMenu(fileName = "New File", menuName = "Game/Lane Setting")]
public class LaneSetting : ScriptableObject
{
    [Header(
        "- レーン設定 -\n\n" +
        "Lane Angle : レーンの流れる角度の設定。Elementの数字がレーン番号。\n"
    )]
    public float[] laneAngle;          //レーンごとの角度.
    public Vector3 goalPos;            //目標地点.
    public float   dist = 1;           //距離.
    public float   moveTime = 1;       //何秒で中心に移動するか.
    public float   destroyTime = 1;    //中心到達後、何秒で消滅するか.

    [Header("- リズム判定設定 -")]
    public float badBeat = 0.25f;        //BAD判定になる最大の拍ズレ.
    public float goodBeat = 0.15f;       //GOOD判定になる最大の拍ズレ.
    public float perfectBeat = 0.05f;    //PERFECT判定になる最大の拍ズレ.
}