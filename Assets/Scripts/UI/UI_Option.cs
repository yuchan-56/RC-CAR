using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Option : UI_Base
{
    public override void Init()
    {
        throw new System.NotImplementedException();
  
    }

    public void OptionClicked()
    {
        Managers.UI.ShowPopUpUI<Option>();
        SoundManager.Instance.SFXPlay("Button Click");
    }
    public void OptionClicked_InGame()
    {
        Managers.UI.ShowPopUpUI<Option_InGame>();
        SoundManager.Instance.SFXPlay("Button Click");
    }
}
