using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    Collider2D playerCollider;
    Collider2D enemyCollider;
    // 좌우이동 + following player
    public GameObject player;
    public float speed = 2.5f;


    public enum EnemyState { Idle, Following, Attacking, Throwing }

    private EnemyState currentState;


    // 거리제한
    public float wanderDistance = 2f;
    bool isWandering = true;
    bool isFollowing = true;

    public float followDistance = 10f; // 따라가기 시작하는 거리
    public float followDistanceY = 3f; // 따라가기 시작하는 Y축거리
    public float throwDistance = 5f; // 오브젝트 던지기 시작하는 거리
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
    public GameObject hpBarPrefab;
    private Image hpBarImage;
    public Sprite[] hpSprites;
    public int maxHP = 3;
    private int currentHP;
    private Transform canvasTransform;

    // Speech
    private bool speeched = false;


     
    void Start()
    {
        stopPosition = transform.position;
        player = GameObject.FindWithTag("Player");

        playerCollider = player.GetComponent<Collider2D>();
        enemyCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(playerCollider, enemyCollider);

        currentHP = maxHP;

        canvasTransform = GameObject.Find("EnemyHPCanvas").transform;

        GameObject newHpBar = Instantiate(hpBarPrefab, canvasTransform);
        hpBarImage = newHpBar.GetComponent<Image>();

        hpBarImage.sprite = hpSprites[(int)currentHP];
    }

    void Update()
    {
        if (isDead) { return; }  // 더 이상 Update 로직 실행하지 않음

        float distanceToPlayer = Mathf.Abs(transform.position.x - player.transform.position.x);
        float distanceToPlayerY = Mathf.Abs(transform.position.y - player.transform.position.y);

        if (distanceToPlayer <= attackDistance && distanceToPlayerY <= followDistanceY)
        {
            currentState = EnemyState.Attacking;
        }
        else if (distanceToPlayer <= throwDistance && distanceToPlayerY <= followDistanceY)
        {
            currentState = EnemyState.Throwing;
        }
        else if (distanceToPlayer <= followDistance&&distanceToPlayerY <= followDistanceY)
        {
            currentState = EnemyState.Following;
        }
        else
        {
            currentState = EnemyState.Idle;
        }

        switch (currentState)
        {
            case EnemyState.Attacking:
                AttackPlayer();
                break;
            
            case EnemyState.Throwing:
                if (Time.time >= nextThrowAttack)
                {
                    attackObject.SetActive(false);
                    isFollowing = false;
                    ThrowObject(player.transform);
                    nextThrowAttack = Time.time + throwCooldown;
                }
                break;
            
            case EnemyState.Following:
                attackObject.SetActive(false);
                SpeechPopUp();
                FollowPlayer();
                isWandering = false;
                break;
            
            case EnemyState.Idle:
                attackObject.SetActive(false);
                if (!isWandering)
                {
                    stopPosition = transform.position;
                    isWandering = true;
                }
                Wander();
                break;
        }
        

        // HP 슬라이더 위치 업데이트
        if (hpBarImage != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.7f, 0));
            hpBarImage.transform.position = screenPosition;
        }




        // 확인용
        if(Input.GetKeyDown(KeyCode.Space)) {
            EnemyDamage(10);
        }
    }

    void UpdateHPBar()
    {
        if (currentHP >= 0 && currentHP < hpSprites.Length)
        {
            hpBarImage.sprite = hpSprites[(int)currentHP];  // HP 이미지 변경
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
        animator.SetBool("enemy_attack", false);
        animator.SetBool("enemy_throw", false);

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

            Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            Vector2 throwVelocity = new Vector2(direction.x, direction.y + 0.3f) * throwForce;
            throwableRb.velocity = throwVelocity;
        }
    }




    // 일반 공격
    void AttackPlayer()
    {
        animator.SetBool("enemy_throw", false);
        animator.SetBool("enemy_attack", true);

        if (attackObject != null) {
            StartCoroutine(EnemyAttack(0.8f));
        }
    }

    IEnumerator EnemyAttack(float delay) {
        yield return new WaitForSeconds(delay);

        attackObject.SetActive(true);
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
        StartCoroutine(IsAttacked());

        // 피격 애니메이션 적용
        IsHit();

        if(currentHP > 0) {
            currentHP --;
            UpdateHPBar();
        }
        
        
        if(currentHP <= 0) {
            Die();
        }
        Debug.Log(currentHP);
        
        Managers.Game.GetHit = true;        
    }

    IEnumerator IsHit()
    {
        animator.SetBool("enemy_hit", true);

        yield return new WaitForSeconds(0.22f);

        animator.SetBool("enemy_hit", false);
    }

    IEnumerator IsAttacked() {
        animator.SetBool("enemy_attacked", true);

        yield return new WaitForSeconds(0.3f);
        animator.SetBool("enemy_attacked", false);
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

        // HP 슬라이더 삭제
        if (hpBarImage != null)
        {
            Destroy(hpBarImage.gameObject);
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length; // 애니메이션 길이 가져오기

        yield return new WaitForSeconds(animationLength * 1.02f);

        

        // 적 오브젝트 삭제
        Destroy(gameObject);
        Managers.Game.EnemyDied();
    }


}
