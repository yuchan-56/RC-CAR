using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComboManager : MonoBehaviour
{
    public DashAttack dashAttack;
    public JumpAttack jumpAttack;
    public PlayerAttack playerAttack;
    public HashSet<string> InputButton;
    public PlayerMove player;
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
                        if (InputButton.Contains("Button1")) // jump
                        {
                            Debug.Log("skill 1 active");
                            player.jump(); 
                        }

                        else if (InputButton.Contains("Button2"))
                        {
                            Debug.Log("skill 2 active");
                            player.TriggerDash();
                        }

                        else
                        {
                            Debug.Log("skill 3 active");
                            foreach(var button in InputButton)
                            {
                                playerAttack.SkillMotionActive("Attack");
                                Debug.Log(button);

                            }
                        }

                        break;
                    }

                case 2:
                    {
                        if (InputButton.Contains("Button1") && InputButton.Contains("Button2"))//점프+대쉬
                        {
                            Debug.Log("Combo 1 active");
                            
                        }

                        else if (InputButton.Contains("Button2") && InputButton.Contains("Button3"))//대쉬+공격
                        {
                            Debug.Log("Combo 2 active");
                            player.TriggerDash();
                            playerAttack.SkillMotionActive("DashAttack");

                        }

                        else
                        {
                            Debug.Log("Combo 3 active");//점프+공격
                            playerAttack.SkillMotionActive("JumpAttack");
                            player.jump();
                        }

                        break;
                    }

                case 3:
                    {
                        Debug.Log("Combo 4 active");
                        break;
                    }
            }

            InputButton.Clear();
        }
    }
}