using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

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

        // ¿˙¿Â
        int stage = PlayerPrefs.GetInt("StageData");
        stage++;
        PlayerPrefs.SetInt("StageData", stage);
        PlayerPrefs.Save();
    }
    void GameClearClicked(PointerEventData eventData)
    {
        LoadingScene.Instance.GoLoading("MainTitle");
        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<GameClear>(this.gameObject));
        Managers.Clear();
    }
}
