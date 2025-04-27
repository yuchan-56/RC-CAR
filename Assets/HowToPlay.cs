using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
public class HowToPlay : UI_Popup
{
    enum Buttons
    {
        Skip
    }
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Skip).gameObject.AddUIEvent(Skipped);

    }
    private void Start()
    {
        Init();
    }

    void Skipped(PointerEventData eventData)
    {
        SoundManager.Instance.SFXPlay("Button Click");
        Time.timeScale = 1;
        Destroy(this.gameObject);
    }
}
