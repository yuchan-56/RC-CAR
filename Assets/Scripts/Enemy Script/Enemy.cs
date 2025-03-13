using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // 좌우이동 + following player
    public GameObject player;
    public float speed = 2.5f;

    // 거리제한
    public float wanderDistance = 2f;
    bool isWandering = true;

    public float followDistance = 10f; // 따라가기 시작하는 거리
    public float throwDistance = 8f; // 오브젝트 던지기 시작하는 거리
    public float attackDistance = 2.0f; // 일반 공격 거리
    private Vector2 stopPosition;

    // 방향전환
    private bool facingRight = false; // 적의 현재 바라보는 방향
    private float lastXPosition = 0f;

    private bool isDead = false;


    // Animation
    public Animator animator;


    // Throw
    public GameObject throwableObjPrefab;
    public float throwForce = 12f;
    public float throwCooldown = 1.5f;
    private float nextThrowAttack = 0f;


    //attack
    public GameObject attackObject;


    // HP
    public float hp = 100f;
    public Slider hpSlider;
    public GameObject hpSliderPrefab; // Slider 프리팹 연결

    public float attackedRange = 1.5f;

    // Speech
    private bool speeched = false;
     
    void Start()
    {
        stopPosition = transform.position;
        player = GameObject.FindWithTag("Player");

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
        if (isDead) { return; }  // 더 이상 Update 로직 실행하지 않음

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);



        if (distanceToPlayer <= attackDistance)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= throwDistance && Time.time >= nextThrowAttack)
        {
            attackObject.SetActive(false);

            // Throw 공격
            ThrowObject(player.transform);
            nextThrowAttack = Time.time + throwCooldown;
        }
        else if (distanceToPlayer <= followDistance)
        {
            attackObject.SetActive(false);

            // 따라가기
            SpeechPopUp();
            FollowPlayer();
            isWandering = false;
        }
        else
        {
            attackObject.SetActive(false);

            // Wander 상태
            if (!isWandering)
            {
                stopPosition = transform.position; // 현재 위치를 Wander 시작점으로 설정
                isWandering = true;
            }  
            
            Wander();
        }
        

        // HP 슬라이더 위치 업데이트
        if (hpSlider != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.7f, 0));
            hpSlider.transform.position = screenPosition;
        }




        // 확인용
        if(Input.GetKeyDown(KeyCode.Space)) {
            EnemyDamage(10);
        }
    }







    // -------- enemy move --------

    void Wander()
    {
        animator.SetBool("enemy_attack", false);
        animator.SetBool("enemy_throw", false);

        // PingPong을 사용하여 이동 방향 결정
        float xPos = Mathf.PingPong(Time.time * speed, wanderDistance) - (wanderDistance / 2);

        // 이동 방향 변화 확인 및 FlipDirection 호출
        float horizontalDirection = xPos - lastXPosition;
        FlipDirection(horizontalDirection);

        // 위치 업데이트
        transform.position = new Vector2(stopPosition.x + xPos, transform.position.y);

        // 현재 X 위치 저장
        lastXPosition = xPos;
    }

    void FollowPlayer()
    {
        Vector2 directionToPlayer = player.transform.position - transform.position;
        FlipDirection(directionToPlayer.x);
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void FlipDirection(float horizontalDirection)
    {
        if (horizontalDirection > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalDirection < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }








    // Throw 공격
    void ThrowObject(Transform player)
    {
        Vector2 throwStartPosition = transform.position + new Vector3(0, 2.0f, 0); // 적의 키나 던지는 위치에 맞춰 조정

        GameObject throwable = Instantiate(throwableObjPrefab, throwStartPosition, Quaternion.identity);
        Rigidbody2D throwableRb = throwable.GetComponent<Rigidbody2D>();

        if (throwableRb != null)
        {
            animator.SetBool("enemy_throw", true);
            animator.SetBool("enemy_attack", false);

            // 플레이어 위치 예측을 추가하여 더 정확한 타격 시도
            Vector2 playerVelocity = player.GetComponent<Rigidbody2D>()?.velocity ?? Vector2.zero;
            Vector2 predictedPosition = (Vector2)player.position + playerVelocity * (Vector2.Distance(transform.position, player.position) / throwForce);
            Vector2 direction = (predictedPosition - (Vector2)transform.position).normalized;
            Vector2 throwVelocity = new Vector2(direction.x, direction.y + 0.3f) * throwForce; // 포물선을 그리도록 Y값을 조정
            throwableRb.velocity = throwVelocity;
        }
    }




    // 일반 공격
    void AttackPlayer()
    {
        animator.SetBool("enemy_throw", false);
        animator.SetBool("enemy_attack", true);

        if (attackObject != null) {
            attackObject.SetActive(true);
        }
    }

    

    void SpeechPopUp()
    {
        if (!speeched) // 만난적이 있는가?
        {
            Managers.Speech.speechTmp = this.gameObject;
            Managers.UI.ShowPopUpUI<SpeechBalloon>(); ; // 발견시 SpeechBallon 출력 
            speeched = true;
        }
    }





    // HP 감소
    public void EnemyDamage(float damage)
    {
        hp -= damage;
        Managers.Game.GetHit = true;
        if (hpSlider != null)
        {
            hpSlider.value = hp; // 슬라이더 값 업데이트
        }

        if (hp <= 0)
        {
            Die();
        }
        Debug.Log(damage);
        Debug.Log(hp);
    }

    void Die()
    {
        StartCoroutine(DieCoroutine()); 
    }

    IEnumerator DieCoroutine()
    {
        Debug.Log("Enemy died!");
        isDead = true;

        // 죽는 애니메이션 시작
        animator.SetBool("enemy_die", true);

        yield return new WaitForSeconds(1.4f);

        // HP 슬라이더 삭제
        if (hpSlider != null)
        {
            Destroy(hpSlider.gameObject);
        }

        // 적 오브젝트 삭제
        Destroy(gameObject);
        Managers.Game.EnemyDied();
    }


}
