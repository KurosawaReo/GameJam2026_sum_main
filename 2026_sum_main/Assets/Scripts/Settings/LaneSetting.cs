using UnityEngine;

/// <summary>
/// レーン関係のデータ.
/// </summary>
[CreateAssetMenu(fileName = "New File", menuName = "Game/Lane Setting")]
public class LaneSetting : ScriptableObject
{
    [Header("- レーン設定 -")]
    public float[] laneAngle;          //レーンごとの角度.
    public Vector3 goalPos;            //目標地点.
    public float   dist = 1;           //距離.
    public float   moveTime = 1;       //何秒で中心に移動するか.
    public float   destroyTime = 1;    //中心到達後、何秒で消滅するか.

    [Header("- 判定設定 -")]
    public float   badDist = 1;        //BAD判定になる距離.
    public float   goodDist = 1;       //GOOD判定になる距離.
    public float   perfectDist = 1;    //PERFECT判定になる距離.
}