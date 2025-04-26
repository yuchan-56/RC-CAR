using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GrpBoss : Boss
{
    [SerializeField] private GameObject framePrefab; // Inspector에 연결
    private GameObject frameInstance;
    bool showFrame = false;


    //attack
    public GameObject bulletPrefab; // 총알 프리팹
    public float bulletSpeed = 13f; // 총알 속도
    public float fireRate = 1.0f;

    Vector3 firePoint = new Vector3(0, 2.0f, 0);

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
        new Vector3(-5f, 10f, 0),  
        new Vector3(0, 10f, 0),   
        new Vector3(5f, 10f, 0)
    };
    private List<GameObject> spawnedObjects = new List<GameObject>();

    bool p2Pos = true;

    //p2
    private Rigidbody2D rb;
    private Vector3 initialFlyingPos;

    //p3
    public GameObject p3Object;
    public GameObject p3Collider;

    public BossManager bmScript;


    protected override void Start()
    {
        base.Start();
        Debug.Log("그래픽 보스");

        rb = GetComponent<Rigidbody2D>();
        initialFlyingPos = transform.position;
    }


    protected override void Update()
    {
        base.Update();
        if(showHP && !showFrame) {
            ShowFrame();
        }

        if (isDead)
        {
            Destroy(frameInstance.gameObject);
        }
    }

    private void ShowFrame()
    {
        if (hpBarTransform == null) return;

        // 프레임 생성 및 HP 바에 붙이기
        frameInstance = Instantiate(framePrefab, hpBarTransform.parent);
        RectTransform frameRect = frameInstance.GetComponent<RectTransform>();

        // 프레임 위치 설정 (HP 바 기준 상대 위치 조정)
        frameRect.pivot = new Vector2(0f, 0.8f);
        frameRect.anchoredPosition = hpBarTransform.anchoredPosition;
        frameRect.sizeDelta = hpBarTransform.sizeDelta + new Vector2(0, 10);

        showFrame = true;
    }
   

    public override void Attack() {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;

        animator.SetBool("isAttack", true);
        animator.SetBool("isStop", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isDead", false);
        animator.SetBool("isP1", false);

        StartCoroutine(ShootBullets(3));
    }

    

    IEnumerator ShootBullets(int shotCount)
    {
        for (int i = 0; i < shotCount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }

        animator.SetBool("isAttack", false);
        isWandering = true;
        isFollowing = true;
        isStop = true;
        bmScript.attackPos = true;
        Debug.Log("attackPos = true");
    }

    void Shoot()
    {
        if (bulletPrefab == null || player == null) return; // 플레이어가 없으면 실행 X

        GameObject bullet = Instantiate(bulletPrefab, transform.position + firePoint, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            Vector2 direction = (player.position - transform.position).normalized; // 방향 벡터 계산
            bulletRb.velocity = direction * bulletSpeed; // 방향 적용
        }
    }



    public override void P1() {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isDead", false);
        animator.SetBool("isStop", false);
        animator.SetBool("isP1", true);

        
        StartCoroutine(SpawnRotateAndFallObjects());
    }

    IEnumerator SpawnRotateAndFallObjects()
    {
        for (int i = 0; i < p1Object.Length; i++)
        {
            if (p1Object[i] != null)
            {
                
                Vector3 spawnPosition = transform.position + spawnPositions[i];
                GameObject newObj = Instantiate(p1Object[i], spawnPosition, Quaternion.identity);

                spawnedObjects.Add(newObj);
                StartCoroutine(RotateObject(newObj));
            }
        }

        // 일정 시간 대기 후 낙하 시작
        yield return new WaitForSeconds(fallDelay);

        for(int i = 0; i < spawnedObjects.Count; i++)
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

        obj.transform.position = player.transform.position + fallStartPos;
        obj.transform.localScale *= scaleMultiplier;

        while (obj.transform.position.y > transform.position.y - 4f) // 땅까지 떨어질 때까지
        {
            obj.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(obj); // 바닥에 닿으면 삭제

        spawnedObjects.Clear();
        animator.SetBool("isP1", false);
        
        isWandering = true;
        isFollowing = true;
        isStop = true;
        bmScript.attackPos = true;
    }


    public override void P2() {
        if((currentHP <= maxHP * 0.5f) && p2Pos) {
            isWandering = false;
            isFollowing = false;
            isStop = false;
            bmScript.attackPos = false;
            p2Pos = false;

            // 피회복
            animator.SetBool("isAttack", false);
            animator.SetBool("isP1", false);
            animator.SetBool("isP3", false);
            animator.SetBool("isDead", false);
            animator.SetBool("isStop", false);
            animator.SetBool("isP2", true);

            StartCoroutine(DescendAndHeal());
            //StartCoroutine(GetHP());
        }
    }

    IEnumerator DescendAndHeal()
    {
        // ★ 1. Gravity를 켜서 땅으로 내려오게 만들자
        rb.gravityScale = 3f; // 중력 세기 (조절 가능)

        // ★ 2. 땅에 닿을 때까지 기다린다 (간단하게 y축 위치로 체크)
        while (transform.position.y > 0.5f) // 바닥 높이가 0쯤이라고 가정
        {
            yield return null;
        }

        // ★ 3. 땅에 닿으면 Gravity를 끄고 위치 고정
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z); // 정확히 바닥 위치 고정

        // ★ 4. 이제 피 회복 시작
        yield return StartCoroutine(GetHP());

        // ★ 5. 회복 끝나면 다시 하늘로 올라가자
        yield return StartCoroutine(AscendToSky());
    }

    /// /////////////////////////////////
    IEnumerator GetHP() {
        float targetHP = maxHP * 0.65f; // 회복 목표는 70%
        float healSpeed = maxHP * 0.01f;
        Vector3 currentPos = transform.position;
        // 현재 Gravity 저장

        while (currentHP < targetHP)
        {
            currentHP += healSpeed;
            currentHP = Mathf.Min(currentHP, targetHP); // 넘치지 않게
            UpdateHPBar(); // HP 바 실시간 갱신
            yield return new WaitForSeconds(0.1f); // 회복 간격
        }

        // 회복 종료 후 애니메이션 리셋
        animator.SetBool("isP2", false);
        isWandering = true;
        isFollowing = true;
        isStop = true;
        bmScript.attackPos = true;
        // transform.position = currentPos; 자연스럽게
        //gravity, position 다시 리셋
    }

    IEnumerator AscendToSky()
    {
        Vector3 targetPos = initialFlyingPos + new Vector3(0f, 3f, 0f); // 원하는 높이까지 상승
        float ascendSpeed = 3f;

        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, ascendSpeed * Time.deltaTime);
            yield return null;
        }

        // 다 올라가면
        transform.position = targetPos;
    }




    public override void P3() {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", true);
        animator.SetBool("isDead", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isStop", false);

        StartCoroutine(Delay(0.4f));
        StartCoroutine(ShootBeam());
    }

    IEnumerator Delay(float sec) {
        yield return new WaitForSeconds(sec);
        
        p3Object.SetActive(true);

        StartCoroutine(DelayForCollider(1.2f));
    }

    IEnumerator DelayForCollider(float sec) {
        yield return new WaitForSeconds(sec);

        p3Collider.SetActive(true);
    }


    IEnumerator ShootBeam() {
        yield return new WaitForSeconds(2.4f);

        animator.SetBool("isP3", false);
        p3Object.SetActive(false);
        p3Collider.SetActive(false);

        bmScript.attackPos = true;
        isWandering = true;
        isFollowing = true;
        isStop = true;
    }
}
