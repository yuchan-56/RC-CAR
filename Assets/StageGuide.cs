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
        if(Managers.Game.currentGround == GameManager.GameGround.G4 || Managers.Game.currentGround ==GameManager.GameGround.A2) // 보스 조우시 조금만 더 기다렸다가 출력 3f
        {
            Managers.UI.ShowPopUpUI<BossJoin>();
            BossJoin boss = FindObjectOfType<BossJoin>();
            boss.setBossImage("Programming");

            new WaitForSecondsRealtime(3f); //3초대기
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            uiImage.sprite = sprites[i];
            if (i == 9) { StageText.gameObject.SetActive(true);}
            if (i == 10) { yield return new WaitForSecondsRealtime(1f); } // 글씨 출력후 좀더 기다리기
            if(i==15){ StageText.gameObject.SetActive(false); }
        yield return new WaitForSecondsRealtime(0.1f);
            
        }
        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<StageGuide>(this.gameObject));

    }
}
