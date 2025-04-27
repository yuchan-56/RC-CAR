using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrpBoss : Boss
{
    public BossManager bmScript;

    [SerializeField] private GameObject framePrefab; // Inspector에 연결
    private GameObject frameInstance;
    bool showFrame = false;


    //attack
    public GameObject bulletPrefab; // 총알 프리팹
    public float bulletSpeed = 13f; // 총알 속도
    public float fireRate = 1.0f;
    private Vector3 fireOffset = new Vector3(0, 2f, 0);

    //p1
    public GameObject[] p1Object = new GameObject[3];
    public float rotationSpeed = 5f;
    public float orbitRadius = 2f;
    public float orbitTime = 2f;
    public float dropSpeed = 10f;
    private List<GameObject> _orbiters = new List<GameObject>();

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

  

    //p2
    public float healThreshold = 0.5f;
    public float healTarget = 0.65f;       // 목표 비율
    public float healRate = 0.01f;         // 1회당 회복 비율
    private bool _p2Done = false;
    public float fallSpeed = 6f;
    public float groundY = -1.11f;
    bool hadLanded = false;
    //private bool p2Started = false;

    //p3
    public GameObject p3Object;
    public GameObject p3Collider;
    public float beamDelay = 0.4f;
    public float beamDuration = 2.0f;

    

    private Rigidbody2D rb;


    protected override void Start()
    {
        base.Start();
        Debug.Log("그래픽 보스");

        rb = GetComponent<Rigidbody2D>();
    }


    protected override void Update()
    {
        base.Update();

        if(showHP && !showFrame) {
            ShowFrame();
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
        isAttacking = true;
        bmScript.attackPos = false;

        animator.SetBool("isAttack", true);

        StartCoroutine(AttackRoutine());
    }

    

    IEnumerator AttackRoutine()
    {
        Vector2 dir = facingRight ? Vector2.right : Vector2.left;

        for (int i = 0; i < 3; i++)
        {
            // 보스가 바라보는 방향(transform.right)으로 발사
            if (bulletPrefab != null)
            {
                var b = Instantiate(bulletPrefab, transform.position + fireOffset, transform.rotation);
                var rb2 = b.GetComponent<Rigidbody2D>();
                if (rb2 != null)
                    rb2.velocity = dir * bulletSpeed;
            }
            yield return new WaitForSeconds(fireRate);
        }

        animator.SetBool("isAttack", false);
        isWandering = isFollowing = isStop = true;
        bmScript.attackPos = true;
        isAttacking = false;
    }



    public override void P1() {
        isAttacking = true;
        bmScript.attackPos = false;

        animator.SetBool("isP1", true);

        StartCoroutine(P1Routine());
    }

    IEnumerator P1Routine()
    {
        _orbiters.Clear();

        for (int i = 0; i < p1Object.Length; i++)
        {
            if (p1Object[i] == null) continue;

            Vector3 spawnPos = transform.position + spawnPositions[i];
            var o = Instantiate(p1Object[i], spawnPos, Quaternion.identity);

            _orbiters.Add(o);
        }

        float t = 0f;
        while (t < orbitTime)
        {
            t += Time.deltaTime;
            foreach (var o in _orbiters)
            {
                if (o != null)
                    o.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            }
            yield return null;
        }

        for (int i = 0; i < _orbiters.Count; i++)
        {
            var o = _orbiters[i];
            if (o != null)
            {
                o.transform.localScale *= 1.4f;
                o.transform.position = player.transform.position + fallStartPositions[i];
            }
        }

        // 3) 낙하
        float bottomY = transform.position.y - 1f;
        while (_orbiters.Count > 0)
        {
            for (int i = _orbiters.Count - 1; i >= 0; i--)
            {
                var o = _orbiters[i];
                if (o == null)
                {
                    _orbiters.RemoveAt(i);
                    continue;
                }
                o.transform.position += Vector3.down * dropSpeed * Time.deltaTime;
                if (o.transform.position.y <= bottomY)
                {
                    Destroy(o);
                    _orbiters.RemoveAt(i);
                }
            }
            yield return null;
        }

        // 4) 패턴 종료 리셋
        animator.SetBool("isP1", false);
        isWandering = isFollowing = isStop = true;
        bmScript.attackPos = true;
        isAttacking = false;
    }
    


    public override void P2() {
        Debug.Log("P2 호출됨: currentHP=" + currentHP + " _p2Done=" + _p2Done);

        if (_p2Done || currentHP > maxHP * healThreshold)
        {
            isAttacking = false;    
            bmScript.attackPos = true;
            return;
        }

        _p2Done = true;
        isAttacking = true;
        bmScript.attackPos = false;
        animator.SetBool("isP2", true);
        StartCoroutine(P2Routine());
    }

    private IEnumerator P2Routine()
    {
        Vector3 start = transform.position;
        Vector3 target = new Vector3(start.x, groundY, start.z);

        while (transform.position.y > groundY + 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, fallSpeed * Time.deltaTime);
            yield return null;
        }

        // 2) 착지 후 힐
        //rb.gravityScale = 0f;
        //rb.velocity = Vector2.zero;
        transform.position = new Vector3(transform.position.x, groundY, transform.position.z);

        float targetHP = maxHP * healTarget;
        while (currentHP < targetHP)
        {
            currentHP = Mathf.Min(currentHP + maxHP * healRate, targetHP);
            UpdateHPBar();
            yield return new WaitForSeconds(0.1f);
        }

        // 3) 리셋 및 하늘로 복귀
        animator.SetBool("isP2", false);
        isWandering = isFollowing = isStop = true;
        bmScript.attackPos = true;
        isAttacking = false;

        // 4) Ascend
        //Vector3 sky = transform.position + new Vector3(0, 3f, 0);
        while (transform.position.y < start.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, start, Time.deltaTime * 3f);
            yield return null;
        }
    }






    public override void P3() {
        //isWandering = false;
        //isFollowing = false;
        //isStop = false;
        isAttacking = true;
        bmScript.attackPos = false;

        //animator.SetBool("isAttack", false);
        //animator.SetBool("isP2", false);
        animator.SetBool("isP3", true);
        //animator.SetBool("isDead", false);
        //animator.SetBool("isP1", false);
        //animator.SetBool("isStop", false);

        StartCoroutine(P3Routine());
    }

    private IEnumerator P3Routine()
    {
        // 1) 빔 이펙트 & 콜라이더
        p3Object?.SetActive(true);
        yield return new WaitForSeconds(beamDelay);
        p3Collider?.SetActive(true);

        // 2) 지속
        yield return new WaitForSeconds(beamDuration);

        // 3) 리셋
        p3Collider?.SetActive(false);
        p3Object?.SetActive(false);
        animator.SetBool("isP3", false);
        isWandering = isFollowing = isStop = true;
        bmScript.attackPos = true;
        isAttacking = false;
    }

    public override void Die()
    {
        base.Die();
        if (frameInstance != null)
            Destroy(frameInstance);

        // 1) 즉시 중단
        StopAllCoroutines();
        isWandering = false;
        isFollowing = false;
        isStop = false;
        isAttacking = true;    // 더 이상 다른 공격 안 나가게
        bmScript.attackPos = false;

        // 2) 땅으로 자연 하강
        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        Vector3 start = transform.position;
        Vector3 target = new Vector3(start.x, groundY, start.z);
        while (transform.position.y > groundY + 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, fallSpeed * Time.deltaTime);
            yield return null;
        }

        // 3) 땅에 닿으면 완전 삭제
        animator.SetBool("isDead", true);
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Ground"))
            hadLanded = true;
    }
}
