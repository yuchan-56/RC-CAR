using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGauage : MonoBehaviour
{
    public Image image;
    public Sprite[] sprites;

    private void Start()
    {
        if (image == null)
            image = GetComponent<Image>();
    }

    private void Update()
    {
        UpdateGuageUI();
    }

    private void UpdateGuageUI()
    {
        switch(Managers.Game.gage)
        {
            case 0:
                {
                    image.sprite = sprites[0];
                    break;
                }
            case 5:
                {
                    image.sprite = sprites[1];
                    break;
                }
            case 10:
                {
                    image.sprite = sprites[2];
                    break;
                }
            case 15:
                {
                    image.sprite = sprites[3];
                    break;
                }
            case 25:
                {
                    image.sprite = sprites[4];
                    break;
                }
            case 40:
                {
                    image.sprite = sprites[5];
                    break;
                }
            case 50:
                {
                    image.sprite = sprites[6];
                    break;
                }
            case 60:
                {
                    image.sprite = sprites[7];
                    break;
                }
            case 65:
                {
                    image.sprite = sprites[8];
                    break;
                }
            case 70:
                {
                    image.sprite = sprites[9];
                    break;
                }
            case 75:
                {
                    image.sprite = sprites[10];
                    break;
                }
            case 80:
                {
                    image.sprite = sprites[11];
                    break;
                }
            case 90:
                {
                    image.sprite = sprites[12];
                    break;
                }
            case 95:
                {
                    image.sprite = sprites[13];
                    break;
                }
            case 100:
                {
                    image.sprite = sprites[14];
                    break;
                }
        }
    }
}
