using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEditor;
using System.Runtime.Serialization.Formatters;

public class ComboManager : MonoBehaviour
{
    public DashAttack dashAttack;
    public JumpAttack jumpAttack;
    public CharacterEffect characterEffect;
    public HashSet<string> InputButton;
    public PlayerMove player;
    public PlayerAttackGeneral playerAttackGeneral;
    public PlayerAttackAnimation ani;
    public AtkAni atkAni;
    public DashAni dashAni;
    public JumpAni jumpAni;
    public rightGO rgo;
    public rightdownGO rdgo;
    public rightupGO rugo;
    public leftGO lgo;
    public leftdownGO ldgo;
    public leftupGO lugo;
    public Jumpblink jumpblink;
    public Dashblink dashblink;
    public Atkblink atkblink;
    private Coroutine dashAniCoroutine;
    private Coroutine jumpAniCoroutine;
    private Coroutine atkAniCoroutine;
    bool AniSetup;
    bool AniSetup2;
    GameObject CurrentObject;

    List<RaycastResult> raycastResults = new List<RaycastResult>();


    void Start()
    {
        InputButton = new HashSet<string>();
        AniSetup = false;
        AniSetup2 = false;
    }

    void Update()
    {
        if(Managers.Game.SkillAniReset == true)
        {
            StopExistingCoroutine(ref atkAniCoroutine);
            StopExistingCoroutine(ref dashAniCoroutine);
            StopExistingCoroutine(ref jumpAniCoroutine);
            lgo.ImageDisabled();
            rgo.ImageDisabled();
            lugo.ImageDisabled();
            ldgo.ImageDisabled();
            rugo.ImageDisabled();
            rdgo.ImageDisabled();
            atkblink.ImageDisabled();
            jumpblink.ImageDisabled();
            dashblink.ImageDisabled();

            atkAni.ResetImage();
            dashAni.ResetImage();
            jumpAni.ResetImage();
            atkAni.isAnimating = false;
            dashAni.isAnimating = false;
            jumpAni.isAnimating = false;

            playerAttackGeneral.AttackSetDeactive();
            jumpAttack.SkillMotionDeactive();
            dashAttack.SkillMotionDeactive();

           // player.ForcedAniReset();
            Managers.Game.SkillAniReset = false;
        }

        else
        {
            //empty
        }

        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = Input.mousePosition;

                raycastResults.Clear();
                EventSystem.current.RaycastAll(pointerData, raycastResults);

                if (raycastResults.Count > 0)
                {
                    CurrentObject = raycastResults[0].gameObject;
                }

                InputButton.Add(CurrentObject.name);
            }

            if (InputButton.Count == 1 && AniSetup == false)
            {
                AniSetup = true;
                if (InputButton.Contains("Dash"))
                {
                    if (dashAni != null)
                    {
                        rgo.ImageAbled();
                        rugo.ImageAbled();
                        jumpblink.ImageAbled();
                        atkblink.ImageAbled();
                        StopExistingCoroutine(ref dashAniCoroutine);
                        dashAniCoroutine = StartCoroutine(dashAni.AnimateButton());
                    }
                }
                else if (InputButton.Contains("Jump"))
                {
                    if (jumpAni != null)
                    {
                        ldgo.ImageAbled();
                        rdgo.ImageAbled();
                        atkblink.ImageAbled();
                        dashblink.ImageAbled();
                        StopExistingCoroutine(ref jumpAniCoroutine);
                        jumpAniCoroutine = StartCoroutine(jumpAni.AnimateButton());
                    }
                }
                else if (InputButton.Contains("Attack"))
                {
                    if (atkAni != null)
                    {
                        lgo.ImageAbled();
                        lugo.ImageAbled();
                        dashblink.ImageAbled();
                        jumpblink.ImageAbled();
                        StopExistingCoroutine(ref atkAniCoroutine);
                        atkAniCoroutine = StartCoroutine(atkAni.AnimateButton());
                    }
                }
                else
                {
                    AniSetup = false;
                    InputButton.Clear();
                }
            }

            else if (InputButton.Count == 2 && AniSetup && AniSetup2 == false)
            {
                AniSetup2 = true;
                lgo.ImageDisabled();
                rgo.ImageDisabled();
                lugo.ImageDisabled();
                ldgo.ImageDisabled();
                rugo.ImageDisabled();
                rdgo.ImageDisabled();
                if (InputButton.Contains("Jump") && InputButton.Contains("Dash"))
                {
                    if (dashAniCoroutine != null)
                    {
                        jumpblink.ImageDisabled();
                        jumpAniCoroutine = StartCoroutine(jumpAni.AnimateButton());
                        rdgo.ImageAbled();
                    }

                    else if (jumpAniCoroutine != null)
                    {
                        dashblink.ImageDisabled();
                        dashAniCoroutine = StartCoroutine(dashAni.AnimateButton());
                        rgo.ImageAbled();
                    }
                }

                else if (InputButton.Contains("Dash") && InputButton.Contains("Attack"))
                {
                    if (dashAniCoroutine != null)
                    {
                        atkblink.ImageDisabled();
                        atkAniCoroutine = StartCoroutine(atkAni.AnimateButton());
                        lugo.ImageAbled();
                    }

                    else if (atkAniCoroutine != null)
                    {
                        dashblink.ImageDisabled();
                        dashAniCoroutine = StartCoroutine(dashAni.AnimateButton());
                        rugo.ImageAbled();
                    }
                }

                else if (InputButton.Contains("Jump") && InputButton.Contains("Attack"))
                {
                    if (jumpAniCoroutine != null)
                    {
                        atkblink.ImageDisabled();
                        atkAniCoroutine = StartCoroutine(atkAni.AnimateButton());
                        lgo.ImageAbled();
                    }

                    else if (atkAniCoroutine != null)
                    {
                        jumpblink.ImageDisabled();
                        jumpAniCoroutine = StartCoroutine(jumpAni.AnimateButton());
                        ldgo.ImageAbled();
                    }
                }

                else
                {
                    AniSetup = false;
                    AniSetup2 = false;
                    InputButton.Clear();
                }
            }
        }

        else if(Time.timeScale == 0f)
        {
            StopExistingCoroutine(ref atkAniCoroutine);
            StopExistingCoroutine(ref dashAniCoroutine);
            StopExistingCoroutine(ref jumpAniCoroutine);
            lgo.ImageDisabled();
            rgo.ImageDisabled();
            lugo.ImageDisabled();
            ldgo.ImageDisabled();
            rugo.ImageDisabled();
            rdgo.ImageDisabled();
            atkblink.ImageDisabled();
            jumpblink.ImageDisabled();
            dashblink.ImageDisabled();

            atkAni.ResetImage();
            dashAni.ResetImage();
            jumpAni.ResetImage();
            atkAni.isAnimating = false;
            dashAni.isAnimating = false;
            jumpAni.isAnimating = false;
        }

        else if (Input.GetMouseButtonUp(0) && AniSetup == true)
        {
            Debug.Log("마우스 업 감지됨");

            if (Managers.Game.isHit)
            {
                return;
            }
            Debug.Log("HashSet Contents: " + string.Join(", ", InputButton));

            StopExistingCoroutine(ref atkAniCoroutine);
            StopExistingCoroutine(ref dashAniCoroutine);
            StopExistingCoroutine(ref jumpAniCoroutine);
            lgo.ImageDisabled();
            rgo.ImageDisabled();
            lugo.ImageDisabled();
            ldgo.ImageDisabled();
            rugo.ImageDisabled();
            rdgo.ImageDisabled();
            atkblink.ImageDisabled();
            jumpblink.ImageDisabled();
            dashblink.ImageDisabled();

            atkAni.ResetImage();
            dashAni.ResetImage();
            jumpAni.ResetImage();
            atkAni.isAnimating = false;
            dashAni.isAnimating = false;
            jumpAni.isAnimating = false;

            if (player.IsAttacking)
            {
                return;

            }

            switch (InputButton.Count)
            {
                case 1:
                    if (InputButton.Contains("Jump"))
                    {
                        Debug.Log("Jump");
                    }
                    else if (InputButton.Contains("Dash"))
                    {
                        Debug.Log("Dash");
                    }
                    else if (InputButton.Contains("Attack"))
                    {
                        Debug.Log("Attack");
                        playerAttackGeneral.AttackSetActive();
                        player.SkillMotionActive("Attack");
                    }
                    break;

                case 2:
                    if (InputButton.Contains("Jump") && InputButton.Contains("Dash"))
                    {

                        Debug.Log("JumpDash active");
                       
                        player.SkillMotionActive("JumpDash");

                    }
                    else if (InputButton.Contains("Dash") && InputButton.Contains("Attack"))
                    {
                        Debug.Log("DashAttack active");
                        
                        dashAttack.SkillMotionActive();
                        player.SkillMotionActive("DashAttack");
                    }
                    else if (InputButton.Contains("Jump") && InputButton.Contains("Attack"))
                    {
                        Debug.Log("JumpAttack active");
                    
                        jumpAttack.SkillMotionActive();
                        player.SkillMotionActive("JumpAttack");
                    }
                    break;

                case 3:
                    if (Managers.Game.gage >= 0 && InputButton.Contains("Jump") && InputButton.Contains("Attack") && InputButton.Contains("Dash"))
                    {
                        Debug.Log("Ultimate skill active");
                        playerAttackGeneral.UltimateSkillActive();
                    }
                    break;
            }

            AniSetup = false;
            AniSetup2 = false;
            InputButton.Clear();
        }

        else if (Managers.Game.GetHit == true)
        {
            Managers.Game.gage++;
            Debug.Log(Managers.Game.gage);
            Managers.Game.GetHit = false;
        }
    }

    private void StopExistingCoroutine(ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}
