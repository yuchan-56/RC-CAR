using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SndBoss : Boss
{
    public GameObject hpFramePref;
    public Image hpFrameImg;


    public GameObject[] attackObject = new GameObject[3];
    public GameObject P1Object;
    public GameObject P3Object;

    public BossManager bmScript;


    protected override void Start()
    {
        base.Start();
        Debug.Log("sound boss 등장!");
    }


    public override void Attack() {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;


        animator.SetBool("isAttack", true);
        animator.SetBool("isP1", false);
        //animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isStop", false);

        if (attackObject != null)
        {
            //StartCoroutine(SndAttack());
        }

        animator.SetBool("isAttack", false);
    }

    /*
    IEnumerator SndAttack() {
        for(int i = 0; i < attackObject.Length; i++) {
            GameObject aa = Instantiate(attackObject[i]);

            yield return new WaitForSeconds(0.3f);
        }
    }*/

    public override void P1() {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;

        animator.SetBool("isP1", true);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isStop", false);

        if (P1Object != null)
        {
            P1Object.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(P1Object, 2.6f));
        }
    }


    public override void P2() {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;

        animator.SetBool("isP2", true);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isStop", false);

        StartCoroutine(Delay(2.0f));
    }

    IEnumerator Delay(float sec) {
        yield return new WaitForSeconds(sec);

        isWandering = true;
        isFollowing = true;
        isStop = true;

        StartCoroutine(P2Count(20.0f));
        Invoke("Attack", 1.0f);     
    }

    IEnumerator P2Count(float sec) {
        float originalSpeed = speed;  // 기존 속도 저장
        speed *= 1.5f;



        yield return new WaitForSeconds(sec);

        speed = originalSpeed;

        animator.SetBool("isP2", false);
        bmScript.attackPos = true;
    }

    public override void P3() {
        isWandering = false;
        isFollowing = false;
        isStop = false;
        bmScript.attackPos = false;

        animator.SetBool("isP3", true);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isStop", false);

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

        if(animator.GetBool("isP2")) {
            animator.SetBool("isP3", false);
            animator.SetBool("isP1", false);
            animator.SetBool("isAttack", false);
        } else {
            animator.SetBool("isP3", false);
            animator.SetBool("isP2", false);
            animator.SetBool("isP1", false);
            animator.SetBool("isAttack", false);
        }
        
        

        isWandering = true;
        isFollowing = true;
        isStop = true;
        bmScript.attackPos = true;
    }
}
