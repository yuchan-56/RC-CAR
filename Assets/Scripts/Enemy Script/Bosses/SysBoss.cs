using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SysBoss : Boss
{
    [SerializeField] private GameObject framePrefab; // Inspector에 연결

    private GameObject frameInstance;
    bool showFrame = false;


    //attack
    public GameObject attackObject;

    //p1
    public GameObject p1Object;
    bool eDead = false;

    //p2
    public GameObject p2Object;
    //p3
    public GameObject p3Object;


    public BossManager bmScript;


    protected override void Start()
    {
        base.Start();
        Debug.Log("시스템 보스");
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

    protected override void Update()
    {
        base.Update();
        if(showHP && !showFrame) {
            ShowFrame();
        }

        if(currentHP > 50) {
                animator.SetBool("isLowHP", false);
        }
        else {
                animator.SetBool("isLowHP", true);
        }
    }

    public override void Attack()
    {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;

        animator.SetBool("isAttack", true);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        

        if(attackObject != null) {
            attackObject.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(attackObject, 3.0f));
        }
    }

    



    public override void P1()
    {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", true);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        

        StartCoroutine(Enemy());
    }
    
    IEnumerator Enemy() {
        p1Object.SetActive(true);
        sysP1 = true;
        
        yield return new WaitUntil(() => p1Object == null);

        eDead = true;
        isWandering = true;
        isFollowing = true;
        isStop = true;
        bmScript.attackPos = true;
        sysP1 = false;

        animator.SetBool("isP1", false);

        yield return null;
    }

    
    public override void P2()
    {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", true);
        animator.SetBool("isP3", false);


        if(p2Object != null) {
            p2Object.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(p2Object, 2.2f));
        }
    }

    IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        animator.SetBool("isP3", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isAttack", false);

        isWandering = true;
        isFollowing = true;
        isStop = true;
        bmScript.attackPos = true;
    }

    



    public override void P3()
    {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", true);

        StartCoroutine(MovePos());
        
    }

    IEnumerator MovePos() {
        yield return new WaitForSeconds(1.1f);

        animator.SetBool("isP3_2", true);


        transform.position = new Vector3(player.transform.position.x + 1.5f, player.transform.position.y - 1.3f, player.transform.position.z);
        
        if(p3Object != null) {
            p3Object.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(p3Object, 2.2f));
        }
    }
}
