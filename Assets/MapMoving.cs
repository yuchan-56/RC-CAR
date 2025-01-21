using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
public class MapMoving : UI_Popup
{
    float blackTime = 1.5f;
    public Image Black;
    public Camera camera;
    private float FixedY = -1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(goBlack());
        camera = FindObjectOfType<Camera>();
        
    }
    IEnumerator goBlack()
    {
        Time.timeScale = 0;
        Debug.Log("ÀÛµ¿");
        Black.DOFade(1f, blackTime).SetUpdate(true).OnComplete(() =>
        {
            FixedY += 20f;
            camera.transform.position = new Vector2(-3, FixedY);
            Black.DOKill();
            Black.DOFade(0f, blackTime).SetUpdate(true).OnComplete(() =>
            {

                Black.DOKill();
                Time.timeScale = 1;
                Managers.UI.ClosePopUpUI();
            });
        });

        yield return null;
    }

}
