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

        if(isDead)
        {
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
        isWandering = false;
        isFollowing = false;
        isStop = false;
        this.isAttacking = true;
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
        this.isAttacking = true;
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
        // 1) 프리팹을 Instantiate 해서 리스트에 담는다
        List<GameObject> fallingObjs = new List<GameObject>();
        foreach (var prefab in printW)
        {
            if (prefab == null) continue;
            // 원하는 시작 위치 계산
            float randomX = Random.Range(transform.position.x - 15f, transform.position.x + 15f);
            float randomY = Random.Range(7, 11);

            Vector3 startPos = new Vector3(randomX, transform.position.y + randomY, 0);
            GameObject fo = Instantiate(prefab, startPos, Quaternion.identity);
            fallingObjs.Add(fo);
        }

        // 2) 모두 땅에 닿고 Destroy 될 때까지 매 프레임 위치 갱신
        float fallSpeedMin = 2f, fallSpeedMax = 5f;
        while (fallingObjs.Count > 0)
        {
            for (int i = fallingObjs.Count - 1; i >= 0; i--)
            {
                var obj = fallingObjs[i];
                if (obj == null)
                {
                    // 이미 Destroy 됐으면 리스트에서 제거
                    fallingObjs.RemoveAt(i);
                    continue;
                }

                // 떨어뜨리기
                float fs = Random.Range(fallSpeedMin, fallSpeedMax);
                obj.transform.position += Vector3.down * fs * Time.deltaTime;

                // 땅에 닿으면 제거
                if (obj.transform.position.y <= transform.position.y - 5f)
                {
                    Destroy(obj);
                    fallingObjs.RemoveAt(i);
                }
            }
            yield return null;
        }

        // 3) 모든 낙하 완료 후 리셋
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
        this.isAttacking = true;    // ← 추가
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
        bmScript.attackPos = true;    // 다음 공격 가능 신호
        this.isAttacking = false;

        p1Object.SetActive(false);
        p3Object.SetActive(false);
        attackObject[0].SetActive(false);
        attackObject[1].SetActive(false);
        attackObject[2].SetActive(false);
        attackObject[3].SetActive(false);
    }
}
