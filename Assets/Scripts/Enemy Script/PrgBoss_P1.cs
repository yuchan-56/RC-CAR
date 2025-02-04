using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrgBoss_P1 : MonoBehaviour
{
    public float knockbackForce = 20f; // 튕겨나가는 힘 크기
    public float knockbackDuration = 0.5f; // 밀려나는 지속 시간
    public float dragAmount = 5f; // 감속을 위한 드래그 값
    public float gravityScaleDuringKnockback = 0.5f; // 튕겨나는 동안 중력 값

    private Vector2 chargeDirection; // 보스의 진행 방향
    private Rigidbody2D bossRb; // 부모(보스)의 Rigidbody2D 참조

    private Rigidbody2D playerRb; // 플레이어의 Rigidbody2D 참조
    private Vector2 knockbackDirection; // 플레이어가 튕겨나갈 방향
    private bool shouldKnockback = false; // FixedUpdate에서 Knockback 실행 여부

    void Start()
    {
        // 부모(보스)의 Rigidbody2D 찾기
        if (transform.parent != null)
        {
            bossRb = transform.parent.GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        if (bossRb != null)
        {
            chargeDirection = bossRb.velocity.normalized; // 부모의 이동 방향 가져오기
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 플레이어가 맞았을 때
        {
            playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                knockbackDirection = chargeDirection;
                if (knockbackDirection == Vector2.zero)
                {
                    knockbackDirection = Vector2.left; // 기본 방향
                }

                Debug.Log("Knockback Direction: " + knockbackDirection);

                shouldKnockback = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (shouldKnockback && playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.drag = dragAmount; // 드래그 적용
            playerRb.gravityScale = gravityScaleDuringKnockback; // 중력 조절

            bossRb.constraints |= RigidbodyConstraints2D.FreezePositionX;
            bossRb.constraints |= RigidbodyConstraints2D.FreezePositionY;

            playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            StartCoroutine(ResetKnockback(playerRb));

            shouldKnockback = false;
        }
    }

    private IEnumerator ResetKnockback(Rigidbody2D playerRb)
    {
        //Debug.Log("코루틴 시작");

        yield return new WaitForSeconds(knockbackDuration);

        //Debug.Log("준비중");

        if(bossRb == null) {
            Debug.LogError("boss null");
            yield break;
        }

        if(playerRb == null) {
            Debug.Log("player null");
            yield break;
        }

        //Debug.Log("시시시작");

        bossRb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        bossRb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;

        // 원래 드래그 및 중력 값 복원
        playerRb.drag = 0f; // 드래그 초기화
        playerRb.gravityScale = 1f; // 원래 중력 값으로 복구
        playerRb.velocity = Vector2.zero;

        //Debug.Log("끝");
    }
}
