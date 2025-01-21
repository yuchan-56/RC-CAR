using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAnimation : MonoBehaviour
{
    public void SkillMotionActive(string AttackType)
    {
        StartCoroutine(PerformAttack(AttackType));
    }
    IEnumerator PerformAttack(string AttackType)
    {
        Animator animator = GetComponent<Animator>();
        if (AttackType == "Attack")
        {
            animator.SetTrigger("Attack");
            yield return null;
        }
        else if (AttackType == "DashAttack")
        {
            animator.SetTrigger("DashAttack");
            yield return null;
        }
        else if (AttackType == "JumpAttack")
        {
            
            animator.SetTrigger("JumpAttack");
            yield return null;

        }
    }

}
