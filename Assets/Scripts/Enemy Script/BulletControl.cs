using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    void Update()
    {
        //Destroy(gameObject, 3f);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
        }

        if(collision.gameObject.CompareTag("Ground")) {
            Destroy(gameObject);
        }
    }
}
