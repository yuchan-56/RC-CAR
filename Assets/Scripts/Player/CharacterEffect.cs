using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : MonoBehaviour
{
    Animator ani;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        spriteRenderer.enabled = false;
        ani.SetBool("UltimateSkill", false);
    }

    public void UltimateEffectActive()
    {
        spriteRenderer.enabled = true;
        ani.SetBool("UltimateSkill", true);
    }

    public void UltimateEffectDeactive()
    {
        spriteRenderer.enabled = false;
        ani.SetBool("UltimateSkill", false);
    }
}
