using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class EveryTimeSchedule : UI_Popup
{
    public GameObject[] obj;
    
    enum Stage { //Sx_y ���� x -> ��ȭ�����.. y�� ����
        S1_1,
        S1_2,
        S1_3,
        S1_4

    };
    enum Line
    {
        S1_1_Line,
        S1_2_Line,
        S1_3_Line,
        S1_4_Line,
        S1_5_Line,

    }

    private void Start()
    {
        Init();
        Managers.UI.ShowPopUpUI<SelectGuide>();

        
    }
    public override void Init()
    {
        base.Init();
        SetResolution();
        Bind<Button>(typeof(Stage));
        Bind<Image>(typeof(Line));

        GetButton((int)Stage.S1_1).gameObject.AddUIEvent(StageGo);
        GetButton((int)Stage.S1_2).gameObject.AddUIEvent(StageGo);
        GetButton((int)Stage.S1_3).gameObject.AddUIEvent(StageGo);
        GetButton((int)Stage.S1_4).gameObject.AddUIEvent(StageGo);


        obj[0] = GetImage((int)Line.S1_1_Line).gameObject;
        obj[1] = GetImage((int)Line.S1_2_Line).gameObject;
        obj[2] = GetImage((int)Line.S1_3_Line).gameObject;
        obj[3] = GetImage((int)Line.S1_4_Line).gameObject;
        obj[4] = GetImage((int)Line.S1_5_Line).gameObject;
        Debug.Log(obj);
        for(int i =0;i<5;i++)
        {
            obj[i].SetActive(false);
        }
    }

    private void StageGo(PointerEventData eventData)    
    {
        string StageName = eventData.pointerClick.name; // Ŭ���� ������Ʈ�� �̸� 

        if(!string.IsNullOrEmpty(StageName))
        {
            if(Managers.UI.isPopuping==true) // Popuping�Ǵ°� �ִ�
            {
                Managers.UI.ClosePopUpUI();
               
            }
            Managers.Data.Stage = StageName;

            Managers.UI.ShowPopUpUI<StageInfo>();
            Managers.UI.isPopuping = true;

        }

        for (int k = 0; k < 5; k++) // �׵θ� �ʱ�ȭ
        {
            obj[k].SetActive(false);
        }

        int i = int.Parse(StageName[3].ToString());
        Debug.Log(i);
        obj[i-1].SetActive(true);

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }
}
