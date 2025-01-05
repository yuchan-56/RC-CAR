using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSkill : MonoBehaviour
{
    public PlayerAttack playerAttack;

    public void UltimateActive()
    {
        int UltimateDamage = playerAttack.UlitmateDamageUp();

    }

    public void UltimateDeactive()
    {

    }
}
