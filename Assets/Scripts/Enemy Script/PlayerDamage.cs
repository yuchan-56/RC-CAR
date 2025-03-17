using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public PlayerHP playerHP;

    // Start is called before the first frame update
    void Start()
    {
        playerHP = FindObjectOfType<PlayerHP>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")) {
            Debug.Log("player damage");
            playerHP.GetDamaged(1);

            PlayerEffect player = other.GetComponent<PlayerEffect>();
            if (player != null)
            {
                Vector2 knockbackDriection = (player.transform.position - transform.position).normalized;
                player.TakeHit(knockbackDriection);
            }
            else Debug.Log("Playerã�� ����");
        }
    }
}
