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
    bool IsJumping = false;
    bool IsAttacking = false;
    int IsComboAttacking;
    float acceleration = 8f;
    float deceleration = 8f;
    float jumpforce = 10f;
    bool isground = false;
    float dashSpeed = 15f;
    public float dashDuration =0.5f;
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
        Managers.Game.GameStart();
        rigid = GetComponent<Rigidbody2D>();
        camera = FindObjectOfType<CameraMove>();
        animator = GetComponent<Animator>();
        initialScale = transform.localScale;
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        IsComboAttacking = 0;
        animator.SetBool("GetDamaged", false);
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
    public IEnumerator Jump()
    {
        if (Managers.Game.currentState == GameManager.GameState.Battle)
        {
            IsJumping = true; // 점프 시작
            isground = false; // 착지 상태 초기화 (Raycast가 정확히 감지되도록)
            rigid.velocity = new Vector2(rigid.velocity.x, jumpforce); // 기존 속도 반영
            animator.SetTrigger("jump");
            Debug.Log("Jump");

            // 착지 전까지 점프 불가능
            yield return new WaitForSeconds(0.1f);

            // 착지를 감지할 때까지 대기 (Raycast로 다시 갱신)
            while (!isground)
            {
                yield return null;
            }

            IsJumping = false; // 착지 완료 후 점프 가능
        }
    }

    public void TriggerJump()
    {
        if (IsComboAttacking >=2)
            return;
        else if (IsComboAttacking < 2)
        {
            IsComboAttacking++;
            StartCoroutine(Jump());
        }
        else
        {
            Debug.Log("Cant Jump");
        }
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, LayerMask.GetMask("groundLayer"));
        if (hit.collider != null)
        {
            isground = true;
            IsComboAttacking = 0;
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
        if (IsComboAttacking >=2)
            return;
        if (canDash)
        {
            IsComboAttacking++;
            StartCoroutine(Dash());
            animator.SetTrigger("dash");

        }
    }
    public IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        float dashDirection = transform.localScale.x > 0 ? 1f : -1f;
        rigid.velocity = new Vector2(dashDirection * dashSpeed, rigid.velocity.y);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        canDash = true;
        IsComboAttacking = 0;
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
        // 공격 중이면 새로운 공격 실행하지 않음
        if (IsAttacking || IsComboAttacking>=2)
            yield break;

        IsComboAttacking++;
        if (ComboType == "Attack") 
        {
            IsAttacking = true;
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.3f);
            IsAttacking = false;
        }
        else if (ComboType == "DashAttack")
        {
            TriggerDash();
            animator.SetTrigger("DashAttack");
            yield return 0;
        }
        else if (ComboType == "JumpAttack")
        {
         
            TriggerJump();
            animator.SetTrigger("JumpAttack");
            yield return 0;

        }
        else if (ComboType == "JumpDash")
        {
           
            TriggerJump();
            TriggerDash();
            animator.SetTrigger("JumpDash");
            yield return 0;
        }
    }

   /* public IEnumerator ForcedAniReset()
    {
        if (Managers.Game.SkillAniReset == true)
        {
            IsJumping = false;
            IsAttacking = false;
            IsComboAttacking = false;
            IsComboDashing = false;
            isJumpattacking = false;
            isJumpdashing = false;
            isDashattacking = false;

            animator.ResetTrigger("Attack");
            animator.ResetTrigger("JumpAttack");
            animator.ResetTrigger("DashAttack");
            animator.ResetTrigger("JumpDash");
        }
        yield return null;
        StopCoroutine(PerformAttack(""));
        StopCoroutine(ForcedAniReset());
    }*/

}
