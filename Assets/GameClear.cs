using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameClear : UI_Popup
{
    enum Buttons
    {
        ClearButton
    }

    private void Start()
    {
        Init();
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.ClearButton).gameObject.AddUIEvent(GameClearClicked);
    }
    void GameClearClicked(PointerEventData eventData)
    {
        Managers.Scene.LoadScene(Define.Scene.MainTitle);
        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<GameClear>(this.gameObject));
    }
}
