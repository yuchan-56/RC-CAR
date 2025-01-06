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
        if (AttackType == "Attack")
        {
            animator.SetTrigger("BasicAttack");
            attackRange = normalAttackRange;
        }
        else if (AttackType == "DashAttack")
        {
            animator.SetTrigger("DashAttack");
            attackRange = dashAttackRange;
        }
        else if (AttackType == "JumpAttack")
        {
            animator.SetTrigger("JumpAttack");
            attackRange = jumpAttackRange;

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
    private void OnDrawGizmos()
    {
        if (attackpoint == null|| !isAttacking) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackpoint.position, attackRange);
    }

    //for ultimate skill by choijeanwoo
    public int UlitmateDamageUp()
    {
        int UlitmateDamage = attackDamage * (UltimateDamageUpRate / 100);
        return UlitmateDamage;
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
