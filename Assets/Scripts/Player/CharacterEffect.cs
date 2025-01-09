using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : MonoBehaviour
{
    GameObject gameObject;

    void Awake()
    {
        gameObject = GetComponent<GameObject>();
        gameObject.SetActive(false);
    }

    public void UltimmateEffectActive()
    {
        gameObject.SetActive(true);
    }

    public void UltimateEffectDeactive()
    {
        gameObject.SetActive(false);
    }
}
