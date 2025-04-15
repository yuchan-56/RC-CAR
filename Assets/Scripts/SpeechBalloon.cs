using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBalloon : UI_Popup
{
    public GameObject target;
    public TMP_Text text;
    public TMP_Text Name;



    public static readonly Dictionary<Define.EnemyCategory, (string quote, string english)> enemyQuotes =
     new Dictionary<Define.EnemyCategory, (string, string)>
 { // ���� ���⋚���� Dictionary�� tuple������ �Է�.
    { Define.EnemyCategory.����, ("������ ��� �ǻ��ΰ� �ƴ϶��", "law") },
    { Define.EnemyCategory.�õ�, ("�õ�� ���� �뿹��� �о���!", "visualD") },
    { Define.EnemyCategory.��ȭ, ("�� �� ������ 2.1���� ���� ������", "autonomous") },
    { Define.EnemyCategory.��õ�, ("�̴� �ƴ϶��!!! ������!!", "industrialD") },
    { Define.EnemyCategory.����, ("�츮���� ���ϴ����� �׸� �����", "ceramics") },
    { Define.EnemyCategory.����, ("��! ������ ���� 1�ð�10���̳� ���!", "architect") },
    { Define.EnemyCategory.����, ("��..�������� �����������..??", "mechanicalE") },
    { Define.EnemyCategory.ȸȭ, ("ȸȭ����� �� ��û�Ѱ� �ƴϾ�!", "finearts") },
    { Define.EnemyCategory.��ȭ, ("����! ���ش� �������� �Ѹ��̳� ���!", "print") },
    { Define.EnemyCategory.����, ("���� �ڷ� ��� ������ �Դ� �ΰ� û�ұ��!", "sculpture") },
    { Define.EnemyCategory.����, ("�� ���� �ڼ��� ��ǰ ���ٰ� �����!!", "arts") },
    { Define.EnemyCategory.���, ("��� �����ΰ��� ���밡 �ƴ϶�ϱ�?", "industrialD") },
    { Define.EnemyCategory.�ݵ�, ("�� �̾�, �� ���� ����ؼ� �� �� ����", "metal") },
    { Define.EnemyCategory.����, ("�� �����ָ�! ��ġ ���ڱ�ó��!", "ceramics") },
    { Define.EnemyCategory.����, ("���ڸ� ����� �а��� �ƴϾ�..", "wood") },
    { Define.EnemyCategory.�濵, ("���� ���� �Ѷ� �� ��Ҵµ�", "business") },
    { Define.EnemyCategory.����, ("���� �о��� �׸��ض�", "english") },
    { Define.EnemyCategory.����, ("���Ͼ�.. ���踦 ��������...", "german") },
    { Define.EnemyCategory.�ҹ�, ("����� ������� �� ���� ����", "french") },
    { Define.EnemyCategory.����, ("�������а���� ������!!", "korean") },
    { Define.EnemyCategory.����, ("���и� ���Ѵٰ�?!?! �׾��", "education") },
    { Define.EnemyCategory.����, ("����j�� �����߾˰��̽�", "education") },
    { Define.EnemyCategory.����, ("�ӿ��� �������� ��ϳİ�? �׾��", "education") },
    { Define.EnemyCategory.����, ("���縦 ��� ���İ�?. . �׾��", "education") },
    { Define.EnemyCategory.����, ("�����ϴ¹��� ���� �ް� ���� �ʾ�!", "education") },
    { Define.EnemyCategory.����, ("�����а��� ���а����� ���Ѵٴϱ�?", "economics") }
 };


    public Define.EnemyCategory type;
    private void Start()
    {
        target = Managers.Speech.speechTmp;
        Debug.Log($"{target}�� SpeechBalloon ����");

        // target.name �� ������� Dictionary���� type ã��
        string targetName = target.name.ToLower(); // ���� �̸�
        targetName = targetName.Substring(7);
        foreach (var pair in enemyQuotes) // enemyQuotes �� �ִ� pair �� ���Ͽ�
        {
            if (pair.Value.english.ToLower() == targetName) // english�� �´� target�̸��� ã����
            {
                Debug.Log(pair.Key);
                type = pair.Key; // Key�� ���� type
                break;
            }
        }

        setText();
        StartCoroutine(PopUping_timeSet(3));
    }


    private void FixedUpdate()
    {
        playerPopup();
    }
    void playerPopup()
    {
        if (target != null)
        {
            this.transform.position = target.transform.position;
        }
        else { Destroy(this.gameObject); }
        
    }

    IEnumerator PopUping_timeSet(int time)
    {
        yield return new WaitForSeconds(time);

        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<SpeechBalloon>(this.gameObject));
        yield return null;
    }

    void setText()
    {
        if (enemyQuotes.TryGetValue(type, out var pair))
        {
            text.text = pair.quote; // ��� ���
            Name.text = type.ToString(); // Key �ѱ��� ���
        }
        else
        {
            Debug.LogWarning($"'{type}'�� �ش��ϴ� ��簡 �����ϴ�.");
        }
    }

}
