using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameClear : UI_Popup
{
    enum Buttons
    {
        GameClear
    }

    private void Start()
    {
        Init();
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.GameClear).gameObject.AddUIEvent(GameClearClicked);
    }
    void GameClearClicked(PointerEventData eventData)
    {
        Managers.Scene.LoadScene(Define.Scene.MainTitle);
    }
}
