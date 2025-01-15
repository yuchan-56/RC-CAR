using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{
    public BoxCollider2D boxCollider2D;
    bool SkillActive_DashAttack;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        SkillActive_DashAttack = false;
    }


    public void SkillMotionActive()
    {
        SkillActive_DashAttack = true;
        boxCollider2D.enabled = true;
        if (SkillActive_DashAttack == true)
        {
            Invoke("SkillMotionDeactive", 1f);
        }
    }

    void SkillMotionDeactive()
    {
        SkillActive_DashAttack = false;
        boxCollider2D.enabled = false;
    }
}
