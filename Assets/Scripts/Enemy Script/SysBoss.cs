using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SysBoss : Boss
{
    //p2
    public GameObject p2Object;
    protected override void Start()
    {
        base.Start();
        Debug.Log("시스템 보스");
    }

    protected override void Update()
    {
        base.Update();
        if(hp > 30) {
                animator.SetBool("isLowHP", false);
        }
        else {
                animator.SetBool("isLowHP", true);
        }
    }

    public override void Attack()
    {
        isWandering = false;

        animator.SetBool("isAttack", true);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
    }

    



    public override void P1()
    {
        isWandering = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", true);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
    }

    



    public override void P2()
    {
        isWandering = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", true);
        animator.SetBool("isP3", false);

        if(p2Object != null) {
            p2Object.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(p2Object, 2.0f));
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
    }

    



    public override void P3()
    {
        isWandering = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", true);
    }
}
