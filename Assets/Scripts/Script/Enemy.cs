using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
     // 좌우이동 + following player
    public int speed = 2;
    Vector2 startPos;
    bool follow = false;
    public float followSpeed = 2;

    // Shooting
    public GameObject bulletPref;
    public bool canShoot = false;
    float bulletSpeed = 10.0f;
    float spawnInterval = 0.6f;
    float nextSpawn = 0f;
    
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (follow)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                Vector2 targetPos = player.transform.position;
                transform.position = Vector2.MoveTowards(transform.position, targetPos, followSpeed * Time.deltaTime);

                // Shooting
                if(canShoot && Time.time >= nextSpawn)
                {
                    Shooting(player.transform);
                    nextSpawn = Time.time + spawnInterval;
                }
            }
        }
        else {
            float xPos = Mathf.PingPong(Time.time * speed, 5) - (5 / 2);
            transform.position = Vector2.Lerp(transform.position, new Vector3(startPos.x + xPos, transform.position.y), Time.deltaTime * speed);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.name == "Player") {
            follow = true;
            canShoot = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            follow = false;
            canShoot = false;
            startPos = transform.position;
        }
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
