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
    float dashSpeed = 20f;
    public float dashDuration = 1f;
    float dashCoolDown = 0.1f;
    bool isDashing = false;
    bool canDash = true;
    public Animator animator;
    private CameraMove camera;
    private Vector3 initialScale;
    Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        Managers.Game.GameStart(); // Player�� ������ ���ӽ������� ����.
        rigid = GetComponent<Rigidbody2D>();
        camera = FindObjectOfType<CameraMove>();// CmeraUpdate�ޱ�
        animator = GetComponent<Animator>();
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            TriggerDash();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movedirection = -1;
            animator.SetBool("player_run", true);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movedirection = 1;
            animator.SetBool("player_run", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            movedirection = 0;
            animator.SetBool("player_run", false);
        }


    }
    void FixedUpdate()
    {
       
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

        if (isground && Managers.Game.currentState == GameManager.GameState.Battle) // ���� �������� ���� üũ)
        {
            rigid.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            Debug.Log("Jump");
            animator.SetTrigger("jump");

        }
        else Debug.Log("Cant Jump");

    }
    public void OnLeftButtonDown()
    {
        if (Managers.Game.currentState == GameManager.GameState.Battle) // ���� �������� ���� üũ
        {
            movedirection = -1;
            animator.SetBool("player_run", true);
        }

    }

    public void OnRightButtonDown()
    {
        if (Managers.Game.currentState == GameManager.GameState.Battle) // ���� �������� ���� üũ
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2.5f, LayerMask.GetMask("groundLayer")); // 10f�� ĳ������ ũ�� ��, 5/2 = 2.5f
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

        if (tag != "Player")
        {

            return; 
        }


        if (collision.tag == "NextJumpUp")
        {
            if (Managers.Game.CheckNextRound()) // 적이 모두 처치되었다면.
            {
                camera.CameraGoUp();
                Managers.UI.ShowPopUpUI<MapMoving>();
                Managers.Game.GoJump();
                return;
              
            }
        }

        if (collision.tag == "Final")
        {
            Managers.UI.ShowPopUpUI<StageInfo>();
        }
    }

    public void setPlayerMove()
    {
        float fixedY = this.gameObject.transform.position.y + 20f;
        this.gameObject.transform.position = new Vector2(-11f, fixedY);
    }
    public void SkillMotionActive(string ComboType)
    {
        StartCoroutine(PerformAttack(ComboType));
    }
    IEnumerator PerformAttack(string ComboType)
    {
        if (ComboType == "Attack")
        {
            animator.SetTrigger("Attack");
            yield return null;
        }
        else if (ComboType == "DashAttack")
        {
            animator.SetTrigger("DashAttack");
            yield return null;
        }
        else if (ComboType == "JumpAttack")
        {

            animator.SetTrigger("JumpAttack");
            yield return null;

        }
        else if (ComboType == "JumpDash")
        {
            animator.SetTrigger("JumpDash");
            yield return null;
        }

    }
}
