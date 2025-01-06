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
    private Vector2 initialScale;
    public Animator animator;
    private CameraMove camera;
    Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        Managers.Game.GameStart(); // Player가 들어오면 게임시작으로 간주.
        rigid = GetComponent<Rigidbody2D>();
        camera = FindObjectOfType<CameraMove>();// CmeraUpdate받기
        initialScale = transform.localScale;
        animator = GetComponent<Animator>(); 
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
        { 
            movedirection = -1;
            animator.SetBool("player_run", true);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movedirection = 1;
            animator.SetBool("player_run", true);
        }
        CheckGround();
        if (!isDashing)
        {
            if (movedirection != 0)
            {
                currentspeed = Mathf.Lerp(currentspeed, movedirection * maxspeed, acceleration * Time.fixedDeltaTime);
                FlipUsingScale(movedirection);
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

        if (isground&&Managers.Game.currentState == GameManager.GameState.Battle) // 현재 전투가능 여부 체크)
        {
            rigid.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            Debug.Log("Jump");
            animator.SetTrigger("jump");

        }
        else Debug.Log("Cant Jump");

    }
    public void OnLeftButtonDown()
    {
        if (Managers.Game.currentState == GameManager.GameState.Battle) // 현재 전투가능 여부 체크
        {
            movedirection = -1;
            animator.SetBool("player_run", true);
        }

    }

    public void OnRightButtonDown()
    {
        if (Managers.Game.currentState == GameManager.GameState.Battle) // 현재 전투가능 여부 체크
        {
            movedirection = 1;
            animator.SetBool("player_run", true);
        }
    }
    public void ButtonUp()
    {
            movedirection = 0;
        animator.SetBool("player_run", false);
    }
    void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down,2.5f, LayerMask.GetMask("groundLayer")); // 10f는 캐릭터의 크기 즉, 5/2 = 2.5f
        if (hit.collider != null)
        {
            isground = true;
        }
        else
        {
            isground = false;
            
        }
    }
    private void FlipUsingScale(float direction)
    {
        transform.localScale = new Vector2(direction > 0 ? Mathf.Abs(initialScale.x) : -Mathf.Abs(initialScale.x), initialScale.y); 
    }

    public void TriggerDash() {

        if (canDash && movedirection != 0)
        { 
            StartCoroutine(Dash());
            animator.SetTrigger("dash");

        }
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
        if (collision.tag == "NextJumpRight")
        {
            Debug.Log("triggerOn");
            float fixedY = this.gameObject.transform.position.y;
            float newX = this.gameObject.transform.position.x+8f;
            this.gameObject.transform.position = new Vector2(newX,fixedY);
            camera.CameraUpdate(newX);
            return;
        }
        else if(collision.tag == "NextJumpUp")
        {
            Debug.Log("triggerOn");

            float fixedY = this.gameObject.transform.position.y + 20f;
            float newX = this.gameObject.transform.position.x ;
            this.gameObject.transform.position = new Vector2(newX, fixedY);
          
            camera.CameraGoUp();//테스트
            return;
        }

        if(collision.tag == "Final")
        {
            Managers.UI.ShowPopUpUI<StageInfo>();
        }
    }
}
