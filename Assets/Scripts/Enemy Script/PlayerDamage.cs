using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public PlayerHP playerHP;
    private PlayerEffect player;
    private Enemy enemyScript;


    // Start is called before the first frame update
    void Start()
    {
        playerHP = FindObjectOfType<PlayerHP>();
        player = FindObjectOfType<PlayerEffect>();
        enemyScript = FindObjectOfType<Enemy>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(enemyScript.IsEnemyDead) return;

        if(other.CompareTag("Player")&&!Managers.Game.isHit) {
            Debug.Log($"{other.name}");
            Debug.Log("player damage");
            playerHP.GetDamaged(1);
            player = other.GetComponent<PlayerEffect>();
            if (player != null)
            {
                Vector2 knockbackDriection = (player.transform.position - transform.position).normalized;
                player.TakeHit(knockbackDriection);
            }
            else Debug.Log("Playerã�� ����");
        }
    }
}
