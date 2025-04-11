using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
public class MapMoving : UI_Popup
{
    public float blackTime = 1.5f;

    //검은색 Panel 4개
    public RectTransform panelTop;
    public RectTransform panelBottom;
    public RectTransform panelLeft;
    public RectTransform panelRight;
    Camera camera_m;
    private float FixedY = -1;

    private bool moving = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("맵이동구현");
        StartCoroutine(goBlack());
        camera_m = FindObjectOfType<Camera>();
        
    }
    IEnumerator goBlack()
    {
        //Time.timeScale = 0;
        moving = true; // 맵이동중 
        StartCoroutine(goFall());  // 떨어질 수 있게 하는 코드

        StartCoroutine(CloseScreen());  // 화면 닫기

        // Black.DOFade(1f, blackTime).SetUpdate(true);
        yield return new WaitForSecondsRealtime(blackTime*1.1f);

        camera_m.GetComponent<CameraMove>().FixedY += 20; // 카메라 움직이기
        FixedY = camera_m.GetComponent<CameraMove>().FixedY;
        camera_m.transform.position = new Vector3(-3, FixedY,-10); // 카메라 포지션 지정하기
        FindObjectOfType<PlayerMove>().setPlayerMove(); // 플레이어 움직이기
      
        moving = false;
        StartCoroutine(OpenScreen());  // 화면 열기
        Time.timeScale = 1;

        // Black.DOFade(0f, blackTime); // DOFadeOut이 올바르게 진행되려면 Time.timeScale이 1이어야함. 왜인지는..? 모름 Complete가 제대로 작동 안하는듯
        yield return new WaitForSecondsRealtime(blackTime);

        
        Managers.Game.SkillAniReset = true;
        Managers.UI.ShowPopUpUI<StageGuide>(); // 맵이동이 끝날때 StageGuide 출력

        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<MapMoving>(this.gameObject));


        yield return null;
    }
    IEnumerator goFall()
    {
        Physics2D.autoSimulation = false; // 수동 물리 엔진
        while (moving)
        {
            Physics2D.Simulate(Time.unscaledDeltaTime);
            yield return null;
        }
        Physics2D.autoSimulation = true;
    }

    IEnumerator CloseScreen()
    {
        float duration = blackTime;

        panelTop.DOAnchorPosY(0, duration).SetUpdate(true);
        panelBottom.DOAnchorPosY(0, duration).SetUpdate(true);
        panelLeft.DOAnchorPosX(0, duration).SetUpdate(true);
        panelRight.DOAnchorPosX(0, duration).SetUpdate(true);

        yield return new WaitForSecondsRealtime(duration * 1.1f);
    }

    IEnumerator OpenScreen()
    {
        float duration = blackTime;

        panelTop.DOAnchorPosY(panelTop.rect.height, duration).SetUpdate(true);
        panelBottom.DOAnchorPosY(-panelBottom.rect.height, duration).SetUpdate(true);
        panelLeft.DOAnchorPosX(-panelLeft.rect.width, duration).SetUpdate(true);
        panelRight.DOAnchorPosX(panelRight.rect.width, duration).SetUpdate(true);

        yield return new WaitForSecondsRealtime(duration * 1.1f);

 

    }

}
