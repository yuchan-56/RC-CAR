using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerSkills : MonoBehaviour
{
    PlayerMove player;
    [SerializeField] Button[] buttons;
    void Start()
    {
        player = FindObjectOfType<PlayerMove>();

    }   
     
    void LeftButtonClciked()
    {
       
        player.OnLeftButtonDown();
    }

    void RightButtonClicked()
    {
        player.OnRightButtonDown();
    }

    void JumpButtonClicked()
    {
        player.TriggerJump();
    }
}
