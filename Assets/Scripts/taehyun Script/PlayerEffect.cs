using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerEffect : MonoBehaviour
{
    public float knockbackPower = 100f; // 넉백 힘
    public float knockbackDuration = 0.8f; // 넉백 지속 시간
    public float shakeDuration = 0.2f; // 움찔 지속 시간, 무적시간 0.8f 기본
    public float shakeStrength = 0.3f; // 움찔 강도
    public float blinkDuration = 0.2f; // 한 번 깜빡이는 시간
    public int blinkCount = 5; // 깜빡이는 횟수

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public bool isHit = false; // 연속 피격 방지
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 가져오기
    }

    private IEnumerator HandleHit(Vector2 hitDirection)
    {
            Debug.Log("피격불가");  // (1) 이제 정확한 시점에 실행됨
            isHit = true;
            Managers.Game.isHit = true;

            // 1️⃣ DOTween으로 움찔하는 효과
            transform.DOShakePosition(shakeDuration, shakeStrength);

            // 2️⃣ Rigidbody2D를 이용한 넉백 적용
            rb.velocity = Vector2.zero;
            rb.velocity = hitDirection * knockbackPower;

            // 3️⃣ 깜빡거리는 효과 실행
            StartCoroutine(BlinkEffect());
            StartCoroutine(InvincibilityTimer());
            // 🎯 여기서 무적 시간만큼 기다림 (이제 즉시 실행되지 않음)
            yield return new WaitForSeconds(shakeDuration+1);
    }

    public void TakeHit(Vector2 hitDirection)
    {
        if (!Managers.Game.isHit) // 이미 무적 상태면 피격 무시
        { 
        StartCoroutine(HandleHit(hitDirection));
        }
    }

  
    private IEnumerator InvincibilityTimer()
    {
        yield return new WaitForSeconds(shakeDuration); // 무적 시간 유지
        Debug.Log("피격가능");
        Managers.Game.isHit = false; // 무적 해제
        isHit = false;
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

