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
        GetButton((int)Buttons.Close).gameObject.AddUIEvent(CloseButtonClicked);
        StageName = Managers.Data.Stage;

        
        if(StageName =="S1_1")
        {
            text.text = $"월요일 11:50 " +
                $"난이도 하" +
                $"10분 내에 도착";
        }
        else if(StageName =="S1_2")
        {
            text.text = StageName;
        }
        else if(StageName =="S1_3")
        {
            text.text = StageName;
        }
        else if (StageName == "S1_4")
        {
            text.text = StageName;
        }
        else if (StageName == "S1_5")
        {
            text.text = StageName;
        }

        every =  GetComponent<EveryTimeSchedule>();
    }
    void GoStage(PointerEventData eventData)
    {      
        LoadingScene.Instance.GoLoading(StageName);
    }
    void CloseButtonClicked(PointerEventData eventData)
    {
        Managers.UI.ClosePopUpUI();
        Managers.UI.isPopuping = false;
        
    }
}
