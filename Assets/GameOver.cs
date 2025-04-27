using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static LoadingScene;
using UnityEngine;

public class GameOver : UI_Popup
{
    enum Buttons
    {
        ReTryButton
    }

    private void Start()
    {
        Init();
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.ReTryButton).gameObject.AddUIEvent(ReTryClicked);
        Managers.UI.SetCanvasNumber(this.gameObject, 1000);

        SoundManager.Instance.AudioStop();
        SoundManager.Instance.SFXPlay("Stage Fail");
    }
    void ReTryClicked(PointerEventData eventData)
    {
        Debug.Log("ReTryClicked");
        LoadingScene.Instance.GoLoading("MainTitle");
        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<GameOver>(this.gameObject));
    }
}
