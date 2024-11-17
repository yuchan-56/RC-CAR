using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class EveryTimeSchedule : UI_Popup
{
    enum Stage { //Sx_y 에서 x -> 월화수목금.. y는 교시
        S1_1,
        S1_2,
        S1_3,
        S1_4

    };

    private void Start()
    {
        Init();
        
    }
    public override void Init()
    {
        base.Init();
        SetResolution();
        Bind<Button>(typeof(Stage));
        GetButton((int)Stage.S1_1).gameObject.AddUIEvent(StageGo);
        GetButton((int)Stage.S1_2).gameObject.AddUIEvent(StageGo);
        GetButton((int)Stage.S1_3).gameObject.AddUIEvent(StageGo);
        GetButton((int)Stage.S1_4).gameObject.AddUIEvent(StageGo);
    }

    private void StageGo(PointerEventData eventData)
    {
        string StageName = eventData.pointerClick.name; // 클릭한 오브젝트의 이름 

        if(!string.IsNullOrEmpty(StageName))
        {
            Managers.Data.Stage = StageName;

            Managers.UI.ShowPopUpUI<StageInfo>();
       
        }

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }
}
