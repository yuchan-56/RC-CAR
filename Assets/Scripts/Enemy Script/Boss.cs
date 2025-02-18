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

    //public float followDistance = 10f; // 따라가기 시작하는 거리
    //public float throwDistance = 8f; // 오브젝트 던지기 시작하는 거리
    //public float attackDistance = 2.0f; // 일반 공격 거리
    private Vector2 stopPosition;

    // 방향전환
    private bool facingRight = false; // 적의 현재 바라보는 방향
    private float lastXPosition = 0f;



    public virtual void Attack() { Debug.Log("Boss 기본 Attack() 실행"); }
    public virtual void P1() { Debug.Log("Boss 기본 P1() 실행"); }
    public virtual void P2() { Debug.Log("Boss 기본 P2() 실행"); }
    public virtual void P3() { Debug.Log("Boss 기본 P3() 실행"); }


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

        if(isWandering) {
            Wander();
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            BossDamage(10);
        }
    }

    

    protected virtual void Wander()
    {
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isAttack", false);


        
        float xPos = Mathf.PingPong(Time.time * speed, wanderDistance) - (wanderDistance / 2);

        float horizontalDirection = xPos - lastXPosition;
        FlipDirection(horizontalDirection);

        transform.position = new Vector2(stopPosition.x + xPos, transform.position.y);

        lastXPosition = xPos;
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
        StartCoroutine(DieCoroutine());   
    }

    private IEnumerator DieCoroutine()
    {
        Debug.Log("boss died!");
        isDead = true;
        animator.SetBool("isDead", true);

        yield return new WaitForSeconds(4.5f);

        
        if (hpSlider != null)
        {
            Destroy(hpSlider.gameObject);
        }

        Destroy(gameObject);
    }
}
