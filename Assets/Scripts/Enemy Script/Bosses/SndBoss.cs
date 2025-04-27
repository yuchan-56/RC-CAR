using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SndBoss : Boss
{
    public BossManager bmScript;

    [SerializeField] private GameObject framePrefab; // Inspector에 연결
    private GameObject frameInstance;
    bool showFrame = false;


    //attack
    public GameObject attackPrefab; // 총알 프리팹
    public float bulletSpeed = 15f; // 총알 속도
    public float fireRate = 1.5f;
    Vector3 firePoint = new Vector3(0, 2.0f, 0);


    //p1
    public GameObject P1Object;

    //p2
    public float p2SpeedMultiplier = 1.7f;
    public GameObject p2Prefab;
    public float p2PhaseDuration = 2f;
    public float p2Duration = 9.0f;

    //p3
    public GameObject P3Object;


    protected override void Start()
    {
        base.Start();
        
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

        animator.SetBool("isStop", false);
        animator.SetBool("isAttack", true);
    }

    public void SndAttRoutine()
    {
        if (attackPrefab != null)
        {
            Vector2 dir = facingRight ? Vector2.right : Vector2.left;
            var wave = Instantiate(attackPrefab, transform.position + firePoint, Quaternion.identity);

            if (wave.TryGetComponent<Rigidbody2D>(out var rb2)) rb2.velocity = dir * bulletSpeed;

            if (rb2 != null) rb2.velocity = dir * bulletSpeed;
            wave.transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);

            Destroy(wave, 1.5f);
        }

        StartCoroutine(EndPattern(0, 3.0f));
    }

    // 콜라이더
    public override void P1()
    {
        isAttacking = true;
        bmScript.attackPos = false;
        animator.SetBool("isStop", false);
        animator.SetBool("isP1", true);
    }

    public void SndP1Routine()
    {
        P1Object.SetActive(true);
        StartCoroutine(EndPattern(1, 1.2f));
    }


    //싱크 안맞음, 마지막에 공격만하고 루프걸림
    public override void P2()
    {
        isAttacking = true;
        bmScript.attackPos = false;

        animator.SetBool("isStop", false);
        

        StartCoroutine(P2Routine(1.0f));
    }

    private IEnumerator P2Routine(float f)
    {
        animator.SetBool("isP2", true);
        yield return new WaitForSeconds(f + 1.2f);

        float originalSpeed = speed;

        float endTime = Time.time + p2Duration;
        float phaseTime = p2PhaseDuration;
        

        while (Time.time < endTime)
        {
            animator.SetBool("isP2Walk", true);
            animator.SetBool("isP2Attack", false);

            speed = originalSpeed * p2SpeedMultiplier;

            float phaseEnd = Time.time + p2PhaseDuration;
            while (Time.time < phaseEnd && Time.time < endTime)
            {
                FollowPlayer();
                yield return null;
            }

            // --- phase 2: attack with ultimate attack anim ---
            animator.SetBool("isP2Walk", false);
            animator.SetBool("isP2Attack", true);
            speed = originalSpeed;

            phaseEnd = Time.time + p2PhaseDuration;
            while (Time.time < phaseEnd && Time.time < endTime)
            {
                yield return new WaitForSeconds(fireRate);
            }
            animator.SetBool("isP2Attack", false);
        }

        animator.SetBool("isP2Walk", false);
        animator.SetBool("isP2Attack", false);
        animator.SetBool("isP2", false) ;
        speed = originalSpeed;
        StartCoroutine(EndPattern(2, 0));
    }

    public void SndP2AttRoutine()
    {
        if (attackPrefab != null)
        {
            Vector2 dir = facingRight ? Vector2.right : Vector2.left;
            var wave = Instantiate(p2Prefab, transform.position + firePoint, Quaternion.identity);

            if (wave.TryGetComponent<Rigidbody2D>(out var rb2)) rb2.velocity = dir * bulletSpeed;

            if (rb2 != null) rb2.velocity = dir * bulletSpeed;
            wave.transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);

            Destroy(wave, 1.5f);
        }
    }


    public override void P3()
    {
        isAttacking = true;
        bmScript.attackPos = false;

        animator.SetBool("isP3", true);
        animator.SetBool("isStop", false);
    }

    public void SndP3Routine()
    {
        P3Object.SetActive(true);
        StartCoroutine(EndPattern(3, 2.25f));
    }

    IEnumerator EndPattern(int attackNum, float min)
    {
        yield return new WaitForSeconds(min);

        isWandering = isFollowing = isStop = true;
        isAttacking = false;
        bmScript.attackPos = true;

        switch(attackNum)
        {
            case 0:
                animator.SetBool("isAttack", false);
                break;
            case 1:
                P1Object?.SetActive(false);
                animator.SetBool("isP1", false);
                break;
            case 2:
                animator.SetBool("isP2Walk", false);
                animator.SetBool("isP2Attack", false);
                animator.SetBool("isP2", false);
                break;
            case 3:
                P3Object?.SetActive(false);
                animator.SetBool("isP3", false);
                break;
            default:
                break;

        }

       
    }

    public override void Die()
    {
        base.Die();
        if (frameInstance != null)
            Destroy(frameInstance);
    }
}
