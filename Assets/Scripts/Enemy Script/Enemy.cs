using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 좌우이동 + following player
    public Transform player;
    public float speed = 2f;
    public float wanderDistance = 2f;
    private Vector2 stopPosition;
    private bool isFollowing = false;

    // Shooting
    public GameObject bulletPref;
    public bool shootOn = false;
    float bulletSpeed = 10.0f;
    float spawnInterval = 0.6f;
    float nextSpawn = 0f;
    
    void Start()
    {
        stopPosition = transform.position;
    }

    void Update()
    {
        if (isFollowing)
        {
            FollowPlayer();
            
            if (shootOn && Time.time >= nextSpawn)
            {
                Shooting(player);
                nextSpawn = Time.time + spawnInterval;
            }
        }
        else
        {   
            Wander();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayerTrigger")
        {
            isFollowing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayerTrigger")
        {
            isFollowing = false;
            stopPosition = transform.position;
        }
    }

    void Wander()
    {
        float xPos = Mathf.PingPong(Time.time * speed, 5) - (5 / 2);
        transform.position = Vector2.Lerp(transform.position, new Vector3(stopPosition.x + xPos, transform.position.y), Time.deltaTime * speed);
    }

    void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void Shooting(Transform player)
    {
        GameObject bullet = Instantiate(bulletPref, transform.position, Quaternion.identity);
        Rigidbody2D bulletrb = bullet.GetComponent<Rigidbody2D>();

        if(bulletrb != null) {
            Vector2 direction = (player.position - transform.position).normalized;
            bulletrb.velocity = direction * bulletSpeed;
        }
    }
}
