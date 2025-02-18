using UnityEngine;
using System.Collections.Generic;

public class BossManager : MonoBehaviour
{
    private List<Boss> allBosses = new List<Boss>(); // 모든 보스 리스트

    void Start()
    {
        allBosses.AddRange(FindObjectsOfType<Boss>());

        foreach (Boss boss in allBosses)
        {
            Debug.Log("감지된 보스: " + boss.GetType().Name);
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
