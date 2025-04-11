
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
    private List<Enemy> hitEnemies = new List<Enemy>();

    void Awake()
    {
        if (enemy == null)
        {
            enemy = UnityEngine.Object.FindObjectOfType<Enemy>();
        }
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
        ani = GetComponent<Animator>();
        SkillAttack_Active = false;
        ani.SetBool("UltAttack", false);
        ani.SetBool("Attack", false);
    }
    IEnumerator UltDeactiveCoroutine()
    {
        if (Managers.Game.SkillAniReset == true)
        {
            StopCoroutine(UltDeactiveCoroutine());
        }
        yield return new WaitForSeconds(5);
        UltimateSkillDeactive();
        StopCoroutine(UltDeactiveCoroutine());
    }
    IEnumerator DeactiveCoroutine()
    {
        if (Managers.Game.SkillAniReset == true)
        {
            StopCoroutine(DeactiveCoroutine());
        }
        yield return new WaitForSeconds(0.5f);
        AttackSetDeactive();
        StopCoroutine(DeactiveCoroutine());
    }

    public void AttackSetActive()
    {
        spriteRenderer.enabled = true;
        boxCollider2D.enabled = true;
        SkillAttack_Active = true;
        StartCoroutine(DeactiveCoroutine());

        if (UltimateSkill_Active)
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
        Enemy enemy = collision.GetComponent<Enemy>();
        if (collision.tag == "Enemy" && enemy.isEnemyHit == false)
        {
            enemy.isEnemyHit = true;
            hitEnemies.Add(enemy);
            collision.GetComponent<Enemy>().EnemyDamage(Managers.Game.damage, 1);
        }
    }

    public void AttackSetDeactive()
    {
        boxCollider2D.enabled = false;
        spriteRenderer.enabled = false;
        SkillAttack_Active = false;
        foreach (Enemy enemy in hitEnemies)
        {
            if (enemy != null)
                enemy.isEnemyHit = false;
        }

        hitEnemies.Clear();
        ani.SetBool("UltAttack", false);
        ani.SetBool("Attack", false);
        if(Managers.Game.SkillAniReset == true)
        {
            StopCoroutine(DeactiveCoroutine());
            StopCoroutine(UltDeactiveCoroutine());
        }
    }


    public void UltimateSkillActive()
    {
        Managers.UI.ShowPopUpUI<UltGoAction>();
        UltimateSkill_Active = true;
        Managers.Game.gage = 0;
        characterEffect.UltimateEffectActive();
        StartCoroutine(UltDeactiveCoroutine());
    }

    void UltimateSkillDeactive()
    {
        UltimateSkill_Active = false;
        characterEffect.UltimateEffectDeactive();
    }
}
