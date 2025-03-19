using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossJoin : UI_Popup
{
    public Image image;
    private void Start()
    {
        
    }

    public void setBossImage(string boss) // Graphic,Programming,Sound,System
    {
        image.sprite = Resources.Load<Sprite>($"UI/EnemyBoss/{boss}");

    }
}
