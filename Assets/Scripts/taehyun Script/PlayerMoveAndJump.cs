using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermoveandjump : MonoBehaviour
{
    float movespeed = 35f;//gravityscale 4
    float maxspeed = 6f;
    float jumpforce = 16f;
    bool ismovingleft = false;
    bool ismovingright = false;
    bool isground = true;
    Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {


    }
    void FixedUpdate()
    {
        if (ismovingleft && rigid.velocity.x > -maxspeed)
            rigid.AddForce(Vector2.left * movespeed);
        else if (ismovingright && rigid.velocity.x < maxspeed)
            rigid.AddForce(Vector2.right * movespeed);
    }
    public void jump()
    {
        if (isground)
            rigid.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isground = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isground = false;
    }
    public void OnLeftButtonDown()
    {
        ismovingleft = true;
    }
    public void OnRightButtonDown()
    {
        ismovingright = true;
    }
    public void OnLeftButtonUp()
    {
        ismovingleft = false;
    }
    public void OnRightButtonUp()
    {
        ismovingright = false;
    }

}