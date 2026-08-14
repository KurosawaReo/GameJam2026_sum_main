using UnityEngine;

/// <summary>
/// プレイヤー管理クラス.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] NoteManager noteManager;

    [Header("- position -")]
    [SerializeField] Vector3 playerPos;

    void Start()
    {
        
    }

    void Update()
    {
        //左クリックした瞬間.
        if (Input.GetMouseButtonDown(0))
        {
            //TODO クリックしても距離が0になってる問題 <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            noteManager.JudgeNearestNote(playerPos); //最寄りのノーツ判定を行う.
        }
    }
}
