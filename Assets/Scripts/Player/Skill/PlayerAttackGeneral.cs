
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
    private List<EnemyHP> hitEnemies = new List<EnemyHP>();

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
        UltimateSkill_Active = false;
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

    public void AttackSetDeactive()
    {
        boxCollider2D.enabled = false;
        spriteRenderer.enabled = false;
        SkillAttack_Active = false;
        foreach (EnemyHP enemy in hitEnemies)
        {
            if (enemy != null)
                enemy.IsEnemyHit = false;
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
        StartCoroutine(ActivateUltimateSkillAfterDelay(0.3f));
        StartCoroutine(UltDeactiveCoroutine());
    }

    void UltimateSkillDeactive()
    {
        Managers.Game.gage = 0;
        UltimateSkill_Active = false;
        characterEffect.UltimateEffectDeactive();
    }

    private IEnumerator ActivateUltimateSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UltimateSkill_Active = true;
        characterEffect.UltimateEffectActive();
    }
}
