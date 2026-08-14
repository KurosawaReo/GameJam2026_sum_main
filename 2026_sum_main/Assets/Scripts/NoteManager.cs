using UnityEngine;
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

public class NoteManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject prfbNote;   //ノーツprefab.

    [Header("- lane -")]
    [SerializeField] float[]    laneAngle;  //レーンごとの角度.

    [Header("- note -")]
    [SerializeField] NoteData[] noteDatas;  //ノーツデータ.
    [SerializeField] Vector3    goalPos;    //目標地点.
    [SerializeField] float      dist;       //距離.
    [SerializeField] float      speed;      //移動速度.

    int noteIndex; //次に生成するノーツの番号.

    void Start()
    {
        noteIndex = 0;
    }

    void Update()
    {
        //まだ生成していないノーツがあるか確認.
        if (noteIndex >= noteDatas.Length)
        {
            return;
        }

        //指定時間になったらノーツを生成.
        if (Time.time >= noteDatas[noteIndex].time)
        {
            SpawnNote(noteDatas[noteIndex]);
            noteIndex++;
        }
    }

    /// <summary>
    /// ノーツを生成.
    /// </summary>
    private void SpawnNote(NoteData data)
    {
        //ノーツ生成.
        var objNote  = Instantiate(prfbNote);
        var scptNote = objNote.GetComponent<Note>();

        //速度設定.
        scptNote.Init(speed, goalPos);
        //角度抽選.
        float angle = laneAngle[data.laneNo];
        //ベクトル.
        Vector3 vec = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        //初期位置の設定.
        objNote.transform.position = goalPos + vec * dist;
    }
}
