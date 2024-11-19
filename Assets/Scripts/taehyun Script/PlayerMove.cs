using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float movespeed = 30f;//gravityscale 2
    float maxspeed = 5f;
    float jumpforce = 10f;
    bool ismovingleft = false;
    bool ismovingright = false;
    bool isground = false;
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
        CheckGround();
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
    void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down,0.2f, LayerMask.GetMask("groundLayer"));
        if (hit.collider != null)
        {
            isground = true;
        }
        else
            isground = false;
    }
}
