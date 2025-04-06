using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    public PlayerAttackGeneral playerAttackGeneral;
    public BoxCollider2D boxCollider2D;
    public Enemy enemy;
    SpriteRenderer spriteRenderer;
    Animator ani;
    bool SkillActive_JumpAttack;

    IEnumerator DeactiveCoroutine()
    {
        if (Managers.Game.SkillAniReset == true)
        {
            StopCoroutine(DeactiveCoroutine());
        }
        yield return new WaitForSeconds(1f);
        SkillMotionDeactive();
        StopCoroutine(DeactiveCoroutine());
    }

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        boxCollider2D.enabled = false;
        spriteRenderer.enabled = false;
        SkillActive_JumpAttack = false;
        ani.SetBool("UltJumpAtt", false);
        ani.SetBool("JumpAtt", false);
    }


    public void SkillMotionActive()
    {
        SkillActive_JumpAttack = true;
        boxCollider2D.enabled = true;
        spriteRenderer.enabled = true;
        StartCoroutine(DeactiveCoroutine());

        if (playerAttackGeneral.UltimateSkill_Active)
        {
            ani.SetBool("UltJumpAtt", true);
        }
        else
        {
            ani.SetBool("JumpAtt", true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && enemy.isEnemyHit == false)
        {
            collision.GetComponent<Enemy>().EnemyDamage(Managers.Game.damage, 2);
        }
    }

    public void SkillMotionDeactive()
    {
        SkillActive_JumpAttack = false;
        boxCollider2D.enabled = false;
        spriteRenderer.enabled = false;
        ani.SetBool("UltJumpAtt", false); 
        ani.SetBool("JumpAtt", false);
        if (Managers.Game.SkillAniReset == true)
        {
            StopCoroutine(DeactiveCoroutine());
        }
    }
}
