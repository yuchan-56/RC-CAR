using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class StageInfo : UI_Popup
{
    enum Buttons
    {
        GoStage
    }

    private void Start()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.GoStage).gameObject.AddUIEvent(GoStage);
    }
    void GoStage(PointerEventData eventData)
    {

    }
}
