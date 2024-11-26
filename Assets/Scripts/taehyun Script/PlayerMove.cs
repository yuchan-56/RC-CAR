using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float maxspeed = 7f;
    float currentspeed;
    float movedirection;
    float acceleration = 8f;
    float deceleration = 8f;
    float jumpforce = 8f;
    bool isground = false;
    float dashSpeed = 30f;
    float dashDuration = 0.2f;
    float dashCoolDown = 2f;
    bool isDashing = false;
    bool canDash = true;

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
        if (Input.GetKey(KeyCode.LeftArrow))
            movedirection = -1;
        if (Input.GetKey(KeyCode.RightArrow))
            movedirection = 1;
        CheckGround();
        if (movedirection != 0)
        {
            currentspeed = Mathf.Lerp(currentspeed, movedirection * maxspeed, acceleration * Time.fixedDeltaTime);
        }
        else { 
            currentspeed = Mathf.Lerp(currentspeed, 0, deceleration * Time.fixedDeltaTime);
        }
        rigid.velocity = new Vector2(currentspeed, rigid.velocity.y);
 
        
    }
    public void jump()
    {
        if (isground)
            rigid.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);

    }
    public void OnLeftButtonDown()
    {
        movedirection = -1;

    }

    public void OnRightButtonDown()
    {
        movedirection=1;

    }
    public void ButtonUp()
    {
        movedirection = 0;

    }
    void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down,1f, LayerMask.GetMask("groundLayer"));
        if (hit.collider != null)
        {
            isground = true;
        }
        else
            isground = false;
    }

    public void TriggerDash() {

        if (canDash)
            StartCoroutine(Dash());
    }
   public IEnumerator Dash()
    {

        if (movedirection == 0)
        {
            isDashing = false;
            canDash = true;
            yield break;
        }
        rigid.velocity=new Vector2 (movedirection * dashSpeed, rigid.velocity.y);
        isDashing = true;
        canDash = false;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
}
