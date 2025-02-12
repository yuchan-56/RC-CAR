using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrpBoss : MonoBehaviour
{
    // HP
    public float hp = 100f;
    public Slider hpSlider;
    public GameObject hpSliderPrefab; // Slider 프리팹 연결


    // animator
    public Animator animator;
    bool isDead = false;

    //p1
    public GameObject[] p1Object = new GameObject[3];
    public float rotationSpeed = 500f; // 회전 속도
    public float fallDelay = 2f; // 회전 후 낙하하는 시간
    public float fallStartY = 10f;
    public Vector3[] spawnPositions = new Vector3[3]
    {
        new Vector3(-2f, 3f, 0),  // 왼쪽 위
        new Vector3(0f, 4.5f, 0),   // 정 중앙 위
        new Vector3(2f, 3f, 0)    // 오른쪽 위
    };

    public Vector3[] fallStartPositions = new Vector3[3]
    {
        new Vector3(-7f, 10f, 0),  // 왼쪽에서 낙하
        new Vector3(-5f, 10f, 0),   // 중앙에서 낙하
        new Vector3(-3f, 10f, 0)    // 오른쪽에서 낙하
    };
    private List<GameObject> spawnedObjects = new List<GameObject>();


    //p3
    public GameObject p3Object;
   



    // Start is called before the first frame update
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

    // Update is called once per frame
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
        animator.SetBool("isAttack", true);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isDead", false);
        animator.SetBool("isP1", false);
    }

    void P1() {
        // 돌다가
        // 하늘에서 떨어짐
        StartCoroutine(SpawnRotateAndFallObjects());

        animator.SetBool("isAttack", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isDead", false);
        animator.SetBool("isP1", true);
    }

    IEnumerator SpawnRotateAndFallObjects()
    {
        for (int i = 0; i < p1Object.Length; i++)
        {
            if (p1Object[i] != null)
            {
                // 보스를 기준으로 지정된 위치에 오브젝트 생성
                Vector3 spawnPosition = transform.position + spawnPositions[i];
                GameObject newObj = Instantiate(p1Object[i], spawnPosition, Quaternion.identity);

                spawnedObjects.Add(newObj);
                StartCoroutine(RotateObject(newObj));
            }
        }

        // 일정 시간 대기 후 낙하 시작
        yield return new WaitForSeconds(fallDelay);

        for(int i = 0; i < 3; i++)
        {
            if (spawnedObjects[i] != null)
            {
                StartCoroutine(FallObject(spawnedObjects[i], fallStartPositions[i]));
            }
        }
    }
    IEnumerator RotateObject(GameObject obj)
    {
        float rotationTime = fallDelay;
        float elapsedTime = 0f;

        while (elapsedTime < rotationTime)
        {
            obj.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FallObject(GameObject obj, Vector3 fallStartPos)
    {
        float fallSpeed = 10f;
        float scaleMultiplier = 1.5f;

        obj.transform.position = transform.position + fallStartPos;
        obj.transform.localScale *= scaleMultiplier;

        while (obj.transform.position.y > transform.position.y - 5f) // 땅까지 떨어질 때까지
        {
            obj.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(obj); // 바닥에 닿으면 삭제
    }






    void P2() {
        // 피회복
        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isDead", false);

        animator.SetBool("isP2", true);

        hp += 30;

        if(hp > 100) {
            hp = 100;
        }
        else if (hpSlider != null) {
            hpSlider.value = hp;
        }

        // 초당회복?
    }



    void P3() {
        animator.SetBool("isAttack", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", true);
        animator.SetBool("isDead", false);
        animator.SetBool("isP1", false);

        StartCoroutine(ShootBeam());
    }

    IEnumerator ShootBeam() {

        yield return null;
    }




    // dying
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

        yield return new WaitForSeconds(3.5f); // 애니메이션 길이만큼 대기

        
        if (hpSlider != null)
        {
            Destroy(hpSlider.gameObject);
        }

        Destroy(gameObject);
    }
}
