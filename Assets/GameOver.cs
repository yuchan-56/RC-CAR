using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    }
    void ReTryClicked(PointerEventData eventData)
    {
        Managers.Scene.LoadScene(Define.Scene.MainTitle);
    }
}
