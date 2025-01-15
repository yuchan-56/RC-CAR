using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : MonoBehaviour
{
    Animator ani;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        
        ani = GetComponent<Animator>();
        ani.SetBool("UltimateSkill", false);
    }

    public void UltimateEffectActive()
    {
        
        ani.SetBool("UltimateSkill", true);
    }

    public void UltimateEffectDeactive()
    {
        ani.SetBool("UltimateSkill", false);
    }
}
