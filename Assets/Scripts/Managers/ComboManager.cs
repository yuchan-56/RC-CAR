using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEditor;
using System.Runtime.Serialization.Formatters;
using UnityEditor.Experimental.GraphView;

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
    public MapMoving mapMoving;
    private Coroutine dashAniCoroutine;
    private Coroutine jumpAniCoroutine;
    private Coroutine atkAniCoroutine;
    bool AniSetup;
    bool AniSetup2;
    GameObject CurrentObject;

    List<RaycastResult> raycastResults = new List<RaycastResult>();
    Dictionary<int, string> validTouchButtons = new();


    void Start()
    {
        InputButton = new HashSet<string>();
        AniSetup = false;
        AniSetup2 = false;
    }

    void Update()
    {

        if (Managers.Game.SkillAniReset == true || player.buttonDeactive == true)
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

        else if (Managers.Game.isHit)
        {
            playerAttackGeneral.AttackSetDeactive();
            dashAttack.SkillMotionDeactive();
            jumpAttack.SkillMotionDeactive();
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
            InputButton.Clear();
            AniSetup = false;
            AniSetup2 = false;
        }
        else
        {
            //empty
        }

        if (Input.touchCount > 0)
        {

            InputButton.Clear();

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    PointerEventData pointerData = new PointerEventData(EventSystem.current);
                    pointerData.position = touch.position;

                    raycastResults.Clear();
                    EventSystem.current.RaycastAll(pointerData, raycastResults);

                    if (raycastResults.Count > 0)
                    {
                        var hitName = raycastResults[0].gameObject.name;

                        if (hitName != "Left" && hitName != "Right")
                        {
                            validTouchButtons[touch.fingerId] = hitName;
                        }

                        if(Input.touchCount == 1)
                        {
                            if (hitName == "Left" || hitName == "Right")
                            {
                                validTouchButtons.Clear();
                                InputButton.Clear();
                                SkillReset();
                            }
                        }
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    PointerEventData pointerData = new PointerEventData(EventSystem.current);
                    pointerData.position = touch.position;

                    raycastResults.Clear();
                    EventSystem.current.RaycastAll(pointerData, raycastResults);

                    string hitName = "None";

                    if (raycastResults.Count > 0)
                    {
                        hitName = raycastResults[0].gameObject.name;
                    }

                    if (validTouchButtons.TryGetValue(touch.fingerId, out string buttonName))
                    {
                        if (hitName != "Left" && hitName != "Right")
                        {
                            InputButton.Add(buttonName);
                            ExecuteSkill();
                        }

                        validTouchButtons.Remove(touch.fingerId);
                    }
                }
            }
        }

        else if(Input.touchCount == 0)
        {
            validTouchButtons.Clear();
            InputButton.Clear();
            SkillReset();
        }
    }

    private void SkillReset()
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

    private void ExecuteSkill()
    {
        InputButton.Remove("Left");
        InputButton.Remove("Right");

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

        else if (Time.timeScale == 0f)
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

        if (Managers.Game.isHit || player.buttonDeactive)
        {
            return;
        }

        playerAttackGeneral.AttackSetDeactive();
        dashAttack.SkillMotionDeactive();
        jumpAttack.SkillMotionDeactive();
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
            InputButton.Clear();
            return;
        }

        switch (InputButton.Count)
        {
            case 1:
                if (InputButton.Contains("Attack"))
                {
                    playerAttackGeneral.AttackSetActive();
                    player.SkillMotionActive("Attack");
                }
                break;

            case 2:
                if (InputButton.Contains("Jump") && InputButton.Contains("Dash"))
                {
                    player.SkillMotionActive("JumpDash");
                }
                else if (InputButton.Contains("Dash") && InputButton.Contains("Attack"))
                {
                    dashAttack.SkillMotionActive();
                    player.SkillMotionActive("DashAttack");
                }
                else if (InputButton.Contains("Jump") && InputButton.Contains("Attack"))
                {
                    jumpAttack.SkillMotionActive();
                    player.SkillMotionActive("JumpAttack");
                }
                break;

            case 3:
                if (Managers.Game.gage >= 100 &&
                    InputButton.Contains("Jump") &&
                    InputButton.Contains("Attack") &&
                    InputButton.Contains("Dash") &&
                    playerAttackGeneral.UltimateSkill_Active == false)
                {
                    playerAttackGeneral.UltimateSkillActive();
                }
                break;
        }

        AniSetup = false;
        AniSetup2 = false;
        InputButton.Clear();
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
