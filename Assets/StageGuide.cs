using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StageGuide : UI_Popup
{
    public Sprite[] sprites;
    public Image uiImage;
    public Image StageText;

    private void Start()
    {
        StartCoroutine(PlaySpriteAnimation());
        Init();
        StageText.sprite = Resources.Load<Sprite>($"UI/classroom/{Managers.Game.currentGround}");

        
    }
    IEnumerator PlaySpriteAnimation()
    {
        if(Managers.Game.currentGround == GameManager.GameGround.G4 || Managers.Game.currentGround ==GameManager.GameGround.A2) // ���� ����� ���ݸ� �� ��ٷȴٰ� ��� 3f
        {

            new WaitForSecondsRealtime(3f); //3�ʴ��
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            uiImage.sprite = sprites[i];
            if (i == 9) { StageText.gameObject.SetActive(true); }
            if (i == 10) { yield return new WaitForSecondsRealtime(1f); } // �۾� ����� ���� ��ٸ���
            if(i==15){ StageText.gameObject.SetActive(false);}
        yield return new WaitForSecondsRealtime(0.1f);
            
        }
        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<StageGuide>(this.gameObject));

    }
}
