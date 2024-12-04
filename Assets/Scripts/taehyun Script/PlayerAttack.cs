using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackpoint;
    public Vector2 NormalAttackRange;
    public Vector2 dashAttackRange;
    public Vector2 attackRange;
    public LayerMask enemyLayers;
    public int attackDamage = 10;
    private bool isAttacking = false;
    public void SkillMotionActive(string AttackType)
    {
        if (isAttacking)
            return;
        StartCoroutine(PerformAttack(AttackType));
    
    }
    IEnumerator PerformAttack(string AttackType)
    {
        isAttacking = true;
        Animator animator = GetComponent<Animator>();
        if (AttackType == "Normal")
        {
            animator.SetTrigger("BasicAttack");
            attackRange = NormalAttackRange;
        }
        else if (AttackType == "Dash")
        {
            animator.SetTrigger("DashAttack");
            attackRange = (Vector2)transform.right*dashAttackRange;
        }
        yield return new WaitForSeconds(0.1f);
        Attack();
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }
    void Attack() {
      
        Collider2D[] hitEnimies = Physics2D.OverlapBoxAll(attackpoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnimies)
        {
            Debug.Log("Hit Enemy: " + enemy.name);
        }
    
    }
    private void OnDrawGizmosSelected()
    {
        if (attackpoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackpoint.position, attackRange);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
