using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    Collider2D playerCollider;
    Collider2D bossCollider;
    // HP
    public GameObject hpBarPrefab; // HP Bar 프리팹 (UI Image)
    public RectTransform hpBarTransform; // 개별 HP 바의 RectTransform
    protected Transform canvasTransform; // UI Canvas

    public float maxHP = 100f;
    protected float currentHP;
    protected float initialWidth;
    protected bool showHP = false;

    protected bool sysP1 = false;


    // animator
    public Animator animator;
    public bool isDead = false;

    // 좌우이동 + following player
    public Transform player;
    public float speed = 1.4f;

    // 거리제한
    public float wanderDistance = 2f;
    public bool isWandering = true;
    public bool isFollowing = true;
    public bool isStop = true;

    public float followDistance = 20.0f; // 따라가기 시작하는 거리
    public float minFollowDistance = 2.5f;


    
    private Vector2 stopPosition;

    // 방향전환
    private bool facingRight = false; // 적의 현재 바라보는 방향
    private float lastXPosition = 0f;

    protected enum BossState {
        Idle,
        Following,
        Stopping
    }

    protected BossState currentState = BossState.Idle;



    public virtual void Attack() { }
    public virtual void P1() { }
    public virtual void P2() { }
    public virtual void P3() { }



    protected virtual void Start()
    {
        stopPosition = transform.position;

        currentHP = maxHP;

        canvasTransform = GameObject.Find("EnemyHPCanvas").transform; 

        playerCollider = player.GetComponent<Collider2D>();
        bossCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(playerCollider, bossCollider);
    }


    private  void ShowAnimation()
    {
        Managers.UI.ShowPopUpUI<BossJoin>();
        BossJoin join = FindObjectOfType<BossJoin>();
        join.setBossImage(this.gameObject.name);
        Debug.Log(this.gameObject.name);
    }
    
    protected virtual void Update()
    {
        float heightWPlayer = Mathf.Abs(transform.position.y - player.position.y);
        if(heightWPlayer <= 10.0f && !showHP) {
            StartCoroutine(ShowHPBar());
            ShowAnimation(); // 보스가 Player 조우시 애니메이션 출력
            showHP = true;
        }


        float distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x);


        if (distanceToPlayer <= followDistance && distanceToPlayer > minFollowDistance) 
        {
            currentState = BossState.Following;
        } 
        else if (distanceToPlayer <= minFollowDistance) 
        {
            currentState = BossState.Stopping;
        } 
        else 
        {
            currentState = BossState.Idle;
        }

        switch (currentState) 
        {
            case BossState.Following:
                if(isFollowing) {
                    if (AnimatorHasParameter("isStop")) animator.SetBool("isStop", false);
                    FollowPlayer();
                }
                break;
            case BossState.Stopping:
                if(isStop) { StopMoving(); }
                break;
            case BossState.Idle:
                if(isWandering)  {
                    if (AnimatorHasParameter("isStop")) animator.SetBool("isStop", false);
                    Wander();
                }
                break;
                
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            BossDamage(10);
        }
    }

    IEnumerator ShowHPBar() {
        // 개별 HP 바 생성 및 Canvas의 자식으로 설정
        GameObject newHpBar = Instantiate(hpBarPrefab, canvasTransform);
        hpBarTransform = newHpBar.GetComponent<RectTransform>();
        hpBarTransform.anchoredPosition = new Vector2(-530.0f, canvasTransform.GetComponent<RectTransform>().sizeDelta.y / 2 - 50);

        initialWidth = hpBarTransform.sizeDelta.x; // 원래 체력바 길이 저장

        hpBarTransform.pivot = new Vector2(0f, 0.5f);

        hpBarTransform.sizeDelta = new Vector2(initialWidth, hpBarTransform.sizeDelta.y);

        yield return null;
    }

    public void UpdateHPBar()
    {
        float hpRatio = currentHP / maxHP;
        hpBarTransform.sizeDelta = new Vector2(initialWidth * hpRatio, hpBarTransform.sizeDelta.y); // 체력 비율만큼 너비 조정
    }

    protected virtual void StopMoving()
    {
        if (AnimatorHasParameter("isStop")) 
        {
            animator.SetBool("isStop", true);
        }
    }
    private bool AnimatorHasParameter(string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
            {
                return true;
            }
        }
        return false;
    }
    

    protected virtual void Wander()
    {
        animator.SetBool("isP1", false);
        //animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isAttack", false);

        float xPos = Mathf.PingPong(Time.time * speed, wanderDistance) - (wanderDistance / 2);

        float horizontalDirection = xPos - lastXPosition;
        FlipDirection(horizontalDirection);

        transform.position = new Vector2(stopPosition.x + xPos, transform.position.y);

        lastXPosition = xPos;
    }

    protected virtual void FollowPlayer()
    {
        animator.SetBool("isP1", false);
        //animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isAttack", false);

        Vector2 directionToPlayer = player.position - transform.position;
        FlipDirection(directionToPlayer.x);
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        stopPosition = transform.position; 
    }

    protected void FlipDirection(float horizontalDirection)
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

    protected void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }


    public virtual void BossDamage(float damage)
    {
        if(!sysP1) {
            if(currentHP > 0) {
                currentHP -= damage;
                UpdateHPBar();
            }
            else if (currentHP < 0) { currentHP = 0; }

            Managers.Game.GetHit = true;

            if (currentHP <= 0)
            {
                Die();
            }
        }
    }

    public virtual void Die()
    {
        isWandering = false;
        isFollowing = false;
        StartCoroutine(DieCoroutine());   
    }

    private IEnumerator DieCoroutine()
    {
        Debug.Log("boss died!");
        isDead = true;
        animator.SetBool("isDead", true);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length; // 애니메이션 길이 가져오기

        yield return new WaitForSeconds(animationLength * 2.02f);

        Destroy(hpBarPrefab.gameObject);
        Destroy(gameObject);
    }
}
