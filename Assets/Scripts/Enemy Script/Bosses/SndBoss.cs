using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class SndBoss : Boss
{
    [SerializeField] private GameObject framePrefab; // Inspector에 연결

    private GameObject frameInstance;
    bool showFrame = false;


    public GameObject attackPrefab; // 총알 프리팹
    public float bulletSpeed = 9f; // 총알 속도
    public float fireRate = 1.0f;

    Vector3 firePoint = new Vector3(0, 2.0f, 0);


    public GameObject P1Object;

    bool isP2 = false;
    public GameObject P2Prefab;

    public GameObject P3Object;

    public BossManager bmScript;


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
            DeleteFrame();
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
        animator.SetBool("isP1", false);
        //animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isStop", false);

        StartCoroutine(ShootBullets(5));
        
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
        if (attackPrefab == null || player == null) return; // 플레이어가 없으면 실행 X
        
        GameObject bullet = null;

        if(!isP2) {
            bullet = Instantiate(attackPrefab, transform.position + firePoint, Quaternion.identity);
        } else {
            bullet = Instantiate(P2Prefab, transform.position + firePoint, Quaternion.identity);
        }

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

        animator.SetBool("isP1", true);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isStop", false);

        if (P1Object != null)
        {
            P1Object.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(P1Object, 2.6f));
        }
    }


    public override void P2() {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;
        isP2 = true;

        animator.SetBool("isP2", true);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isStop", false);

        StartCoroutine(Delay(2.0f));
    }

    IEnumerator Delay(float sec) {
        yield return new WaitForSeconds(sec);

        isWandering = true;
        isFollowing = true;
        isStop = true;

        StartCoroutine(P2Count(20.0f));
        Invoke("Attack", 1.0f);     
    }

    IEnumerator P2Count(float sec) {
        float originalSpeed = speed;  // 기존 속도 저장
        speed *= 1.5f;



        yield return new WaitForSeconds(sec);

        speed = originalSpeed;

        animator.SetBool("isP2", false);
        bmScript.attackPos = true;
    }

    public override void P3() {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;

        animator.SetBool("isP3", true);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isStop", false);

        if (P3Object != null)
        {
            P3Object.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(P3Object, 3f));
        }
        
    }

    IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);

        if(animator.GetBool("isP2")) {
            animator.SetBool("isP3", false);
            animator.SetBool("isP1", false);
            animator.SetBool("isAttack", false);
        } else {
            animator.SetBool("isP3", false);
            animator.SetBool("isP2", false);
            animator.SetBool("isP1", false);
            animator.SetBool("isAttack", false);
        }
        
        

        isWandering = true;
        isFollowing = true;
        isStop = true;
        bmScript.attackPos = true;
    }

    void DeleteFrame() {
        Destroy(framePrefab);
    }
}
