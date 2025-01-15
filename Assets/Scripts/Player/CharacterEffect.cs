using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : MonoBehaviour
{
    Animator ani;

    void Awake()
    {
        ani = GetComponent<Animator>();
        ani.SetBool("UltimateSkill", false);
    }

    public void UltimmateEffectActive()
    {
        ani.SetBool("UltimateSkill", true);
    }

    public void UltimateEffectDeactive()
    {
        ani.SetBool("UltimateSkill", false);
    }
}
