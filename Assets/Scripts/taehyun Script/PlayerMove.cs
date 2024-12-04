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
    float jumpforce = 10f;
    bool isground = false;
    float dashSpeed = 25f;
    public float dashDuration = 0.2f;
    float dashCoolDown = 1f;
    bool isDashing = false;
    bool canDash = true;

    private CameraMove camera;
    Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        camera = FindObjectOfType<CameraMove>();// CmeraUpdate받기
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            TriggerDash();
        }

    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            movedirection = -1;
        if (Input.GetKey(KeyCode.RightArrow))
            movedirection = 1;
        CheckGround();
        if (!isDashing)
        {
            if (movedirection != 0)
            {
                currentspeed = Mathf.Lerp(currentspeed, movedirection * maxspeed, acceleration * Time.fixedDeltaTime);
            }
            else
            {
                currentspeed = Mathf.Lerp(currentspeed, 0, deceleration * Time.fixedDeltaTime);
            }
            rigid.velocity = new Vector2(currentspeed, rigid.velocity.y);
        }
        
    }
    public void jump()
    {

        if (isground)
        {
            rigid.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            Debug.Log("Jump");
        }
        else Debug.Log("Cant Jump");

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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down,2.5f, LayerMask.GetMask("groundLayer")); // 10f는 캐릭터의 크기 즉, 5/2 = 2.5f
        if (hit.collider != null)
        {
            isground = true;
        }
        else
            isground = false;
    }

    public void TriggerDash() {

        if (canDash && movedirection != 0)
            StartCoroutine(Dash());
    }
   public IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        rigid.velocity = new Vector2(movedirection * dashSpeed, rigid.velocity.y);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.tag == "NextJump")
        {
            Debug.Log("triggerOn");
            float fixedY = this.gameObject.transform.position.y;
            float newX = this.gameObject.transform.position.x+8f;
            this.gameObject.transform.position = new Vector2(newX,fixedY);
            camera.CameraUpdate();
            return;
        }
        if(collision.tag == "Final")
        {
            Managers.UI.ShowPopUpUI<StageInfo>();
        }
    }
}
