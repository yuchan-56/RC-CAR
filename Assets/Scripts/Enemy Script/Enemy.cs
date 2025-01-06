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

    // 거리 제한
    public float minDistance = 1.5f; // 멈추는 거리
    public float maxDistance = 1.6f;  // 다시 따라가기 시작하는 거리

    // Attack
    // Shooting
    public GameObject bulletPref;
    public bool shootOn = false;
    float bulletSpeed = 10.0f;
    float spawnInterval = 0.6f;
    float nextSpawn = 0f;

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
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (isFollowing)
        {
            if (distanceToPlayer > minDistance) 
            {
                FollowPlayer();
            }

            // 공격 조건: shootOn이 true이고, 일정 시간이 지나면 공격
            if (shootOn && Time.time >= nextSpawn)
            {
                Shooting(player);
                nextSpawn = Time.time + spawnInterval;
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


        // 스페이스바를 눌렀을 때 HP 감소
        /// 임시
        /// 버튼 이벤트 받아야함!!
        if (distanceToPlayer <= attackedRange)
        {
            if (Input.GetKeyDown(KeyCode.X)) // 일반 공격 버튼
            {
                TakeDamage(10f);
                Debug.Log("E.hp -10");
            }
            else if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.Space)) // 대쉬 + 공격
            {
                TakeDamage(20f);
                Debug.Log("E.hp -20");
            }
            else if (Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.Space)) // 점프 + 공격
            {
                TakeDamage(15f);
                Debug.Log("E.hp -15");
            }
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

    // 공격1
    void Shooting(Transform player)
    {
        GameObject bullet = Instantiate(bulletPref, transform.position, Quaternion.identity);
        Rigidbody2D bulletrb = bullet.GetComponent<Rigidbody2D>();

        if(bulletrb != null) {
            Vector2 direction = (player.position - transform.position).normalized;
            bulletrb.velocity = direction * bulletSpeed;
        }
    }

    // 공격2
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

    // 공격 3
    // void PillowAttack() { }
    // 애니메이션 필요





    // ------------


    // HP 감소
    void TakeDamage(float damage)
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
        Destroy(hpSlider.gameObject); // 슬라이더 삭제
        Destroy(gameObject); // 게임 오브젝트를 삭제하여 적을 죽임
    }

}
