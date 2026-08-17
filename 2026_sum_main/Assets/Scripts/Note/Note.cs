using UnityEngine;

/// <summary>
/// ノーツ単体クラス.
/// </summary>
public class Note : MonoBehaviour
{
    int     laneNo;         // どのレーンにいるか.
    float   noteTime;       // 判定位置に到達するBGM時間.
    float   moveTime;       // 目標位置までの移動時間.
    float   spawnTime;      // ノーツが出現するBGM時間.
    Vector3 startPos;       // スタート座標.
    Vector3 goalPos;        // ゴール座標.
    Vector3 moveDir;        // 移動方向.

    bool initialized = false; // 初期化済みか.

    /// <summary>
    /// レーン番号.
    /// </summary>
    public int GetLaneNo() => laneNo;

    /// <summary>
    /// 判定時間.
    /// </summary>
    public float GetNoteTime() => noteTime;

    /// <summary>
    /// 初期化.
    /// </summary>
    public void Init(Sprite _sprite, int _laneNo, float _noteTime, float _moveTime, Vector3 _startPos, Vector3 _goalPos)
    {
        laneNo = _laneNo;
        noteTime = _noteTime;
        moveTime = _moveTime;
        startPos = _startPos;
        goalPos = _goalPos;

        // ノーツの出現時間を計算.
        spawnTime = noteTime - moveTime;

        // 初期化完了.
        initialized = true;

        // 初期位置を設定.
        transform.position = startPos;

        // スタートからゴールへの方向を取得.
        moveDir = (goalPos - startPos).normalized;

        // 画像を設定.
        GetComponent<SpriteRenderer>().sprite = _sprite;
    }

    void Update()
    {
        // 初期化されていなければ何もしない.
        if (!initialized)
        {
            return;
        }

        Move();
    }

    /// <summary>
    /// 移動処理.
    /// </summary>
    private void Move()
    {
        // 現在のBGM時間を取得.
        float currentTime = SoundManager.Inst.GetTimeBGM();

        // 出現してからの経過時間をBGM時間から計算.
        float elapsed = currentTime - spawnTime;

        // 移動時間内なら、スタートからゴールまで移動.
        if (elapsed < moveTime)
        {
            float rate = Mathf.Clamp01(elapsed / moveTime);
            transform.position = Vector3.Lerp(startPos, goalPos, rate);
            return;
        }

        // ゴール到達後は、そのまま同じ速度で通過.
        float overTime = elapsed - moveTime;
        float speed = Vector3.Distance(startPos, goalPos) / moveTime;

        transform.position = goalPos + moveDir * speed * overTime;
    }

    /// <summary>
    /// 消滅.
    /// </summary>
    public void Destroy()
    {
        Destroy(gameObject);
    }
}