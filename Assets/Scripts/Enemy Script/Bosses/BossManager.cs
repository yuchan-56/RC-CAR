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


        if (!_started)
        {
            StartCoroutine(RandomBossAttackRoutine());
            _started = true;
        }

    }


    
    IEnumerator RandomBossAttackRoutine()
    {
       


        while (true)
        {
            
            yield return new WaitUntil(() =>
            {
                if (!attackPos) return false;
                foreach (var b in allBosses)
                    if (b != null && !b.isDead && !b.isAttacking && b.attackPossible)
                        return true;
                return false;
            });

            
            yield return new WaitForSeconds(attackInterval);

            
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
                
                break;
            case 1:
                boss.P1();
                
                break;
            case 2:
                boss.P2();
                
                break;
            case 3:
                boss.P3();
                
                break;
        }
    }


    void DoAll(System.Action<Boss> act)
    {
        foreach (var b in allBosses)
            if (b != null && !b.isDead && !b.isAttacking && b.attackPossible)
                act(b);
    }
}
