using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackpoint;

    public Vector2 attackRange;
    public Vector2 normalAttackRange;
    public Vector2 dashAttackRange;
    public Vector2 jumpAttackRange;
    public LayerMask enemyLayers;
    public int attackDamage = 10;
    private bool isAttacking = false;
    private bool isFacingRight = true;
    private int UltimateDamageUpRate = 100;//100분율 기주
    private bool isAttackOnCooldown = false; // 쿨타임 상태 확인
    public float attackCooldown = 0.5f;      // 쿨타임 지속 시간 (초)
    public void SkillMotionActive(string AttackType)
    {
        if (isAttacking || isAttackOnCooldown)
            return;
        StartCoroutine(PerformAttack(AttackType));
    
    }
    IEnumerator PerformAttack(string AttackType)
    {
        isAttacking = true;
        isAttackOnCooldown = true;
        Animator animator = GetComponent<Animator>();
        if (AttackType == "Attack")
        {
            animator.SetTrigger("BasicAttack");
            attackRange = normalAttackRange;
            isAttackOnCooldown = false;

        }
        else if (AttackType == "DashAttack")
        {
            animator.SetTrigger("DashAttack");
            animator.ResetTrigger("dash");
            attackRange = dashAttackRange;
            yield return new WaitForSeconds(attackCooldown);
            isAttackOnCooldown = false;

        }
        else if (AttackType == "JumpAttack")
        {
            
            animator.SetTrigger("JumpAttack");
            animator.ResetTrigger("jump");
            attackRange = jumpAttackRange;
            yield return new WaitForSeconds(attackCooldown);
            isAttackOnCooldown = false;

        }
        Attack();//데미지적용
        yield return new WaitForSeconds(0.4f);//공격 애니메이션 대기
        isAttacking = false;
    }
    void Attack() {
      
        Collider2D[] hitEnimies = Physics2D.OverlapBoxAll(attackpoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnimies)
        {
            Debug.Log("Hit Enemy: " + enemy.name);
        }
    
    }
    private void OnDrawGizmos()
    {
        if (attackpoint == null|| !isAttacking) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackpoint.position, attackRange);
    }

}
