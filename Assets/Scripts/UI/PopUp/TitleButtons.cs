using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static LoadingScene;

public class TitleButtons : UI_Popup
{
    enum Buttons
    {
        GoBack,
        Option
    }

    private void Start()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.GoBack).gameObject.AddUIEvent(GoBackClicked);
        GetButton((int)Buttons.Option).gameObject.AddUIEvent(OptionClicked);

        Managers.UI.SetCanvasNumber(this.gameObject, 3); // SortOrder 3·Î
    }
    void GoBackClicked(PointerEventData eventData)
    {
        LoadingScene.Instance.GoLoading("MainTitle");

    }
    void OptionClicked(PointerEventData eventData)
    {
        Managers.UI.ShowPopUpUI<Option>();
    }

}
