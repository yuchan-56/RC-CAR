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
    }
    void ReTryClicked(PointerEventData eventData)
    {
        Managers.Scene.LoadScene(Define.Scene.MainTitle);
    }
}
