using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rightGO : MonoBehaviour
{
    public Image image;
    public float blinkInterval = 0.3f;
    private bool isBlinking = false;

    void Awake()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }

    public void ImageAbled()
    {
        image.enabled = true;
        if (!isBlinking)
        {
            StartCoroutine(BlinkImage());
        }
    }

    public void ImageDisabled()
    {
        image.enabled = false;
        isBlinking = false;
        StopCoroutine(BlinkImage());
    }

    private IEnumerator BlinkImage()
    {
        isBlinking = true;
        while (isBlinking)
        {
            image.enabled = !image.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
