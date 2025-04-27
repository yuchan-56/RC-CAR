using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SndP1ToolControl : MonoBehaviour
{
    public GameObject collider0;
    public GameObject collider1;
    public GameObject collider2;

    public void sndP1c0()
    {
        collider0.SetActive(true);
    }

    public void sndP1c1()
    {
        collider0.SetActive(false);
        collider1.SetActive(true);
    }

    public void sndP1c2()
    {
        collider1.SetActive(false);
        collider2.SetActive(true);
    }

    public void sndP1cOff()
    {
        collider0.SetActive(false);
        collider1.SetActive(false);
        collider2.SetActive(false);
    }


}
