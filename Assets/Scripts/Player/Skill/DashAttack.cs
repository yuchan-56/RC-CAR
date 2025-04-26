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
    private List<EnemyHP> hitEnemies = new List<EnemyHP>();

    void Awake()
    {
        if(enemy ==  null)
        {
            enemy = UnityEngine.Object.FindObjectOfType<Enemy>();
        }
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
        EnemyHP enemy = collision.GetComponent<EnemyHP>();

        if (collision.CompareTag("Enemy") && enemy != null
            && enemy.IsEnemyHit == false && enemy.IsEnemyDead == false)
        {
            enemy.IsEnemyHit = true;
            hitEnemies.Add(enemy);

            if (Managers.Game.gage < 100)
            {
                Managers.Game.gage += 5;
            }

            enemy.EnemyDamage(Managers.Game.damage, 1);
        }
    }

    public void SkillMotionDeactive()
    {
        spriteRenderer.enabled = false;
        SkillActive_DashAttack = false;
        boxCollider2D.enabled = false;
        spriteRenderer.enabled = false;
        foreach (Enemy enemy in hitEnemies)
        {
            if (enemy != null)
                enemy.IsEnemyHit = false;
        }
        hitEnemies.Clear();
        ani.SetBool("UltDashAtt", false);
        ani.SetBool("DashAtt", false);
        if (Managers.Game.SkillAniReset == true)
        {
            StopCoroutine(DeactiveCoroutine());
        }
    }
}
