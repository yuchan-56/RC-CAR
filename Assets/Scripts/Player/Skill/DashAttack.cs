using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;
using UnityEngine;

public class DashAttack : MonoBehaviour
{
    public BoxCollider2D boxCollider2D;
    public PlayerAttackGeneral playerAttackGeneral;
    public Enemy enemy;
    SpriteRenderer spriteRenderer;
    Animator ani;
    bool SkillActive_DashAttack;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        SkillActive_DashAttack = false;
        boxCollider2D.enabled = false;
        spriteRenderer.enabled = false;
        ani.SetBool("UltDashAtt", false);
        ani.SetBool("DashAtt", false);
    }

    IEnumerator DeactiveCoroutine()
    {
        if(Managers.Game.SkillAniReset == true)
        {
            StopCoroutine(DeactiveCoroutine());
        }
        yield return new WaitForSeconds(0.8f);
        SkillMotionDeactive();
        StopCoroutine(DeactiveCoroutine());
    }

    public void SkillMotionActive()
    {
        SkillActive_DashAttack = true;
        boxCollider2D.enabled = true;
        spriteRenderer.enabled = true;

        StartCoroutine(DeactiveCoroutine());

        if (playerAttackGeneral.UltimateSkill_Active)
        {
            ani.SetBool("UltDashAtt", true);
        }
        else
        {
            ani.SetBool("DashAtt", true);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().EnemyDamage(Managers.Game.damage);
        }
    }

    public void SkillMotionDeactive()
    {
        spriteRenderer.enabled = false;
        SkillActive_DashAttack = false;
        boxCollider2D.enabled = false;
        spriteRenderer.enabled = false;
        ani.SetBool("UltDashAtt", false);
        ani.SetBool("DashAtt", false);
        if (Managers.Game.SkillAniReset == true)
        {
            StopCoroutine(DeactiveCoroutine());
        }
    }
}
