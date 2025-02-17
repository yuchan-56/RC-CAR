using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SndBoss : Boss
{

    public GameObject attackObject;
    public GameObject P1Object;
    public GameObject P3Object;


    protected override void Start()
    {
        base.Start();
        Debug.Log("보스 등장!");
    }


    public override void Attack() {
        isWandering = false;

        animator.SetBool("isAttack", true);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);

        if (attackObject != null)
        {
            attackObject.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(attackObject, 3f));
        }
    }

    public override void P1() {
        isWandering = false;

        animator.SetBool("isP1", true);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);

        if (P1Object != null)
        {
            P1Object.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(P1Object, 3f));
        }
    }


    public override void P2() {
        isWandering = false;

        animator.SetBool("isP2", true);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP3", false);

    }

    public override void P3() {
        isWandering = false;

        animator.SetBool("isP3", true);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);

        if (P3Object != null)
        {
            P3Object.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(P3Object, 3f));
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
}
