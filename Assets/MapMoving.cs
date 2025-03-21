using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
public class MapMoving : UI_Popup
{
    float blackTime = 1.5f;
    public Image Black;
    Camera camera_m;
    private float FixedY = -1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(goBlack());
        camera_m = FindObjectOfType<Camera>();
        
    }
    IEnumerator goBlack()
    {
        Time.timeScale = 0;
        Black.DOFade(1f, blackTime).SetUpdate(true);
        yield return new WaitForSecondsRealtime(blackTime*1.1f);

        FixedY += 20f;
        camera_m.transform.position = new Vector2(-3, FixedY); // 카메라 포지션 지정하기
        FindObjectOfType<PlayerMove>().setPlayerMove(); // 플레이어 움직이기
       camera_m.GetComponent<CameraMove>().FixedY += 20; // 카메라 움직이기
        //여기에 작성
        Managers.Game.SkillAniReset = true;

        Time.timeScale = 1;
        Black.DOFade(0f, blackTime); // DOFadeOut이 올바르게 진행되려면 Time.timeScale이 1이어야함. 왜인지는..? 모름 Complete가 제대로 작동 안하는듯
        yield return new WaitForSecondsRealtime(blackTime);


        Managers.UI.ShowPopUpUI<StageGuide>(); // 맵이동이 끝날때 StageGuide 출력

        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<MapMoving>(this.gameObject));


        yield return null;
    }

}
