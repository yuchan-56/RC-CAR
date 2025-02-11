using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComboManager : MonoBehaviour
{
    public DashAttack dashAttack;
    public JumpAttack jumpAttack;
    public CharacterEffect characterEffect;
    public HashSet<string> InputButton;
    public PlayerMove player;
    public PlayerAttackGeneral playerAttackGeneral;
    public PlayerAttackAnimation ani;
    bool AniSetup;
    GameObject CurrentObject;

    List<RaycastResult> raycastResults = new List<RaycastResult>();


    void Start()
    {
        InputButton = new HashSet<string>();
        AniSetup = false;
    }

    void Update()
    {
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
                if(InputButton.Contains("Dash"))
                {
                    
                }

                else if(InputButton.Contains("Jump"))
                {

                }

                else if(InputButton.Contains("Attack"))
                {

                }

                else
                {
                    AniSetup = false;
                }
            }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            switch (InputButton.Count)
            {
                case 1:
                    {
                        if (InputButton.Contains("Jump")) // Jump
                        {
                            Debug.Log("Jump");
                        }

                        else if (InputButton.Contains("Dash")) //Dash
                        {
                            Debug.Log("Dash");
                        }

                        else if (InputButton.Contains("Attack"))
                        {
                            Debug.Log("Attack"); //Atack
                            playerAttackGeneral.AttackSetActive();
                            player.SkillMotionActive("Attack");
                        }

                        break;
                    }

                case 2:
                    {
                        if (InputButton.Contains("Jump") && InputButton.Contains("Dash"))//점프대쉬
                        {
                            Debug.Log("JumpDash active");
                            player.jump();
                            player.TriggerDash();
                            player.SkillMotionActive("JumpDash");

                        }

                        else if (InputButton.Contains("Dash") && InputButton.Contains("Attack"))//대쉬어택
                        {
                            Debug.Log("DashAttack active");
                            player.TriggerDash();
                            dashAttack.SkillMotionActive();
                            player.SkillMotionActive("DashAttack");

                        }

                        else if(InputButton.Contains("Jump") && InputButton.Contains("Attack"))
                        {
                            Debug.Log("JumpAttack active");//점프어택
                            player.jump();

                            jumpAttack.SkillMotionActive();
                            player.SkillMotionActive("JumpAttack");

                        }

                        break;
                    }

                case 3:
                    {
                        if (Managers.Game.gage >= 0 && InputButton.Contains("Jump") && InputButton.Contains("Attack") && InputButton.Contains("Dash"))
                        {
                            Debug.Log("Ultimate skill active");
                            playerAttackGeneral.UltimateSkillActive();
                        }
                        break;
                    }
            }
            AniSetup = false;
            InputButton.Clear();
        }

        else if (Managers.Game.GetHit == true)
        {
            Managers.Game.gage++;
            Debug.Log(Managers.Game.gage);
            Managers.Game.GetHit = false;
        }

    }
}