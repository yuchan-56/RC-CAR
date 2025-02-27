using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrgBoss : Boss
{
    //attack
    public GameObject attackObject;
    //p1
    public GameObject p1Object;
    //P2
    public GameObject[] printW = new GameObject[8];
    //p3
    public GameObject p3Object;



    protected override void Start()
    {
        base.Start();
        Debug.Log("프로그래밍 보스");
    }


    public override void Attack() {
        isWandering = false;
        isFollowing = false;
        isStop = false;

        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isAttack", true);
        animator.SetBool("isStop", false);
        

        if (attackObject != null)
        {
            attackObject.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(attackObject, 4f));
        }
    }

    public override void P1() {
        isWandering = false;
        isFollowing = false;
        isStop = false;

        animator.SetBool("isP2", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP1", true);
        animator.SetBool("isStop", false);
        

        if (p1Object != null)
        {
            p1Object.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(p1Object, 3f));
        }
    }


    //------

    public override void P2() {
        isWandering = false;
        isFollowing = false;
        isStop = false;

        animator.SetBool("isAttack", false);
        animator.SetBool("isP3", false);
        animator.SetBool("isP1", false);
        animator.SetBool("isP2", true);
        animator.SetBool("isStop", false);
    

        foreach (GameObject obj in printW)
        {
            if (obj != null)
            {
                StartCoroutine(FallObject(obj));
            }
        }
    }

    

   IEnumerator FallObject(GameObject obj)
    {
        // 랜덤 X 좌표 설정
        float randomX = UnityEngine.Random.Range(transform.position.x - 15f, transform.position.x - 1f);

        // 초기 위치 설정
        Vector3 startPosition = new Vector3(randomX, transform.position.y + 7f, 0);
        GameObject fallingObj = Instantiate(obj, startPosition, Quaternion.identity);

        float fallSpeed = UnityEngine.Random.Range(2f, 5f);

        while (fallingObj.transform.position.y > transform.position.y - 5f)
        {
            fallingObj.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(fallingObj);
        animator.SetBool("isP2", false);

        isWandering = true;
        isFollowing = true;
        isStop = true;
    }


    // ------

    public override void P3() {
        isWandering = false;
        isFollowing = false;
        isStop = false;

        animator.SetBool("isP1", false);
        animator.SetBool("isP2", false);
        animator.SetBool("isAttack", false);
        animator.SetBool("isP3", true);
        animator.SetBool("isStop", false);
        
        if (p3Object != null)
        {
            p3Object.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(p3Object, 5f));
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
}
