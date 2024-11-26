using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    BoxCollider2D boxCollider2D;
    bool SkillActive_JumpAttack;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        SkillActive_JumpAttack = false;
    }


    public void SkillMotionActive()
    {
        SkillActive_JumpAttack = true;
        boxCollider2D.enabled = true;
        Invoke("SkillMotionDeactive", 1f);
    }

    void SkillMotionDeactive()
    {
        SkillActive_JumpAttack = false;
        boxCollider2D.enabled = false;
    }
}
