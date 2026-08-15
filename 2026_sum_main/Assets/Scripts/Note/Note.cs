using UnityEngine;
using Common;

/// <summary>
/// ノーツ単体クラス.
/// </summary>
public class Note : MonoBehaviour
{
    int     laneNo;       //どのレーンにいるか.
    float   moveTime;     //目標位置までの移動時間.
    float   destroyTime;  //目標位置到達後、消滅するまでの時間.
    float   elapsed;      //経過時間.
    Vector3 startPos;     //スタート座標.
    Vector3 goalPos;      //ゴール座標.
    Vector3 moveDir;      //移動方向.

    bool initialized = false; //初期化済みか.

    /// <summary>
    /// レーン番号.
    /// </summary>
    public int GetLaneNo()
    {
        return laneNo;
    }

    /// <summary>
    /// 初期化.
    /// </summary>
    public void Init(Sprite _sprite, int _laneNo, float _moveTime, float _destroyTime, Vector3 _startPos, Vector3 _goalPos)
    {
        laneNo      = _laneNo;
        moveTime    = _moveTime;
        destroyTime = _destroyTime;
        elapsed     = 0.0f;
        startPos    = _startPos;
        goalPos     = _goalPos;

        // 初期化完了.
        initialized = true;

        //スタート地点を設定.
        transform.position = startPos;
        //スタートからゴールへの方向を取得.
        moveDir = (goalPos - startPos).normalized;
        //画像を設定.
        GetComponent<SpriteRenderer>().sprite = _sprite;
    }

    void Update()
    {
        //初期化されたら実行.
        if (initialized)
        {
            Move();
        }
    }

    /// <summary>
    /// 移動処理.
    /// </summary>
    private void Move()
    {
        // 経過時間を加算.
        elapsed += Time.deltaTime;

        // 移動時間内なら、スタートからゴールまで移動.
        if (elapsed < moveTime)
        {
            float rate = elapsed / moveTime;
            transform.position = Vector3.Lerp(startPos, goalPos, rate);
            return;
        }

        // ゴール到達後は、そのまま同じ速度で通過させる.
        float overTime = elapsed - moveTime;
        float speed = Vector3.Distance(startPos, goalPos) / moveTime;
        transform.position = goalPos + moveDir * speed * overTime;

        // 指定時間通過したら消滅.
        if (overTime >= destroyTime)
        {
            Destroy();
        }
    }

    /// <summary>
    /// 消滅.
    /// </summary>
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
