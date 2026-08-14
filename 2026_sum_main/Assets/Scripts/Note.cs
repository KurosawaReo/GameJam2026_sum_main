using UnityEngine;

/// <summary>
/// ノーツ単体クラス.
/// </summary>
public class Note : MonoBehaviour
{
    float   speed;
    Vector3 goalPos;

    /// <summary>
    /// 初期化.
    /// </summary>
    public void Init(float _speed, Vector3 _goalPos)
    {
        speed   = _speed;
        goalPos = _goalPos;
    }

    void Update()
    {
        //目標地点へ一定速度で移動.
        transform.position = Vector3.MoveTowards(
            transform.position,
            goalPos,
            speed * Time.deltaTime
        );
        //目標地点に到達したら消滅.
        if (transform.position == goalPos)
        {
            Destroy(gameObject);
        }
    }
}
