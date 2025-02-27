using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SysBoss : Boss
{
    //p2
    public GameObject p2Object;
    //p3
    public GameObject p3Object;


    protected override void Start()
    {
        base.Start();
        Debug.Log("시스템 보스");
    }

    protected override void Update()
    {
        base.Update();
        if(hp > 50) {
                animator.SetBool("isLowHP", false);
        }
        else {
                animator.SetBool("isLowHP", true);
        }
    }

    public override void Attack()
    {
        isWandering = false;
        isFollowing = false;
        isStop = false;

        animator.SetBool("isAttack", true);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isStop", false);
    }

    



    public override void P1()
    {
        isWandering = false;
        isFollowing = false;
        isStop = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", true);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isStop", false);

        // 소환
    }

    



    public override void P2()
    {
        isWandering = false;
        isFollowing = false;
        isStop = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", true);
        animator.SetBool("isP3", false);
        animator.SetBool("isStop", false);

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
        isFollowing = true;
        isStop = true;
    }

    



    public override void P3()
    {
        isWandering = false;
        isFollowing = false;
        isStop = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", true);
        animator.SetBool("isStop", false);


        if(p2Object != null) {
            p3Object.SetActive(true);
            StartCoroutine(MovePos());
        }
    }

    IEnumerator MovePos() {
        yield return new WaitForSeconds(1.05f);
        animator.SetBool("isP3_2", true);


        transform.position = new Vector3(player.transform.position.x + 1.5f, player.transform.position.y, player.transform.position.z);
        
    }
}
