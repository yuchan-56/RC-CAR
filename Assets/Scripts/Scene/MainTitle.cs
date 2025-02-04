using UnityEngine;
using UnityEngine.EventSystems;
using static LoadingScene;
using UnityEngine.UI;
using System.Collections;
public class MainTitle : BaseScene
//MainTitle Ŭ������ ���� �޴� ȭ���� ����ϴ� Ŭ������� �� �� �ֽ��ϴ�. ���콺 Ŭ���̳� ��ġ�� �����Ͽ� �ƾ� UI�� �����ְų� �ּ� ó���� �κ�ó�� �ٸ� ������ ��ȯ�ϴ� ������ ������ �� �ֽ��ϴ�. 

{
    public Sprite[] sprites;
    public Image uiImage;
  

    void Start()
    {
        StartCoroutine(PlaySpriteAnimation());
        Init();
    }

    IEnumerator PlaySpriteAnimation()
    {

        for (int i = 0; i < sprites.Length; i++)
        {
            uiImage.sprite = sprites[i];
            yield return new WaitForSecondsRealtime(0.1f);
        }
        StartCoroutine(PlaySpriteAnimation());

    }
    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    protected override void Init()
    {
        base.Init();
    }


    public void IsStart()
    {
        LoadingScene.Instance.GoLoading("TitleScene");
    }
}
