using UnityEngine;
using UnityEngine.EventSystems;
public class MainTitle : BaseScene,IPointerClickHandler
//MainTitle Ŭ������ ���� �޴� ȭ���� ����ϴ� Ŭ������� �� �� �ֽ��ϴ�. ���콺 Ŭ���̳� ��ġ�� �����Ͽ� �ƾ� UI�� �����ְų� �ּ� ó���� �κ�ó�� �ٸ� ������ ��ȯ�ϴ� ������ ������ �� �ֽ��ϴ�. 

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
        Init();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Managers.UI.ShowPopUpUI<UI_CutScene>();


        // Managers.Scene.LoadScene(Define.Scene.MainGame);
    }
}
