using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    public PlayerAttackGeneral playerAttackGeneral;
    public BoxCollider2D boxCollider2D;
    Animator ani;
    bool SkillActive_JumpAttack;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        boxCollider2D.enabled = false;
        SkillActive_JumpAttack = false;
        ani.SetBool("UltJumpAtt", false);
        ani.SetBool("JumpAtt", true);
    }


    public void SkillMotionActive()
    {
        SkillActive_JumpAttack = true;
        boxCollider2D.enabled = true;
        if (SkillActive_JumpAttack == true)
        {
            Invoke("SkillMotionDeactive", 1f);
        }
        else
        {
            return;
        }

        if(playerAttackGeneral.UltimateSkill_Active)
        {
            ani.SetBool("UltJumpAtt", true);
        }
        else
        {
            ani.SetBool("JumpAtt", true);
        }
    }

    void SkillMotionDeactive()
    {
        SkillActive_JumpAttack = false;
        boxCollider2D.enabled = false;
        ani.SetBool("UltJumpAtt", false);
        ani.SetBool("JumpAtt", true);
    }
}
