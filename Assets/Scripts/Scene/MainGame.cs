using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainGame : BaseScene   // MainGame Ŭ������ BaseScene Ŭ������ ����� ������� ���� ���� �� �ʿ��� �ʱ�ȭ �۾��� �����ϴ� Ŭ����
{ 
   
    
    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    protected override void Init()
    {
       
        base.Init();
   
    }

    private void Start()
    {
        Managers.Game.currentGround = GameManager.GameGround.Rx;
        Managers.UI.ShowPopUpUI<StageGuide>();
        Init();
    }

        public void Option()
    {
        Managers.UI.ShowPopUpUI<Option>();
    }
}
