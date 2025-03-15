using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Option : UI_Popup
{
    enum Buttons
    {
        Play
    }
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Play).gameObject.AddUIEvent(GoPlay);
    }
    // Start is called before the first frame update
    void GoPlay(PointerEventData eventData)
    {
        Managers.UI.ClosePopUpUI();
    }
    void Start()
    {
        Init();
        Managers.UI.SetCanvasMost(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
