using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    protected override void Init()
    {
        base.Init();
        Managers.UI.ShowPopUpUI<EveryTimeSchedule>();

      
    }
    private void Start()
    {
        Init();
    }
}
