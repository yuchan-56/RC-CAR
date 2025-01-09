using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // 좌우이동 + following player
    public Transform player;
    public float speed = 2f;
    public float wanderDistance = 2f;
    private Vector2 stopPosition;
    private bool isFollowing = false;

    private bool isDead = false;

    // Animation
    public Animator animator;

    // 거리 제한
    public float minDistance = 1.5f; // 멈추는 거리
    public float maxDistance = 1.6f;  // 다시 따라가기 시작하는 거리


    // Throw
    public GameObject throwableObjPrefab;
    public bool throwOn = false;
    public float throwForce = 12f;
    public float throwCooldown = 3f; // 던지기 공격 쿨다운 5초
    private float nextThrowAttack = 0f;

    // HP
    public float hp = 100f;
    public Slider hpSlider;
    public GameObject hpSliderPrefab; // Slider 프리팹 연결

    public float attackedRange = 1.5f;
    
    void Start()
    {
        stopPosition = transform.position;

        // HP Slider 프리팹 생성
        /// 캔버스 따로 필요함
        if (hpSliderPrefab != null)
        {
            GameObject sliderInstance = Instantiate(hpSliderPrefab, GameObject.Find("EnemyHPCanvas").transform);
            hpSlider = sliderInstance.GetComponent<Slider>();

            hpSlider.maxValue = hp;
            hpSlider.value = hp;
        }
    }

    void Update()
    {
        if (isDead)
        {
            return; // 더 이상 Update 로직 실행하지 않음
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (isFollowing)
        {
            if (distanceToPlayer > minDistance) 
            {
                FollowPlayer();
            }
        }
        else
        {
            Wander();
        }

        // 공격 조건: throwOn이 true이고, 일정 시간이 지나면 공격
        if (throwOn && Time.time >= nextThrowAttack)
        {
            ThrowObject(player);
            nextThrowAttack = Time.time + throwCooldown;
        }

        // HP 슬라이더 위치 업데이트
        if (hpSlider != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.7f, 0));
            hpSlider.transform.position = screenPosition;
        }
    }

    // -------- enemy move --------

    // trigger영역 - following player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayerTrigger")
        {
            isFollowing = true;
        }
    }

    // not trigger영역 - stop following player
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayerTrigger")
        {
            isFollowing = false;
            stopPosition = transform.position;
        }
    }

    // default
    void Wander()
    {
        float xPos = Mathf.PingPong(Time.time * speed, 5) - (5 / 2);
        transform.position = Vector2.Lerp(transform.position, new Vector3(stopPosition.x + xPos, transform.position.y), Time.deltaTime * speed);
    }

    void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }


    // ------------

    // Throw 공격
    void ThrowObject(Transform player)
    {
        GameObject throwable = Instantiate(throwableObjPrefab, transform.position, Quaternion.identity);
        Rigidbody2D throwableRb = throwable.GetComponent<Rigidbody2D>();

        if (throwableRb != null)
        {
            // 플레이어 위치 예측을 추가하여 더 정확한 타격 시도
            Vector2 playerVelocity = player.GetComponent<Rigidbody2D>()?.velocity ?? Vector2.zero;
            Vector2 predictedPosition = (Vector2)player.position + playerVelocity * (Vector2.Distance(transform.position, player.position) / throwForce);
            Vector2 direction = (predictedPosition - (Vector2)transform.position).normalized;
            Vector2 throwVelocity = new Vector2(direction.x, direction.y + 0.5f) * throwForce; // 포물선을 그리도록 Y값을 조정
            throwableRb.velocity = throwVelocity;
        }
    }

    // 일반 공격


    // ------------


    // HP 감소
    public void EnemyDamage(float damage)
    {
        hp -= damage;
        if (hpSlider != null)
        {
            hpSlider.value = hp; // 슬라이더 값 업데이트
        }

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        isDead = true;
        animator.SetBool("enemy_die", true);

        Destroy(hpSlider.gameObject, 1.7f); 
        Destroy(gameObject, 1.7f); // 애니메이션 이후 적 오브젝트 삭제    
    }

}
