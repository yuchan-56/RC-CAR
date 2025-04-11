using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
public class MapMoving : UI_Popup
{
    float blackTime = 1.5f;

    //������ Panel 4��
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
        Debug.Log("���̵�����");
        StartCoroutine(goBlack());
        camera_m = FindObjectOfType<Camera>();
        
    }
    IEnumerator goBlack()
    {
        Time.timeScale = 0;
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

        // Black.DOFade(0f, blackTime); // DOFadeOut�� �ùٸ��� ����Ƿ��� Time.timeScale�� 1�̾����. ��������..? �� Complete�� ����� �۵� ���ϴµ�
        yield return new WaitForSecondsRealtime(blackTime);

        Time.timeScale = 1;
        Managers.Game.SkillAniReset = true;
        Managers.UI.ShowPopUpUI<StageGuide>(); // ���̵��� ������ StageGuide ���

        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<MapMoving>(this.gameObject));


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
