using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComboManager : MonoBehaviour
{
    public HashSet<string> InputButton;

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

        else if(Input.GetMouseButtonUp(0))
        {
            switch (InputButton.Count)
            {
                case 1:
                    {
                        if (InputButton.Contains("Button1"))
                        {
                            Debug.Log("skill 1 active");
                        }

                        else if (InputButton.Contains("Button2"))
                        {
                            Debug.Log("skill 2 active");
                        }
                        else
                        {
                            Debug.Log("skill 3 active");
                        }

                        break;
                    }

                case 2:
                    {
                        if (InputButton.Contains("Button1") && InputButton.Contains("Button2"))
                        {
                            Debug.Log("Combo 1 active");
                        }

                        else if (InputButton.Contains("Button2") && InputButton.Contains("Button3"))
                        {
                            Debug.Log("Combo 2 active");
                        }

                        else
                        {
                            Debug.Log("Combo 3 active");
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