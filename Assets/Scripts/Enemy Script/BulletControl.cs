using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    void Update()
    {
        Destroy(gameObject, 4f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player") {
            Destroy(gameObject);
        }
    }
}
