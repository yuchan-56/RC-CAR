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
        
    }


    protected override void Update()
    {
        base.Update();
        if(showHP && !showFrame) {
            ShowFrame();
        }

        if(!doAttack)
        {
            attackObject[0].SetActive(false);
            attackObject[1].SetActive(false);
            attackObject[2].SetActive(false);
            attackObject[3].SetActive(false);
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

    bool doAttack = false;

    public override void Attack() {
        this.isAttacking = true;
        bmScript.attackPos = false;
        animator.SetBool("isAttack", true);
        doAttack = true;

        StartCoroutine(EndPattern(0, 3.0f));
    }

    public void Prg1stAttack() {
        attackObject[3].SetActive(false);
        attackObject[0].SetActive(true);
    }

    public void Prg2ndAttack() {
        attackObject[0].SetActive(false);
        attackObject[1].SetActive(true);
    }

    public void Prg3rdAttack() {
        attackObject[1].SetActive(false);
        attackObject[2].SetActive(true);
    }

    public void Prg4thAttack() {
        attackObject[2].SetActive(false);
        attackObject[3].SetActive(true);
    }

    public override void P1() {
        this.isAttacking = true;
        bmScript.attackPos = false;
        animator.SetBool("isP1", true);

        StartCoroutine(EndPattern(1, 3.0f));
    }

    public void PrgP1Start() {
        if (p1Object != null)
        {
            p1Object.SetActive(true);
        }
    }

    public override void P2() {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        this.isAttacking = true;
        bmScript.attackPos = false;


        animator.SetBool("isAttack", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", true);

        StartCoroutine(P2Routine());
    }

    private IEnumerator P2Routine()
    {
        List<GameObject> fallingObjs = new List<GameObject>();
        foreach (var prefab in printW)
        {
            if (prefab == null) continue;
            
            float randomX = Random.Range(transform.position.x - 15f, transform.position.x + 15f);
            float randomY = Random.Range(7, 11);

            Vector3 startPos = new Vector3(randomX, transform.position.y + randomY, 0);
            GameObject fo = Instantiate(prefab, startPos, Quaternion.identity);
            fallingObjs.Add(fo);
        }

        
        float fallSpeedMin = 2f, fallSpeedMax = 5f;
        while (fallingObjs.Count > 0)
        {
            for (int i = fallingObjs.Count - 1; i >= 0; i--)
            {
                var obj = fallingObjs[i];
                if (obj == null)
                {
                    
                    fallingObjs.RemoveAt(i);
                    continue;
                }

                
                float fs = Random.Range(fallSpeedMin, fallSpeedMax);
                obj.transform.position += Vector3.down * fs * Time.deltaTime;

                
                if (obj.transform.position.y <= transform.position.y - 5f)
                {
                    Destroy(obj);
                    fallingObjs.RemoveAt(i);
                }
            }
            yield return null;
        }

       
        animator.SetBool("isP2", false);
        isWandering = true;
        isFollowing = true;
        isStop = true;

        bmScript.attackPos = true;   // 다음 공격 신호
        this.isAttacking = false;  // 공격 중 플래그 해제
    }




    public override void P3() {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        this.isAttacking = true;
        bmScript.attackPos = false;


        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP3", true);

        StartCoroutine(EndPattern(3, 4.0f));
    }

    public void PrgP3Start() {
        if (p3Object != null)
        {
            p3Object.SetActive(true);
        }
    }

    IEnumerator EndPattern(int attackNum, float min)
    {
        yield return new WaitForSeconds(min);

        isAttacking = false;
        isWandering = true;
        isFollowing = true;
        isStop = true;
        bmScript.attackPos = true;

        switch (attackNum)
        {
            case 0:
                animator.SetBool("isAttack", false);
                doAttack = false;

                attackObject[0].SetActive(false);
                attackObject[1].SetActive(false);
                attackObject[2].SetActive(false);
                attackObject[3].SetActive(false);

                break;
            case 1:
                p1Object.SetActive(false);
                animator.SetBool("isP1", false);
                break;
            case 2:
                break;
            case 3:
                animator.SetBool("isP3", false);
                p3Object.SetActive(false);
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
