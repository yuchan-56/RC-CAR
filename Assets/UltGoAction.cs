using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltGoAction : UI_Popup
{
    public Sprite[] sprites;
    public Image uiImage;

    void Start()
    {
        StartCoroutine(PlaySpriteAnimation());
    }

    IEnumerator PlaySpriteAnimation()
    {
        Time.timeScale = 0;
        for (int i = 0; i < sprites.Length; i++)
        {
            uiImage.sprite = sprites[i];
            yield return new WaitForSecondsRealtime(0.1f);
        }
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1;
        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<UltGoAction>(this.gameObject));
    }
}
