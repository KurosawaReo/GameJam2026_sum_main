using UnityEngine;
using Common;

/// <summary>
/// 全シーンで共有するデータ管理クラス.
/// </summary>
public class AllSceneData : MonoBehaviour
{
    // シングルトン用.
    public static AllSceneData Inst;

    // 現在のスコア.
    public int YariraScore { get; set; }

    // 判定回数.
    public int CountPerfect { get; set; }
    public int CountGood { get; set; }
    public int CountBad { get; set; }

    // 使用する譜面.
    public NoteChartType ChartType { get; set; }

    void Awake()
    {
        // 既に存在する場合は自身を削除.
        if (Inst != null)
        {
            Destroy(gameObject);
            return;
        }

        // 自身をシングルトンとして登録.
        Inst = this;

        // シーン遷移後も残す.
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ゲーム結果を初期化.
    /// </summary>
    public void ResetData()
    {
        YariraScore = 0;
        CountPerfect = 0;
        CountGood = 0;
        CountBad = 0;
    }
}