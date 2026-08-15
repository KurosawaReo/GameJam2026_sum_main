using UnityEngine;

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

        // アニメーションが1回でも再生されたことを確認.
        if (stateInfo.normalizedTime > 0f)
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