using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    // HP
    public float hp = 100f;
    public Slider hpSlider;
    public GameObject hpSliderPrefab; // Slider 프리팹 연결


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

    public float followDistance = 6.0f; // 따라가기 시작하는 거리
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

        if (hpSliderPrefab != null)
        {
            GameObject sliderInstance = Instantiate(hpSliderPrefab, GameObject.Find("EnemyHPCanvas").transform);
            hpSlider = sliderInstance.GetComponent<Slider>();

            hpSlider.maxValue = hp;
            hpSlider.value = hp;
        }
    }


    protected virtual void Update()
    {
        // HP 슬라이더 위치 업데이트
        if (hpSlider != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3.4f, 0));
            hpSlider.transform.position = screenPosition;
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

        
        if (hpSlider != null)
        {
            Destroy(hpSlider.gameObject);
        }

        Destroy(gameObject);
    }
}
