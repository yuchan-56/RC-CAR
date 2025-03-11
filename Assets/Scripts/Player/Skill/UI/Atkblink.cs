using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Atkblink : MonoBehaviour
{
    public Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }

    public void ImageAbled()
    {
        BlinkController.Instance.RegisterBlinkingImage(image);
    }

    public void ImageDisabled()
    {
        BlinkController.Instance.UnregisterBlinkingImage(image);
    }
}
