using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public PlayerHP playerHP;
    private PlayerEffect player;


    bool interval = true; 

    // Start is called before the first frame update
    void Start()
    {
        playerHP = FindObjectOfType<PlayerHP>();
        player = FindObjectOfType<PlayerEffect>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")&&!Managers.Game.isHit) {
           
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

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && interval) {
            Invoke("StillAttacking", 1.5f);
        }
    }

    void StillAttacking() {
        interval = false;

        Debug.Log("player damage");
        playerHP.GetDamaged(1);
        //player = collision.GetComponent<PlayerEffect>();
        if (player != null)
        {
            Vector2 knockbackDriection = (player.transform.position - transform.position).normalized;
            player.TakeHit(knockbackDriection);
        }
        else Debug.Log("Playerã�� ����");

        interval = true;
    }
}
