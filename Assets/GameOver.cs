using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOver : UI_Popup
{
    enum Buttons
    {
        ReTry
    }

    private void Start()
    {
        Init();
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.ReTry).gameObject.AddUIEvent(ReTryClicked);
        Managers.UI.SetCanvasNumber(this.gameObject, 1000); // SortOrder 위로 설정 
    }
    void ReTryClicked(PointerEventData eventData)
    {
        Managers.Scene.LoadScene(Define.Scene.MainTitle);
    }
}
