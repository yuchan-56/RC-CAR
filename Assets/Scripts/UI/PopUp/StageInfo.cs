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

        Managers.UI.SetCanvasNumber(this.gameObject, 4); // SortOrder 4��

        if (StageName =="S1_1")
        {
            text.text = $"������ 11:50\n\n���̵� ��";
        }
        else if(StageName =="S1_2")
        {
            text.text = $"ȭ���� 13:50\n\n���̵� ����";
        }
        else if(StageName =="S1_3")
        {
            text.text = $"������ 14:50\n\n���̵� ��";
        }
        else if (StageName == "S1_4")
        {
            text.text = $"����� 13:50\n\n���̵� �߻�";
        }
        else if (StageName == "S1_5")
        {
            text.text = $"�ݿ��� 14:50\n\n���̵� ��";
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
        Managers.UI.ClosePopUpUI();
        Managers.UI.isPopuping = false;
        
    }
}
