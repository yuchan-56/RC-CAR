using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SndBoss : Boss
{
    [SerializeField] private GameObject framePrefab; // Inspector에 연결

    private GameObject frameInstance;
    bool showFrame = false;

    public BossManager bmScript;


    //attack
    public GameObject attackPrefab; // 총알 프리팹
    public float bulletSpeed = 9f; // 총알 속도
    //public float fireRate = 1.0f;

    Vector3 firePoint = new Vector3(0, 2.0f, 0);


    //p1
    public GameObject P1Object;
    bool didP1 = false;

    //p2
    
    public GameObject p2Prefab;
    public float p2Duration = 14.0f;

    //p3
    public GameObject P3Object;
    public float p3SpeedMultiplier = 1.7f;
    public float p3Duration = 10.0f;
    bool didP3 = false;


    protected override void Start()
    {
        base.Start();
        Debug.Log("sound boss 등장!");
    }

    protected override void Update()
    {
        base.Update();

        if(showHP && !showFrame) {
            ShowFrame();
        }

        if(isDead) {
            Destroy(frameInstance);
            frameInstance = null;
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
        //isWandering = false;
        //isFollowing = false;
        //isStop = false;
        isAttacking = true;
        bmScript.attackPos = false;


        animator.SetBool("isAttack", true);
        //animator.SetBool("isP1", false);
        //animator.SetBool("isP2", false);
        //animator.SetBool("isP3", false);
        //animator.SetBool("isStop", false);

        StartCoroutine(HoldOn(3.0f));
    }

    public void SndAttRoutine()
    {
        if (attackPrefab != null)
        {
            Vector2 dir = facingRight ? Vector2.right : Vector2.left;

            var wave = Instantiate(attackPrefab, transform.position + firePoint, transform.rotation);

            var rb2 = wave.GetComponent<Rigidbody2D>();

            if (rb2 != null) rb2.velocity = dir * bulletSpeed;

            Destroy(wave, 1.5f);
        }
    }

    IEnumerator HoldOn(float f)
    {
        yield return new WaitForSeconds(f);

        animator.SetBool("isAttack", false);
        //p1
        animator.SetBool("isP1", false);
        didP1 = false;

        //p3
        animator.SetBool("isP3", false);
        didP3 = false;

        isWandering = isFollowing = isStop = true;
        isAttacking = false;
        bmScript.attackPos = true;
    }

    // 콜라이더
    public override void P1()
    {
        isAttacking = true;
        bmScript.attackPos = false;
        animator.SetBool("isP1", true);
    }

    public void SndP1Routine()
    {
        if (!didP1)
        {
            P1Object.SetActive(true);
            didP1 = true;
        }
    }

    public void SndP1RoutineOff()
    {
        StartCoroutine(HoldOn(0.1f));
        P1Object.SetActive(false);
    }


    //following, 공제대로 던지셈
    public override void P2()
    {
        isAttacking = true;
        bmScript.attackPos = false;
        animator.SetBool("isP2", true);

        StartCoroutine(P2Routine());
    }

    private IEnumerator P2Routine()
    {
        // 속도 강화
        float originalSpeed = speed;
        speed *= p3SpeedMultiplier;

        float rest = p2Duration;

        while(rest > 0)
        {
            float elapsed = 0f;
            while (elapsed < 2f && rest > 0f)
            {
                float dt = Time.deltaTime;
                rest -= dt;
                elapsed += dt;
                yield return null;
            }
            animator.SetBool("isAttack", true);

            elapsed = 0f;
            while (elapsed < 2.5f && rest > 0f)
            {
                float dt = Time.deltaTime;
                rest -= dt;
                elapsed += dt;
                yield return null;
            }
            animator.SetBool("isAttack", false);
        }
        animator.SetBool("isAttack", false);

        // 리셋
        speed = originalSpeed;
        animator.SetBool("isP2", false);
        isWandering = isFollowing = isStop = true;
        isAttacking = false;
        bmScript.attackPos = true;
    }

    public void SndP2AttRoutine()
    {
        if (p2Prefab != null)
        {
            float dirX = transform.localScale.x > 0 ? 1f : -1f;
            Vector2 dir = new Vector2(dirX, 0f);

            var wave = Instantiate(p2Prefab, transform.position + firePoint, Quaternion.identity);

            var rb2 = wave.GetComponent<Rigidbody2D>();

            if (rb2 != null) rb2.velocity = dir * bulletSpeed;

            Destroy(wave, 1.5f);
        }
    }


    public override void P3()
    {
        isAttacking = true;
        bmScript.attackPos = false;

        animator.SetBool("isP3", true);
    }

    public void SndP3Routine()
    {
        if (!didP3)
        {
            P3Object.SetActive(true);
            didP3 = true;
        }
    }

    public void SndP3RoutineOff()
    {
        StartCoroutine(HoldOn(0.1f));
        P3Object.SetActive(false);
    }
}
