using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SysBoss : Boss
{
    public BossManager bmScript;

    [SerializeField] private GameObject framePrefab; // Inspector에 연결

    private GameObject frameInstance;
    bool showFrame = false;


    //attack
    public GameObject[] attackObject = new GameObject[6];

    //p1
    public Transform[] p1SpawnPoints = new Transform[2];
    public GameObject[] p1EnemyPrefabs = new GameObject[2];
    private List<GameObject> _p1Enemies;
    bool didP1 = false;
    //bool eDead = false;

    //p2
    public GameObject[] p2Object = new GameObject[3];


    //p3
    public GameObject p3Object;
    public Vector2 p3TeleportOffset = new Vector2(2f, 0f);


    protected override void Start()
    {
        base.Start();
        Debug.Log("시스템 보스");
    }

    protected override void Update()
    {
        base.Update();

        if (showHP && !showFrame)
        {
            ShowFrame();
        }

        if (isDead)
        {
            Destroy(frameInstance);
            frameInstance = null;
        }


        if (currentHP > 50)
        {
            animator.SetBool("isLowHP", false);
        }
        else
        {
            animator.SetBool("isLowHP", true);
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

   

    public override void Attack()
    {
        this.isAttacking = true;
        bmScript.attackPos = false;

        animator.SetBool("isAttack", true);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
    }

    public void sysAttack0()
    {
        attackObject[0].SetActive(true);
    }

    public void sysAttack1()
    {
        attackObject[0].SetActive(false);
        attackObject[1].SetActive(true);
    }

    public void sysAttack2()
    {
        attackObject[1].SetActive(false);
        attackObject[2].SetActive(true);
    }

    public void sysAttack3()
    {
        attackObject[2].SetActive(false);
        attackObject[3].SetActive(true);
    }

    public void sysAttack4()
    {
        attackObject[3].SetActive(false);
        attackObject[4].SetActive(true);
    }

    public void sysAttack5()
    {
        attackObject[4].SetActive(false);
        attackObject[5].SetActive(true);
    }

    public void sysAttackOff()
    {
        attackObject[5].SetActive(false);
        StartCoroutine(EndPattern(0, 0.1f));
    }


    public override void P1()
    {
        isAttacking = true;
        bmScript.attackPos = false;
        sysP1 = true;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", true);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
    }

    public void sysP1AnimEnd()
    {
        if (!didP1)
        {
            _p1Enemies = new List<GameObject>();

            for (int i = 0; i < p1EnemyPrefabs.Length; i++)
            {
                float sign = (i % 2 == 0) ? 1f : -1f;
                float offsetX = Random.Range(-1f, 1f) * sign;
                Vector3 spawnPos = new Vector3(
                    player.transform.position.x + offsetX,
                    transform.position.y,
                    transform.position.z
                );

                var go = Instantiate(p1EnemyPrefabs[i], spawnPos, Quaternion.identity);
                _p1Enemies.Add(go);
            }

            didP1 = true;
           
            StartCoroutine(P1Routine());
        }
        
    }

    private IEnumerator P1Routine()
    {
        while (_p1Enemies.Exists(e => e != null)) yield return null;

        animator.SetBool("isP1", false);
        
        StartCoroutine(EndPattern(1, 0.5f));
    }





    public override void P2()
    {
        this.isAttacking = true;
        bmScript.attackPos = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", true);
        animator.SetBool("isP3", false);
    }

    public void sysP2C0()
    {
        p2Object[0].SetActive(true);
    }

    public void sysP2C1()
    {
        p2Object[0].SetActive(false);
        p2Object[1].SetActive(true);
    }

    public void sysP2C2()
    {
        p2Object[1].SetActive(false);
        p2Object[2].SetActive(true);

        StartCoroutine(EndPattern(2, 0.5f));
    }

    public void sysP2Off()
    {
        p2Object[2].SetActive(false);

        StartCoroutine(EndPattern(2, 0.1f));
    }




    public override void P3()
    {
        this.isAttacking = true;
        bmScript.attackPos = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", true);
    }

    public void OnP3Phase1End()
    {
        animator.SetBool("isP3_2", true);
        if (player != null)
        {
            Vector3 target = new Vector3(
                player.transform.position.x + p3TeleportOffset.x,
                transform.position.y,
                transform.position.z
            );
            transform.position = target;
        }

        StartCoroutine(EndPattern(3, 2.0f));
    }

    public void sysP3On()
    {
        p3Object.SetActive(true);
    }

    public void sysP3Off()
    {
        p3Object.SetActive(false);
    }





    IEnumerator EndPattern(int attackNum, float min)
    {
        yield return new WaitForSeconds(min);
        isWandering = true;
        isFollowing = true;
        isStop = true;
        isAttacking = false;
        bmScript.attackPos = true;

        switch (attackNum)
        {
            case 0:
                attackObject[0].SetActive(false);
                attackObject[1].SetActive(false);
                attackObject[2].SetActive(false);
                attackObject[3].SetActive(false);
                attackObject[4].SetActive(false);
                attackObject[5].SetActive(false);

                yield return new WaitForSeconds(1.0f);

                animator.SetBool("isAttack", false);
                break;
            case 1:
                animator.SetBool("isP1", false);
                didP1 = false;
                break;
            case 2:
                p2Object[0].SetActive(false);
                p2Object[1].SetActive(false);
                p2Object[2].SetActive(false);
                animator.SetBool("isP2", false);
                break;
            case 3:
                animator.SetBool("isP3", false);
                animator.SetBool("isP3_2", false);
                break;

        }
    }
}
