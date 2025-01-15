using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackGeneral : MonoBehaviour
{
    CharacterEffect characterEffect;
    public BoxCollider2D boxCollider2D;
    Animator ani;
    bool SkillAttack_Active;
    public bool UltimateSkill_Active;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
        ani = GetComponent<Animator>();
        SkillAttack_Active = false;
        ani.SetBool("UltAttack", false);
        ani.SetBool("Attack", false);
    }

    public void AttackSetActive()
    {
        boxCollider2D.enabled = true;
        SkillAttack_Active = true;
        if(SkillAttack_Active == true)
        {
            Invoke("AttackSetDeactive", 1f);
        }
        else
        {
            return;
        }

        if(UltimateSkill_Active)
        {
            ani.SetBool("UltAttack", true);
        }
        else
        {
            ani.SetBool("Attack", true);
        }
    }

    void AttackSetDeactive()
    {
        boxCollider2D.enabled = false;
        SkillAttack_Active = false;
        ani.SetBool("UltAttack", false);
        ani.SetBool("Attack", false);
    }


    public void UltimateSkillActive()
    {
        UltimateSkill_Active = true;
        characterEffect.UltimmateEffectActive();
        Invoke("UltimateSkillDeactive", 2f);
    }

    void UltimateSkillDeactive()
    {
        UltimateSkill_Active = false;
        characterEffect.UltimateEffectDeactive();
    }
}
