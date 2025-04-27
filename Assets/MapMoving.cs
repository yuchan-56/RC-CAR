using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
public class MapMoving : UI_Popup
{
    public float blackTime = 2f;

    //������ Panel 4��
    public RectTransform panelTop;
    public RectTransform panelBottom;
    public RectTransform panelLeft;
    public RectTransform panelRight;
    public PlayerMove playerMove;
    Camera camera_m;
    private float FixedY = -1;

    public bool moving = false;
    // Start is called before the first frame update
    void Start()
    {
        if(playerMove == null)
        {
            playerMove = FindFirstObjectByType<PlayerMove>();
        }
        
        StartCoroutine(goBlack());
        camera_m = FindObjectOfType<Camera>();
        
        panelTop.sizeDelta = new Vector2(Screen.width, Screen.height);
        panelBottom.sizeDelta = new Vector2(Screen.width, Screen.height);
        panelLeft.sizeDelta = new Vector2(Screen.width, Screen.height);
        panelRight.sizeDelta = new Vector2(Screen.width, Screen.height);
    }
    IEnumerator goBlack()
    {
        //Time.timeScale = 0;
        moving = true; // ���̵��� 
        StartCoroutine(goFall());  // ������ �� �ְ� �ϴ� �ڵ�

        StartCoroutine(CloseScreen());  // ȭ�� �ݱ�

        // Black.DOFade(1f, blackTime).SetUpdate(true);
        yield return new WaitForSecondsRealtime(blackTime*1.1f);

        camera_m.GetComponent<CameraMove>().FixedY += 20; // ī�޶� �����̱�
        FixedY = camera_m.GetComponent<CameraMove>().FixedY;
        camera_m.transform.position = new Vector3(-3, FixedY,-10); // ī�޶� ������ �����ϱ�
        FindObjectOfType<PlayerMove>().setPlayerMove(); // �÷��̾� �����̱�
      
        moving = false;
        StartCoroutine(OpenScreen());  // ȭ�� ����
        Time.timeScale = 1;

        // Black.DOFade(0f, blackTime); // DOFadeOut�� �ùٸ��� ����Ƿ��� Time.timeScale�� 1�̾����. ��������..? �� Complete�� ����� �۵� ���ϴµ�
        yield return new WaitForSecondsRealtime(blackTime);

        
        Managers.Game.SkillAniReset = true;
        Managers.UI.ShowPopUpUI<StageGuide>(); // ���̵��� ������ StageGuide ���

        playerMove.buttonDeactive = false;
        Destroy(this.gameObject);


        yield return null;
    }
    IEnumerator goFall()
    {
        Physics2D.autoSimulation = false; // ���� ���� ����
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
        SoundManager.Instance.SFXPlay("Map Transition");

        //panelTop.DOAnchorPosY(-panelTop.rect.height, duration).SetUpdate(true);
        //panelBottom.DOAnchorPosY(panelTop.rect.height, duration).SetUpdate(true);
        panelLeft.DOAnchorPosX(-panelLeft.rect.width / 2, duration).SetUpdate(true);
        panelRight.DOAnchorPosX(panelRight.rect.width / 2, duration).SetUpdate(true);

        yield return new WaitForSecondsRealtime(duration * 1.1f);
    }

    IEnumerator OpenScreen()
    {
        float duration = blackTime;
        SoundManager.Instance.SFXPlay("Map Transition");
        if(Managers.Game.currentGround==GameManager.GameGround.G4||
            Managers.Game.currentGround == GameManager.GameGround.A2)
            SoundManager.Instance.AudioPlay("Boss");
        if (Managers.Game.currentGround == GameManager.GameGround.Q3 ||
            Managers.Game.currentGround == GameManager.GameGround.C3)
            SoundManager.Instance.AudioPlay("Normal");

        //panelTop.DOAnchorPosY(0, duration).SetUpdate(true);
        //panelBottom.DOAnchorPosY(0, duration).SetUpdate(true);
        panelLeft.DOAnchorPosX(-panelLeft.rect.width, duration).SetUpdate(true);
        panelRight.DOAnchorPosX(panelRight.rect.width, duration).SetUpdate(true);

        yield return new WaitForSecondsRealtime(duration * 1.1f);

 

    }

}
