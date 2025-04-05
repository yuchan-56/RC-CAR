using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtkAni : MonoBehaviour
{
    public Sprite[] animationSprites; // 애니메이션에 사용할 스프라이트 배열
    public float frameRate = 0.1f; // 프레임 간격 (초 단위)
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
            yield return new WaitForSecondsRealtime(frameRate);
        }

        isAnimating = false;
    }

    public void ResetImage()
    {
        buttonImage.sprite = animationSprites[0];
    }
}
