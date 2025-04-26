using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrgBoss : Boss
{
    [SerializeField] private GameObject framePrefab; // Inspector에 연결

    private GameObject frameInstance;
    bool showFrame = false;


    //attack
    public GameObject[] attackObject = new GameObject[4];
    //p1
    public GameObject p1Object;
    //P2
    public GameObject[] printW = new GameObject[8];
    //p3
    public GameObject p3Object;


    public BossManager bmScript;



    protected override void Start()
    {
        base.Start();
        Debug.Log("프로그래밍 보스");
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
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;


        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isAttack", true);

        StartCoroutine(DeactivateAfterDelay(3.7f));
    }

    public void Prg1stAttack() {
        attackObject[0].SetActive(true);
        StartCoroutine(AttackColliderOff(attackObject[0]));
    }

    public void Prg2ndAttack() {
        attackObject[1].SetActive(true);
        StartCoroutine(AttackColliderOff(attackObject[1]));
    }

    public void Prg3rdAttack() {
        attackObject[2].SetActive(true);
        StartCoroutine(AttackColliderOff(attackObject[2]));
    }

    public void Prg4thAttack() {
       attackObject[3].SetActive(true);
       StartCoroutine(AttackColliderOff(attackObject[3]));
    }

    public void PrgAttackOff() {
        attackObject[0].SetActive(false);
        attackObject[1].SetActive(false);
        attackObject[2].SetActive(false);
        attackObject[3].SetActive(false);
    }

    IEnumerator AttackColliderOff(GameObject g)
    {
        yield return new WaitForSeconds(0.15f);
        
        g.SetActive(false);

    }

    public override void P1() {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;
        
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", true);

        StartCoroutine(DeactivateAfterDelay(3f));
    }

    public void PrgP1Start() {
        if (p1Object != null)
        {
            p1Object.SetActive(true);
        }
    }

    /*
    public void PrgP1Off() {
        p1Object.SetActive(false);
    }
    */


    //------

    public override void P2() {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;


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
        // 랜덤 X 좌표 설정
        float randomX = UnityEngine.Random.Range(transform.position.x - 15f, transform.position.x + 15f);

        // 초기 위치 설정
        Vector3 startPosition = new Vector3(randomX, transform.position.y + 7f, 0);
        GameObject fallingObj = Instantiate(obj, startPosition, Quaternion.identity);

        float fallSpeed = UnityEngine.Random.Range(2f, 5f);

        
        while (fallingObj.transform.position.y > transform.position.y - 5f)
        {
            if(fallingObj == null)
            {
                break;
            }
            fallingObj.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }
        
        

        Destroy(fallingObj);
        animator.SetBool("isP2", false);

        isWandering = true;
        isFollowing = true;
        isStop = true;
        bmScript.attackPos = true;
        isAttacking = false;

    }


    // ------

    public override void P3() {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;


        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP3", true);

        StartCoroutine(DeactivateAfterDelay(5f));
    }

    public void PrgP3Start() {
        if (p3Object != null)
        {
            p3Object.SetActive(true);
        }
    }

    public void PrgP3Off() {
        p3Object.SetActive(false);
    }

    IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);


        animator.SetBool("isP3", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isAttack", false);

        isWandering = true;
        isFollowing = true;
        isStop = true;
        bmScript.attackPos = true;
        isAttacking = false;

        p1Object.SetActive(false);
        p3Object.SetActive(false);
        attackObject[0].SetActive(false);
        attackObject[1].SetActive(false);
        attackObject[2].SetActive(false);
        attackObject[3].SetActive(false);
    }
}
