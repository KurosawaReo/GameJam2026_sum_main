using UnityEngine;

public class NoteManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject prfbNote;   //ノーツprefab.
    [SerializeField] GameObject InPrefab;

    [Header("- lane -")]
    [SerializeField] float[]    laneAngle;  //レーンごとの角度.

    [Header("- note -")]
    [SerializeField] NoteChart  noteChart;  //ノーツ譜面データ.
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
        //すべてのノーツを生成したら終了.
        if (noteIndex >= noteChart.noteDatas.Length)
        {
            return;
        }

        //指定時間になったらノーツを生成.
        if (Time.time >= noteChart.noteDatas[noteIndex].time)
        {
            SpawnNote(noteChart.noteDatas[noteIndex]);
            noteIndex++;
        }
    }

    /// <summary>
    /// ノーツを生成.
    /// </summary>
    private void SpawnNote(NoteData data)
    {
        //ノーツ生成.
        var objNote  = Instantiate(prfbNote, InPrefab.transform);
        var scptNote = objNote.GetComponent<Note>();

        //速度と目標地点を設定.
        scptNote.Init(speed, goalPos);

        //レーンの角度を取得.
        float angle = laneAngle[data.laneNo];

        //角度から方向ベクトルを作成.
        Vector3 vec = new Vector3(
            Mathf.Cos(angle),
            Mathf.Sin(angle),
            0
        );

        //初期位置を設定.
        objNote.transform.position = goalPos + vec * dist;
    }
}
