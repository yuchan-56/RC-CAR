using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;

public class StageInfo : UI_Popup
{
    public TMP_Text text;
    public string StageName;
    enum Buttons
    {
        GoStage,
        Close
    }

    private void Start()
    {
        base.Init();
        SetResolution();
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.GoStage).gameObject.AddUIEvent(GoStage);
        GetButton((int)Buttons.Close).gameObject.AddUIEvent(CloseButtonClicked);
        StageName = Managers.Data.Stage;
        text.text = StageName;
    }
    void GoStage(PointerEventData eventData)
    {
        Debug.Log("..");
        Managers.Scene.LoadScene(StageName);
    }
    void CloseButtonClicked(PointerEventData eventData)
    {
        Managers.UI.ClosePopUpUI();
    }
}
