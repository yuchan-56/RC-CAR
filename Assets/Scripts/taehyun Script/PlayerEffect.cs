using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerEffect : MonoBehaviour
{
    public float knockbackPower = 100f; // 넉백 힘
    public float knockbackDuration = 0.8f; // 넉백 지속 시간
    public float shakeDuration = 0.8f; // 움찔 지속 시간
    public float shakeStrength = 100f; // 움찔 강도
    public float blinkDuration = 0.2f; // 한 번 깜빡이는 시간
    public int blinkCount = 5; // 깜빡이는 횟수

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isHit = false; // 연속 피격 방지

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 가져오기
    }

    public void TakeHit(Vector2 hitDirection)
    {
        if (isHit) return; // 연속 피격 방지

        Debug.Log("피격 TakeHit 발동");
        isHit = true;

        // 1️⃣ DOTween으로 움찔하는 효과 (Shake)
        transform.DOShakePosition(shakeDuration, shakeStrength);

        // 2️⃣ Rigidbody2D를 이용한 넉백 적용
        rb.velocity = Vector2.zero; // 기존 속도 초기화
        rb.velocity = hitDirection * knockbackPower; // 넉백

        // 3️⃣ 깜빡거리는 효과 실행
        StartCoroutine(InvincibilityTimer());
        StartCoroutine(BlinkEffect());
    }

    private IEnumerator InvincibilityTimer()
    {
        yield return new WaitForSeconds(shakeDuration); // 무적 시간 유지
        isHit = false; // 무적 해제
    }

    private IEnumerator BlinkEffect()
    {
        
        for (int i = 0; i < blinkCount; i++)
        {
            // 반투명하게 만들기
            SetSpriteAlpha(0.3f);
            yield return new WaitForSeconds(blinkDuration);

            // 원래대로 돌리기
            SetSpriteAlpha(1f);
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    private void SetSpriteAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}

