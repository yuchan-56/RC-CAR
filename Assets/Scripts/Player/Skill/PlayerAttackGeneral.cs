using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackGeneral : MonoBehaviour
{
    public CharacterEffect characterEffect;
    public BoxCollider2D boxCollider2D;
    public Enemy enemy;
    Animator ani;
    SpriteRenderer spriteRenderer;
    bool SkillAttack_Active;
    public bool UltimateSkill_Active;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
        ani = GetComponent<Animator>();
        SkillAttack_Active = false;
        ani.SetBool("UltAttack", false);
        ani.SetBool("Attack", false);
    }

    public void AttackSetActive()
    {
        spriteRenderer.enabled = true;
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().EnemyDamage(Managers.Game.damage);
        }
    }

    void AttackSetDeactive()
    {
        boxCollider2D.enabled = false;
        spriteRenderer.enabled = false;
        SkillAttack_Active = false;
        ani.SetBool("UltAttack", false);
        ani.SetBool("Attack", false);
    }


    public void UltimateSkillActive()
    {
        UltimateSkill_Active = true;
        Managers.Game.gage = 0;
        characterEffect.UltimateEffectActive();
        Invoke("UltimateSkillDeactive", 2f);
    }

    void UltimateSkillDeactive()
    {
        UltimateSkill_Active = false;
        characterEffect.UltimateEffectDeactive();
    }
}
