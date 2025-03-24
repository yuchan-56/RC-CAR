using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainGame : BaseScene   // MainGame Ŭ������ BaseScene Ŭ������ ����� ������� ���� ���� �� �ʿ��� �ʱ�ȭ �۾��� �����ϴ� Ŭ����
{
    public GameObject[] roundStage;

    
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
        Managers.Game.GameStart();
        Managers.UI.ShowPopUpUI<StageGuide>();
        Init();
        for(int i =0;i<14;i++) // ���� ������ŭ Enemy ���� ����
        {
            Managers.Game.roundEnemyCount[i] = roundStage[i].transform.childCount; // ���� ������Ʈ�� ������ ���� ������� �۵�
        }
    }

        public void Option()
    {
        Managers.UI.ShowPopUpUI<Option>();
    }
}
