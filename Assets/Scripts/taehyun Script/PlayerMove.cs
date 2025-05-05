using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float maxspeed = 10f;
    float currentspeed;
    float movedirection;
    bool IsJumping = false;
    public bool IsAttacking = false;
    int IsComboAttacking;
    bool HasDoubleJumped = false;
    bool isDashAttacking = false;
    bool isJumpAttacking = false;
    bool isJumpDashing = false;
    float acceleration = 10f;
    float deceleration = 10f;
    float jumpforce = 25f;
    float enhancedgravity = -40;
    public bool isground = false;
    float dashSpeed = 25f;
    int dashToken = 0;
    public float dashDuration = 0.1f;
    bool HasJumped = false;
    bool isDashing = false;
    bool canDash = true;
    public bool buttonDeactive = false;
    public Animator animator;
    private CameraMove camera;
    private Vector3 initialScale;
    Coroutine currentAction;
    Coroutine dashCoroutine;
    Rigidbody2D rigid;
    public GameObject dashEffect;
    public GameObject dashEffectUlt;
    [SerializeField] private PlayerAttackGeneral attackGeneral;
    // Start is called before the first frame update

    private bool isTouching = false;
    private string currentTouchRegion = ""; // "Left", "Right", ""
    public RectTransform leftButtonRect;
    public RectTransform rightButtonRect;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        camera = FindObjectOfType<CameraMove>();
        animator = GetComponent<Animator>();
        initialScale = transform.localScale;
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        IsComboAttacking = 0;
        dashEffect.SetActive(false);
        dashEffectUlt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Managers.Game.isHit || IsAttacking)
        {
            movedirection = 0;
            animator.SetBool("player_run", false);
            return;
        }

#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseSlide();
#else
    HandleTouchSlide();
#endif
    }

    void HandleMouseSlide()
    {
        if (Input.GetMouseButton(0)) // 마우스를 누르고 있는 중
        {
            Vector2 pointerPos = Input.mousePosition;

            if (RectTransformUtility.RectangleContainsScreenPoint(leftButtonRect, pointerPos))
            {
                if (currentTouchRegion != "Left")
                {
                    currentTouchRegion = "Left";
                    OnLeftButtonDown();
                }
            }
            else if (RectTransformUtility.RectangleContainsScreenPoint(rightButtonRect, pointerPos))
            {
                if (currentTouchRegion != "Right")
                {
                    currentTouchRegion = "Right";
                    OnRightButtonDown();
                }
            }
            else
            {
                if (currentTouchRegion != "")
                {
                    currentTouchRegion = "";
                    ButtonUp();
                }
            }
        }
        else
        {
            if (currentTouchRegion != "")
            {
                currentTouchRegion = "";
                ButtonUp();
            }
        }
    }

    void HandleTouchSlide()
    {
        if (Input.touchCount > 0)
        {
            Vector2 pointerPos = Input.GetTouch(0).position;

            if (RectTransformUtility.RectangleContainsScreenPoint(leftButtonRect, pointerPos))
            {
                if (currentTouchRegion != "Left")
                {
                    currentTouchRegion = "Left";
                    OnLeftButtonDown();
                }
            }
            else if (RectTransformUtility.RectangleContainsScreenPoint(rightButtonRect, pointerPos))
            {
                if (currentTouchRegion != "Right")
                {
                    currentTouchRegion = "Right";
                    OnRightButtonDown();
                }
            }
            else
            {
                if (currentTouchRegion != "")
                {
                    currentTouchRegion = "";
                    ButtonUp();
                }
            }
        }
        else
        {
            if (currentTouchRegion != "")
            {
                currentTouchRegion = "";
                ButtonUp();
            }
        }
    }



    private bool isRunningSFX = false;
    IEnumerator RunSFX()
    {
        if(!isRunningSFX && animator.GetCurrentAnimatorStateInfo(0).IsName("player_run"))
        {
            SoundManager.Instance.SFXPlay("Player Run");
            isRunningSFX = true;
            yield return new WaitForSeconds(0.2f);

            isRunningSFX = false;
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

        if (rigid.velocity.y < 0)
        {
            rigid.velocity += Vector2.up * enhancedgravity * Time.fixedDeltaTime;
        }


    }
    public IEnumerator Jump()
    {

        if (Managers.Game.currentState == GameManager.GameState.Battle)
        {
            isground = false; // 착지 상태 초기화 (Raycast가 정확히 감지되도록)

            if (isJumpDashing)
            {
                rigid.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            }
            else if (isJumpAttacking)
            {
                rigid.velocity = Vector2.zero;
                rigid.velocity = new Vector2(movedirection * maxspeed, 0);
                rigid.AddForce(Vector2.up * jumpforce / 1.5f, ForceMode2D.Impulse);
            }
            else
            {
                rigid.velocity = Vector2.zero;
                rigid.velocity = new Vector2(movedirection * maxspeed, 0);
                rigid.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
                SoundManager.Instance.SFXPlay("Player Jump");
            }
            
            animator.SetTrigger("jump");
            

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
        if (Managers.Game.isHit) return; // 피격상태면 키 안먹기
        if (isground)
        {
            HasJumped = true;
            StartActionCoroutine(Jump());
        }
        else if (!isground && !HasDoubleJumped && !HasJumped)//점프어택 or 점프대쉬 and 점프
        {
            HasDoubleJumped = true;
            StartActionCoroutine(Jump());
        }
        else if (IsComboAttacking == 2 && !HasDoubleJumped)// 점프어택 and 점프대쉬
        {
            StartActionCoroutine(Jump());
        }
        
    }
    public void OnLeftButtonDown()
    {
        if (Managers.Game.isHit) return; // 피격상태면 키 안먹기
        if (Managers.Game.currentState == GameManager.GameState.Battle) // ���� �������� ���� üũ
        {
            movedirection = -1;
            animator.SetBool("player_run", true);
        }

    }

    public void OnRightButtonDown()
    {
        if (Managers.Game.isHit) return; // 피격상태면 키 안먹기
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
            HasJumped = false;
            HasDoubleJumped = false;
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

    public void TriggerDash()
    {
        if (Managers.Game.isHit) return; // 피격상태면 키 안먹기
        if (canDash)
        {
            isJumpDashing = false;
            dashCoroutine = StartCoroutine(Dash());
            animator.SetTrigger("dash");

        }

    }
    public IEnumerator Dash()
    {
        if (!isDashAttacking) // DashAttack이 아닌 경우에만 속도 리셋
            rigid.velocity = Vector2.zero;
        isDashing = true;
        canDash = false;
        float dashDirection = transform.localScale.x > 0 ? 1f : -1f;

        if (isDashAttacking)
        {
            dashEffect.SetActive(false);
            rigid.AddForce(new Vector2(dashDirection * dashSpeed / 1.2f, 0f), ForceMode2D.Impulse);
        }
        else if (isJumpDashing)
        {
            dashEffect.SetActive(false);
            rigid.AddForce(new Vector2(dashDirection * dashSpeed / 1.5f, 0f), ForceMode2D.Impulse);
        }
        else
        {
            StartCoroutine(ShowDashEffect());
            rigid.AddForce(new Vector2(dashDirection * dashSpeed, 0f), ForceMode2D.Impulse);
            SoundManager.Instance.SFXPlay("Dash Attack");
        }
        
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        canDash = true;

    }

    private bool isFinished = false;

    private void OnTriggerStay2D(Collider2D collision)
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
                buttonDeactive = true;
                Managers.Game.GoJump();
                return;

            }
        }

        if (collision.tag == "Final" && !isFinished && Managers.Game.CheckNextRound())
        {
            Time.timeScale = 0;
            Managers.UI.ShowPopUpUI<GameClear>();
            isFinished = true;
        }
    }

    public void setPlayerMove()
    {
        float fixedY = this.gameObject.transform.position.y + 20f;
        this.gameObject.transform.position = new Vector2(-11f, fixedY);
    }
    public void SkillMotionActive(string ComboType)
    {

        StartActionCoroutine(PerformAttack(ComboType));
    }
    IEnumerator PerformAttack(string ComboType)
    {

        if (ComboType == "Attack" && !IsAttacking)
        {
            IsAttacking = true;
            animator.SetTrigger("Attack");
            SoundManager.Instance.SFXPlay("Player Attack");
            yield return new WaitForSeconds(0.583f);
            IsAttacking = false;
        }
        else if (ComboType == "DashAttack" && !isDashAttacking && IsComboAttacking < 2)
        {
            isDashAttacking = true;
            IsComboAttacking++;
            ForceDash();
            animator.SetTrigger("DashAttack");
            SoundManager.Instance.SFXPlay("Dash Attack");
            yield return new WaitForSeconds(0f);
            isDashAttacking = false;
        }
        else if (ComboType == "JumpAttack" && !isJumpAttacking && IsComboAttacking < 2)
        {
            dashEffect.SetActive(false);
            isJumpAttacking = true;
            IsComboAttacking++;
            TriggerJump();
            SoundManager.Instance.SFXPlay("Jump Attack");
            animator.SetTrigger("JumpAttack");
            yield return new WaitForSeconds(0.75f);
            isJumpAttacking = false;

        }
        else if (ComboType == "JumpDash" && !isJumpDashing && IsComboAttacking < 2)
        {
            isJumpDashing = true;
            IsComboAttacking++;
            ForceDash();
            TriggerJump();
            SoundManager.Instance.SFXPlay("Dash Jump");
            animator.SetTrigger("JumpDash");
            yield return new WaitForSeconds(0.667f);
            isJumpDashing = false;

        }
    }
    void StartActionCoroutine(IEnumerator newAction)
    {

        currentAction = StartCoroutine(newAction);
    }
    public void ForceDash()
    {
        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
            dashCoroutine = null;
            isDashing = false;
            canDash = true;
        }
        dashCoroutine = StartCoroutine(Dash());
    }
    IEnumerator ShowDashEffect()
    {  if (attackGeneral.UltimateSkill_Active == false)
        {
            dashEffect.SetActive(true);
            yield return new WaitForSeconds(0.4f); // 이펙트 보여줄 시간
            dashEffect.SetActive(false);
        }
        else if (attackGeneral.UltimateSkill_Active == true)
        {
            dashEffectUlt.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            dashEffectUlt.SetActive(false);


        }
    }

         /*public IEnumerator ForcedAniReset()
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

