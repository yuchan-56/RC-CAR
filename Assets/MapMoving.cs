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
        camera_m.transform.position = new Vector2(-3, FixedY); // ī�޶� ������ �����ϱ�
        FindObjectOfType<PlayerMove>().setPlayerMove(); // �÷��̾� �����̱�
       camera_m.GetComponent<CameraMove>().FixedY += 20; // ī�޶� �����̱�

        Time.timeScale = 1;
        Black.DOFade(0f, blackTime); // DOFadeOut�� �ùٸ��� ����Ƿ��� Time.timeScale�� 1�̾����. ��������..? �� Complete�� ����� �۵� ���ϴµ�
        yield return new WaitForSecondsRealtime(blackTime); 

        Managers.UI.ClosePopUpUI();


        yield return null;
    }

}
