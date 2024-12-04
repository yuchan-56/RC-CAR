using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack2 : MonoBehaviour
{
    public Transform attackpoint;
    public Vector2 attackRange;
    public LayerMask enemyLayers;
    public PlayerMove playerMove;
    private bool isDashing = false;
    // Start is called before the first frame update

    public void PerformDashAttack()
    {
        isDashing = true;
        StartCoroutine(DashAttackCoroutine());
    
    
    }
    private IEnumerator DashAttackCoroutine()
    {
        playerMove.TriggerDash();
        yield return new WaitForSeconds(playerMove.dashDuration);
        Attack();    
    
    }
    private void Attack()
    {
        Collider2D[] hitenemies = Physics2D.OverlapBoxAll(attackpoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitenemies)
        {
            Debug.Log("Dash Attack Hit Enemy: " + enemy.name);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackpoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackpoint.position, attackRange);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
