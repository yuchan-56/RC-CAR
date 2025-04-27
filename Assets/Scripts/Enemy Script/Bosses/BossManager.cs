using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BossManager : MonoBehaviour
{
    private static BossManager _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }


    private List<Boss> allBosses; // 모든 보스 리스트
    public float attackInterval = 4.0f;
    public bool attackPos = true;
    private bool _started = false;



    void Start()
    {
        allBosses = new List<Boss>(FindObjectsOfType<Boss>());

        foreach (var b in allBosses) { Debug.Log("감지된 보스: " + b.GetType().Name); }


        if (!_started)
        {
            StartCoroutine(RandomBossAttackRoutine());
            _started = true;
        }

    }


    
    IEnumerator RandomBossAttackRoutine()
    {
        /*
        while (true)
        {
            // 공격이 끝날 때까지 대기
            yield return new WaitUntil(() => attackPos == true);

            if(attackPos)
            {
                bossScript.isAttacking = false;
                yield return new WaitForSeconds(attackInterval);
                //attackInterval = 4.0f;
            }
           

            if (allBosses.Count > 0)
            {
                List<Boss> aliveBosses = allBosses.FindAll(b => b != null && !b.isDead);

                if (aliveBosses.Count > 0)
                {
                    Boss randomBoss = aliveBosses[Random.Range(0, aliveBosses.Count)];
                    ExecuteRandomAttack(randomBoss);
                }
            }
         }
        */


        while (true)
        {
            // 1) attackPos == true (중앙 신호)
            // 2) 살아있고(!isDead) 현재 공격 중이 아니며(!isAttacking)
            // 3) 같은 층에 있는 보스(attackPossible == true) 가 하나라도 있을 때까지 대기
            yield return new WaitUntil(() =>
            {
                if (!attackPos) return false;
                foreach (var b in allBosses)
                    if (b != null && !b.isDead && !b.isAttacking && b.attackPossible)
                        return true;
                return false;
            });

            // 실제 공격 전 인터벌
            yield return new WaitForSeconds(attackInterval);

            // 필터링된 보스들 중 하나를 랜덤 선택
            var candidates = allBosses.FindAll(b =>
                b != null &&
                !b.isDead &&
                !b.isAttacking &&
                b.attackPossible
            );
            if (candidates.Count == 0)
                continue;

            var randomBoss = candidates[Random.Range(0, candidates.Count)];
            ExecuteRandomAttack(randomBoss);
        }
    }



    void ExecuteRandomAttack(Boss boss)
    {
        // 이 보스만 공격 중 상태로 표시
        boss.isAttacking = true;

        // 랜덤 공격 타입 실행
        int type = Random.Range(0, 4);
        switch (type)
        {
            case 0:
                boss.Attack();
                Debug.Log($"{boss.GetType().Name} 기본 공격");
                break;
            case 1:
                boss.P1();
                Debug.Log($"{boss.GetType().Name} P1 공격");
                break;
            case 2:
                boss.P2();
                Debug.Log($"{boss.GetType().Name} P2 공격");
                break;
            case 3:
                boss.P3();
                Debug.Log($"{boss.GetType().Name} P3 공격");
                break;
        }
    }




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) DoAll(b => b.Attack());
        if (Input.GetKeyDown(KeyCode.I)) DoAll(b => b.P1());
        if (Input.GetKeyDown(KeyCode.O)) DoAll(b => b.P2());
        if (Input.GetKeyDown(KeyCode.P)) DoAll(b => b.P3());

    }

    /*
        void ExecuteBossAction(System.Action<Boss> action)
        {
            foreach (Boss boss in allBosses)
            {
                if (boss != null && !boss.isDead)
                {
                    action(boss);
                }
            }
        }
    */

    void DoAll(System.Action<Boss> act)
    {
        foreach (var b in allBosses)
            if (b != null && !b.isDead && !b.isAttacking && b.attackPossible)
                act(b);
    }
}
