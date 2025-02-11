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

    bool isDead = false;


    //attack
    public GameObject attackObject;


    //p1
    public GameObject p1Object;


    //P2
    public GameObject[] printW = new GameObject[12];


    //p3
    public GameObject p3Object;


    void Start()
    {
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
        // HP 슬라이더 위치 업데이트
        if (hpSlider != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3.4f, 0));
            hpSlider.transform.position = screenPosition;
        }

        // 공격 test
        if(Input.GetKeyDown(KeyCode.U)) {
            Attack();
            Debug.Log("Attack");
        }

        if(Input.GetKeyDown(KeyCode.I)) {
            P1();
            Debug.Log("P1");
        }

        if(Input.GetKeyDown(KeyCode.O)) {
            P2();
            Debug.Log("P2");
        }

        if(Input.GetKeyDown(KeyCode.P)) {
            P3();
            Debug.Log("P3");
        }


        // 확인용
        if(Input.GetKeyDown(KeyCode.Space)) {
            BossDamage(10);
        }
    }

    void Attack() {
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isAttack", true);

        if (attackObject != null)
        {
            attackObject.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(attackObject, 3f));
        }
    }

    void P1() {
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", true);

        if (p1Object != null)
        {
            p1Object.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(p1Object, 3f));
        }
    }


    //------

    void P2() {
        animator.SetBool("isAttack", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", true);

        foreach (GameObject obj in printW)
        {
            if (obj != null)
            {
                StartCoroutine(FallObject(obj));
            }
        }
    }

    

   IEnumerator FallObject(GameObject obj)
    {
        // 랜덤 X 좌표 설정 (보스 기준으로 좌우 범위 지정)
        float randomX = UnityEngine.Random.Range(transform.position.x - 8f, transform.position.x + 8f);

        // 초기 위치 설정 (보스 머리 위에서 떨어지도록)
        Vector3 startPosition = new Vector3(randomX, transform.position.y + 7f, 0);
        GameObject fallingObj = Instantiate(obj, startPosition, Quaternion.identity);

        float fallSpeed = UnityEngine.Random.Range(2f, 5f);

        while (fallingObj.transform.position.y > transform.position.y - 5f)
        {
            fallingObj.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(fallingObj);
        animator.SetBool("isP2", false);
    }


    // ------

    void P3() {
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP3", true);
    
        if (p3Object != null)
        {
            p3Object.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(p3Object, 4f));
        }
    }

    IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        animator.SetBool("isP3", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isAttack", false);

    }


    public void BossDamage(float damage)
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
        Debug.Log("boss died!");
        isDead = true;
        animator.SetBool("isDead", true);

        yield return new WaitForSeconds(1.4f); // 애니메이션 길이만큼 대기

        
        if (hpSlider != null)
        {
            Destroy(hpSlider.gameObject);
        }

        Destroy(gameObject);
    }
}
