using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkController : MonoBehaviour
{
    public static BlinkController Instance;

    public float blinkInterval = 0.3f;
    private bool isBlinking = false;
    private List<Image> blinkingImages = new List<Image>();
    private bool isCurrentlyVisible = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void RegisterBlinkingImage(Image img)
    {
        if (!blinkingImages.Contains(img))
        {
            blinkingImages.Add(img);
            img.enabled = isCurrentlyVisible;
        }

        if (!isBlinking)
        {
            StartCoroutine(BlinkImages());
        }
    }

    public void UnregisterBlinkingImage(Image img)
    {
        if (blinkingImages.Contains(img))
        {
            blinkingImages.Remove(img);
            img.enabled = false;
        }

        if (blinkingImages.Count == 0)
        {
            isBlinking = false;
            StopAllCoroutines();
        }
    }

    private IEnumerator BlinkImages()
    {
        isBlinking = true;
        while (isBlinking)
        {
            isCurrentlyVisible = !isCurrentlyVisible;
            foreach (var img in blinkingImages)
            {
                img.enabled = isCurrentlyVisible;
            }
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
