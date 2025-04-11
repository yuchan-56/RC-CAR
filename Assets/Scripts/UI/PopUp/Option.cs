using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Option : UI_Popup
{
    enum Buttons
    {
       Close,
       Credit
    }
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Close).gameObject.AddUIEvent(GoClose);
        GetButton((int)Buttons.Credit).gameObject.AddUIEvent(GoCredit);
    }
    // Start is called before the first frame update
    void GoClose(PointerEventData eventData)
    {
        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<Option>(this.gameObject));
    }

    void GoCredit(PointerEventData eventData)
    {
        Managers.UI.ShowPopUpUI<Credit>();
    }
    
    void Start()
    {
        Init();
        Managers.UI.SetCanvasNumber(this.gameObject,1000);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
