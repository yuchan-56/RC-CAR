using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BossManager : MonoBehaviour
{
    private List<Boss> allBosses = new List<Boss>(); // 모든 보스 리스트
    private float attackInterval = 1.5f;
    //private float timer = 0f;

    public bool attackPos = true;



    void Start()
    {
        allBosses.AddRange(FindObjectsOfType<Boss>());

        foreach (Boss boss in allBosses)
        {
            Debug.Log("감지된 보스: " + boss.GetType().Name);
        }

        
        StartCoroutine(RandomBossAttackRoutine());
        
    }

    
    IEnumerator RandomBossAttackRoutine()
    {
        while (true)
        {
            // 공격이 끝날 때까지 대기
            yield return new WaitUntil(() => attackPos == true);
            yield return new WaitForSeconds(attackInterval);
            
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
    }

    void ExecuteRandomAttack(Boss boss)
    {
        int attackType = Random.Range(0, 4); // 0~3 사이 랜덤 선택

        switch (attackType)
        {
            case 0:
                boss.Attack();
                Debug.Log(boss.GetType().Name + "이(가) 기본 공격!");
                break;
            case 1:
                boss.P1();
                Debug.Log(boss.GetType().Name + "이(가) P1 공격!");
                break;
            case 2:
                boss.P2();
                Debug.Log(boss.GetType().Name + "이(가) P2 공격!");

                break;
            case 3:
                boss.P3();
                Debug.Log(boss.GetType().Name + "이(가) P3 공격!");
                break;
        }
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) ExecuteBossAction(boss => boss.Attack());
        if (Input.GetKeyDown(KeyCode.I)) ExecuteBossAction(boss => boss.P1());
        if (Input.GetKeyDown(KeyCode.O)) ExecuteBossAction(boss => boss.P2());
        if (Input.GetKeyDown(KeyCode.P)) ExecuteBossAction(boss => boss.P3());
        
    }

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

}
