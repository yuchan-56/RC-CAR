using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using static LoadingScene;

public class StageInfo : UI_Popup
{
    public TMP_Text text;
    private string StageName;
    EveryTimeSchedule every;
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
        //GetButton((int)Buttons.Close).gameObject.AddUIEvent(CloseButtonClicked);
        StageName = Managers.Data.Stage;

        Managers.UI.SetCanvasNumber(this.gameObject, 4); // SortOrder 4로

        if (StageName =="S1_1")
        {
            text.text = $"월요일 11:50\n\n난이도 하";
        }
        else if(StageName =="S1_2")
        {
            text.text = $"화요일 13:50\n\n난이도 중하";
        }
        else if(StageName =="S1_3")
        {
            text.text = $"수요일 14:50\n\n난이도 중";
        }
        else if (StageName == "S1_4")
        {
            text.text = $"목요일 13:50\n\n난이도 중상";
        }
        else if (StageName == "S1_5")
        {
            text.text = $"금요일 14:50\n\n난이도 상";
        }

        every =  GetComponent<EveryTimeSchedule>();
    }
    void GoStage(PointerEventData eventData)
    {
        SoundManager.Instance.SFXPlay("Button Click");
        LoadingScene.Instance.GoLoading(StageName);
    }
    void CloseButtonClicked(PointerEventData eventData)
    {
        Destroy(this.gameObject);
        Managers.UI.ClosePopUpUI();
        Managers.UI.isPopuping = false;
        
    }
}
