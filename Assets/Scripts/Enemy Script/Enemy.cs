using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, EnemyHP
{
    //공통 인터페이스
    public int EnemyHp { get; set; }
    public bool IsEnemyHit { get; set; }
    public bool IsEnemyDead { get; set; }

    Collider2D playerCollider;
    Collider2D enemyCollider;
    // 좌우이동 + following player
    public GameObject player;
    public float speed = 5f;


    public enum EnemyState { Idle, Following, Attacking, Throwing }
    bool canMove = true;

    private EnemyState currentState;
    private EnemyState previousState;
    //private float wanderStartTime;   // Wander 시작 시간 (PingPong 타이밍 기준)
    //private float lastWorldX; 


    // 거리제한
    public float wanderDistance = 2f;
    bool isWandering = true;
    bool isFollowing = true;

    private bool isHitOverride = false;

    private float followDistance = 6f; // 따라가기 시작하는 거리
    private float followDistanceY = 3f; // 따라가기 시작하는 Y축거리
    private float throwDistance = 4f; // 오브젝트 던지기 시작하는 거리
    private float attackDistance = 2.0f; // 일반 공격 거리
    //private Vector2 stopPosition;

    // 방향전환
    private bool facingRight = false; // 적의 현재 바라보는 방향
    private int facingRightSign => facingRight ? 1 : -1;


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
    private int currentHP;
    private Transform canvasTransform;

    // Speech
    private bool speeched = false;


     
    void Start()
    {
        //stopPosition = transform.position;
        player = GameObject.FindWithTag("Player");

        playerCollider = player.GetComponent<Collider2D>();
        enemyCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(playerCollider, enemyCollider);

        EnemyHp = 3;
        IsEnemyHit = false;
        IsEnemyDead = false;
        currentHP = EnemyHp;

        canvasTransform = GameObject.Find("EnemyHPCanvas").transform;

        GameObject newHpBar = Instantiate(hpBarPrefab, canvasTransform);
        hpBarImage = newHpBar.GetComponent<Image>();

        hpBarImage.sprite = hpSprites[(int)currentHP];

        if (Random.Range(0f, 1f) < 0.5f) Flip();

        // 시작할때 NameTag 붙히기
        Managers.UI.ShowPopUpUI_handleTarget<NameTag>(this.gameObject);
    }

    void FixedUpdate()
    {
        if (IsEnemyDead || !canMove) return;

        if (currentState == EnemyState.Following)
        {
            FollowPlayer();
        }
    }

    void Update()
    {
        if (IsEnemyDead) return;

        
        float dx = Mathf.Abs(transform.position.x - player.transform.position.x);
        float dy = Mathf.Abs(transform.position.y - player.transform.position.y);

        if (isHitOverride)
            currentState = EnemyState.Following;
        else if (dx <= attackDistance && dy <= followDistanceY)
            currentState = isMale ? EnemyState.Attacking : EnemyState.Throwing;
        else if (dx <= throwDistance && dy <= followDistanceY)
            currentState = isMale ? EnemyState.Following : EnemyState.Throwing;
        else if (dx <= followDistance && dy <= followDistanceY)
            currentState = EnemyState.Following;
        else
            currentState = EnemyState.Idle;

        
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
            case EnemyState.Idle:
                attackObject.SetActive(false);
                if (!isWandering)
                {
                    isWandering = true;
                }
                Wander(); break;
        }

        // HP 슬라이더 위치 업데이트
        if (hpBarImage != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.7f, 0));
            hpBarImage.transform.position = screenPosition;
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

        float rand = Random.Range(0, 100f);
        if (rand < 0.5f) Flip();

        // 위치 업데이트
        transform.position = new Vector2(transform.position.x +
            (speed * Time.deltaTime * facingRightSign), transform.position.y);

    }

    void FollowPlayer()
    {
        animator.SetBool("enemy_attack", false);
        animator.SetBool("enemy_throw", false);
        
        Vector2 direction = (player.transform.position - transform.position).normalized;
        FlipDirection(direction.x);

        transform.position = new Vector2(transform.position.x +
            (speed * Time.deltaTime * facingRightSign), transform.position.y);

        SpeechPopUp(); // 대사출력
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
    public void EnemyDamage(int damage, int attackMethod)
    {
        animator.SetBool("enemy_attacked", true);
        canMove = false;
        
        if(!IsEnemyDead) {
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
            currentHP -=damage;
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

    IEnumerator IsAttacked(float knockback) {
        isHitOverride = true;

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


        yield return new WaitForSeconds(0.7f);

        isHitOverride = false;
        
    }

    IEnumerator JumpAttacked() {
        isHitOverride = true;

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

        
        yield return new WaitForSeconds(0.7f);
        isHitOverride = false;
    }

    public void CanMoveAtt() {
        canMove = true;
        animator.SetBool("enemy_attacked", false);
        //currentState = EnemyState.Following;
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        IsEnemyDead = true;

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
