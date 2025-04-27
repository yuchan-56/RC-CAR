using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SndAmpControl : MonoBehaviour
{
    public GameObject collider0;
    public GameObject collider1;
    public GameObject collider2;

    public void ampC0()
    {
        collider0.SetActive(true);
    }

    public void ampC1()
    {
        collider0.SetActive(false);
        collider1.SetActive(true);
    }

    public void ampC2()
    {
        collider2.SetActive(false);
        collider2.SetActive(true);
    }

    public void ampCOff()
    {
        collider0.SetActive(false);
        collider1.SetActive(false);
        collider2.SetActive(false);
    }
}
