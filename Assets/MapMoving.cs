using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
public class MapMoving : UI_Popup
{
    float blackTime = 1.5f;
    public Image Black;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(goBlack());
    }
    IEnumerator goBlack()
    {
        Debug.Log("ÀÛµ¿");
        Black.DOFade(1f, blackTime).OnComplete(() =>
        {
            Debug.Log("OnComplete");
            Black.DOKill();
            Black.DOFade(0f, blackTime).OnComplete(() =>
            {

                Black.DOKill();
                Managers.UI.ClosePopUpUI();
            });
        });

        yield return null;
    }

}
