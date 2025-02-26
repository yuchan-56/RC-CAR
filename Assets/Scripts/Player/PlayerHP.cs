using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public Image[] HPimage;
    int playerHP = 3;

    void Awake()
    {
        HPimage = GetComponentsInChildren<Image>();
        for (int i = 0; i < HPimage.Length; i++)
        {
            HPimage[i].enabled = true;
        }
    }

    public void GetDamaged(int damagedamount)//damagedamount에 얼마만큼의 데미지를 받았는지 정수값 대입
    {
        switch(damagedamount)
        {
            case 0:
                {
                    break;
                }
            case 1:
                {
                    if (playerHP > 0)
                        {
                        playerHP--;
                        HPimage[playerHP].enabled = false;
                    }
                    break;
                }
            case 2:
                {
                    if (playerHP > 1)
                    {
                        playerHP = playerHP - 2;
                        if(HPimage[2].enabled)
                        {
                            HPimage[2].enabled = false;
                            HPimage[1].enabled = false;
                        }
                        else
                        {
                            HPimage[1].enabled = false;
                            HPimage[0].enabled = false;
                        }
                    }

                    else
                    {
                        playerHP = 0;
                        HPimage[0].enabled = false;
                    }

                    break;
                }
            case 3:
                {
                    for (int i = 2; i >= 0; i--)
                    {
                        HPimage[i].enabled = false;
                    }
                    break;
                }
        }

        if (playerHP == 0)
        {
            Debug.Log("Game Over");
        }
    }

    public void StaminaRecovery()
    {
        if (playerHP == 1)
        {
            playerHP++;
            HPimage[1].enabled = true;
        }

        else if(playerHP == 2)
        {
            playerHP++;
            HPimage[2].enabled = true;
        }

        else if(playerHP == 3)
        {
            Debug.Log("Aleady fully Recovered");
        }

        else
        {
            //empty
        }
    }
}