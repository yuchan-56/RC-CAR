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
        Debug.Log("sound boss 등장!");
    }


    public override void Attack() {
        isWandering = false;
        isFollowing = false;

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
        isFollowing = false;

        animator.SetBool("isP1", true);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);

        if (P1Object != null)
        {
            StartCoroutine(Delaying(1.2f));
            StartCoroutine(DeactivateAfterDelay(P1Object, 2.6f));
        }
    }

    IEnumerator Delaying(float sec) {
        yield return new WaitForSeconds(sec);

        P1Object.SetActive(true);
    }


    public override void P2() {
        isWandering = false;
        isFollowing = false;

        animator.SetBool("isP2", true);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP3", false);

        StartCoroutine(P2Count(15.0f));
    }


    // 고쳐!!!!!!!!!!!!!!!!!!!!!!!
    IEnumerator P2Count(float sec) {
        yield return new WaitForSeconds(sec);
        animator.SetBool("isP2", false);
    }

    public override void P3() {
        isWandering = false;
        isFollowing = false;

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
        isFollowing = true;
    }
}
