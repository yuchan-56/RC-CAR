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
    GameObject CurrentObject;

    List<RaycastResult> raycastResults = new List<RaycastResult>();


    void Start()
    {
        InputButton = new HashSet<string>();
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

                            player.SkillMotionActive("DashAttack");

                        }

                        else
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
                        Debug.Log("Ultimate skill active");
                        playerAttackGeneral.UltimateSkillActive();
                        break;
                    }
            }

            InputButton.Clear();
        }
    }
}