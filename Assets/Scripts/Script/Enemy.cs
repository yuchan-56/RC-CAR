using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int speed = 2;
    Vector2 startPos;
    bool follow = false;
    public float followSpeed = 2;
    
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
            }
        } else {
            float xPos = Mathf.PingPong(Time.time * speed, 5) - (5 / 2);
            transform.position = Vector2.Lerp(transform.position, new Vector3(startPos.x + xPos, transform.position.y), Time.deltaTime * speed);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.name == "Player") {
            follow = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            follow = false;
            startPos = transform.position;
        }
    }
}
