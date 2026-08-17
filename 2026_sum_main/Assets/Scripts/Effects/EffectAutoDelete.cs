using UnityEngine;

/// <summary>
/// アニメーション終了時に自身を削除する.
/// </summary>
public class EffectAutoDelete : MonoBehaviour
{
    Animator animator; // Animatorコンポーネント.
    bool animationStarted = false; // アニメーションが開始されたか.

    void Start()
    {
        // Animatorを取得.
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Animatorがなければ処理しない.
        if (animator == null)
        {
            return;
        }

        // 現在のアニメーション情報を取得.
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // アニメーションが再生中になったことを確認.
        if (!animationStarted && stateInfo.normalizedTime > 0f)
        {
            animationStarted = true;
        }

        // 再生開始後、アニメーションが終了したら削除.
        if (animationStarted && stateInfo.normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
    }
}