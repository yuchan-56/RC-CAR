using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpAni : MonoBehaviour
{
    public Sprite[] animationSprites;
    public float frameRate = 0.1f;
    private Image buttonImage;
    public bool isAnimating = false;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    public IEnumerator AnimateButton()
    {
        if (isAnimating || animationSprites.Length == 0)
            yield break;

        isAnimating = true;
        for (int i = 0; i < animationSprites.Length; i++)
        {
            buttonImage.sprite = animationSprites[i];
            yield return new WaitForSeconds(frameRate);
        }

        isAnimating = false;
    }
    public void ResetImage()
    {
        buttonImage.sprite = animationSprites[0];
    }
}
