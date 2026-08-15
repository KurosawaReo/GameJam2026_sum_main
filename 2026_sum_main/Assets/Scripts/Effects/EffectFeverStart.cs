using UnityEngine;

public class EffectFeverStart : MonoBehaviour
{
    Animator animator; // Animatorコンポーネント.

    void Start()
    {
        // Animatorを取得.
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // アニメーションが再生中か確認.
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            // アニメーション終了後に自身を削除.
            Destroy(gameObject);
        }
    }
}