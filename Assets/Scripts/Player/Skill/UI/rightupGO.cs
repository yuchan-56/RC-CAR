using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rightupGO : MonoBehaviour
{
    public Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }

    public void ImageAbled()
    {
        image.enabled = true;
    }

    public void ImageDisabled()
    {
        image.enabled = false;
    }
}
