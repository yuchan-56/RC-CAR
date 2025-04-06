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
    bool canMove = true;

    private EnemyState currentState;


    // 거리제한
    public float wanderDistance = 2f;
    bool isWandering = true;
    bool isFollowing = true;
    public bool isEnemyHit = false;

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
    public float throwForce = 15f;
    public float throwCooldown = 1.5f;
    Vector2 throwVelocity;


    //attack
    public GameObject attackObject;

    public bool isMale = false;

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
        if (isDead) { return; }
        
        if(canMove) {

            float distanceToPlayer = Mathf.Abs(transform.position.x - player.transform.position.x);
            float distanceToPlayerY = Mathf.Abs(transform.position.y - player.transform.position.y);

            if (distanceToPlayer <= attackDistance && distanceToPlayerY <= followDistanceY)
            {
                if(isMale) {
                    currentState = EnemyState.Attacking;
                } else {
                    currentState = EnemyState.Throwing;
                }
                
            }
            else if (distanceToPlayer <= throwDistance && distanceToPlayerY <= followDistanceY)
            {
                if(!isMale) {
                    currentState = EnemyState.Throwing;
                } else {
                    currentState = EnemyState.Following;
                }
                
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
                    canMove = false;
                    animator.SetBool("enemy_throw", true);
                    animator.SetBool("enemy_attack", false);

                    attackObject.SetActive(false);
                    isFollowing = false;
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
        }
        

        // HP 슬라이더 위치 업데이트
        if (hpBarImage != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.7f, 0));
            hpBarImage.transform.position = screenPosition;
        }




        // 확인용
        if(Input.GetKeyDown(KeyCode.Space)) {
            EnemyDamage(10, 1);
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
        if (!canMove) return;

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
    public void EnemyThrowing()
    {
        Vector2 throwStartPosition = transform.position + new Vector3(0, 2.0f, 0); // 적의 키나 던지는 위치에 맞춰 조정

        GameObject throwable = Instantiate(throwableObjPrefab, throwStartPosition, Quaternion.identity);
        Rigidbody2D throwableRb = throwable.GetComponent<Rigidbody2D>();

        if (throwableRb != null)
        {
            animator.SetBool("enemy_throw", true);
            animator.SetBool("enemy_attack", false);

            float directionX = facingRight ? 1f : -1f;
            throwVelocity = new Vector2(directionX, 0.8f) * throwForce;
            
            throwableRb.velocity = throwVelocity;
        }
    }




    // throwing 마지막 프레임 
    public void CanMove() {
        Invoke("RealCanMove", 0.3f);
        
    }

    void RealCanMove() {
        canMove = true;
    }




    // 일반 공격
    void AttackPlayer()
    {
        canMove = false;

        animator.SetBool("enemy_throw", false);
        animator.SetBool("enemy_attack", true);
    }

    public void OnAttackStart()
    {
        attackObject.SetActive(true);
    }

    public void OnAttackEnd()
    {
        attackObject.SetActive(false);
        canMove = true;
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
    public void EnemyDamage(float damage, int attackMethod)
    {
        animator.SetBool("enemy_attacked", true);
        canMove = false;
        isEnemyHit = true;
        
        if(!isDead) {
            if(attackMethod == 1) {
                //attack
                StartCoroutine(IsAttacked(1.8f));
            }
            else if(attackMethod == 2) {
                //jump attack
                StartCoroutine(JumpAttacked());
            }
            else if(attackMethod == 3) {
                //dash attack
                StartCoroutine(IsAttacked(3.0f));
            }
        }
        
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

        isEnemyHit = false;
        animator.SetBool("enemy_hit", false);
    }

    IEnumerator IsAttacked(float knockback) {
        Vector2 knockbackDir = (transform.position - player.transform.position).normalized;
        float knockbackDistance = 1.0f;

        Vector2 startPos = transform.position;
        Vector2 targetPos = startPos + knockbackDir * knockbackDistance;

        float duration = 0.2f; // 밀리는 데 걸리는 시간
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // 마지막 위치 보정


        yield return new WaitForSeconds(0.5f);
        
    }

    IEnumerator JumpAttacked() {
        Vector2 startPos = transform.position;

        // 위로 튀는 위치 계산
        Vector2 peakPos = startPos + new Vector2(0, 1.2f);
        Vector2 endPos = startPos;

        float duration = 0.6f; // 전체 시간
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            float height = 4f * t * (1 - t);
            Vector2 midPos = Vector2.Lerp(startPos, endPos, t); 
            transform.position = new Vector2(midPos.x, startPos.y + height * 1.5f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        
        yield return new WaitForSeconds(1.0f);
    }

    public void CanMoveAtt() {
        canMove = true;
        animator.SetBool("enemy_attacked", false);

        //currentState = EnemyState.Following;
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        isDead = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // 죽는 애니메이션 시작
        animator.SetBool("enemy_die", true);

        // HP 슬라이더 삭제
        if (hpBarImage != null)
        {
            Destroy(hpBarImage.gameObject);
        }
    }

    public void EnemyDying() {
        Destroy(gameObject);
        Managers.Game.EnemyDied();
    }

}
