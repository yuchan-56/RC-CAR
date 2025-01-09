using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackGeneral : MonoBehaviour
{
    CharacterEffect characterEffect;
    BoxCollider2D boxCollider2D;
    bool SkillAttack_Active;
    public bool UltimateSkill_Active;
    float SkillDamage;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
        SkillAttack_Active = false;
    }

    void AttackSetActive()
    {
        boxCollider2D.enabled = true;
        SkillAttack_Active = true;
        if(SkillAttack_Active == true)
        {
            Invoke("AttackSetDeactive", 1f);
        }
    }

    void AttackSetDeactive()
    {
        boxCollider2D.enabled = false;
        SkillAttack_Active = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && UltimateSkill_Active == false)
        {
            AttackSetActive();
            SkillDamage = 10;
        }
        else
        {
            AttackSetActive();
            SkillDamage = 20;
        }
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
