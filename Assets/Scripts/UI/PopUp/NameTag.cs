using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameTag : UI_Popup
{
    private GameObject target;
    public TMP_Text name;
    public static readonly Dictionary<Define.EnemyCategory, (string quote, string english)> enemyQuotes =
    new Dictionary<Define.EnemyCategory, (string, string)>
{
          //prefab ������ �ϳ� �� ���Ƽ� ��ȭ�� �׳� chemicalE�� ��ġ�� mechanicalE�� ����.
    { Define.EnemyCategory.����, ("��! ������ ���� 1�ð� 10���̳� ���!", "architect") },
    { Define.EnemyCategory.����, ("�� ���� �ڼ��� ��ǰ ���ٰ� ��� ��!", "arts") },
    { Define.EnemyCategory.��ȭ, ("�� �� ������ 2.1���� ���� ������...", "chemicalE") },
    { Define.EnemyCategory.�濵, ("���� ���� �Ѷ� �� ��Ҵµ�", "business") },
    { Define.EnemyCategory.����, ("�� �����ָ�! ��ġ ���ڱ�ó��!", "ceramics") },
    { Define.EnemyCategory.����, ("�����а��� ���� ������ �� �Ѵٴϱ�?", "economics") },
    { Define.EnemyCategory.����, ("�����ϴ� ���� �����ް� ���� �ʾ�!", "education") },
    { Define.EnemyCategory.����, ("���� �о��� �׸��ض�", "english") },
    { Define.EnemyCategory.ȸȭ, ("ȸȭ����� �� ��û�� �� �ƴϾ�!", "finearts") },
    { Define.EnemyCategory.�ҹ�, ("����� ������� �� ���� ����", "french") },
    { Define.EnemyCategory.����, ("���Ͼ�... ���踦 ��������...", "german") },
    { Define.EnemyCategory.���, ("��������ΰ��� ���밡 �ƴ϶�ϱ�?", "industrialID") },
    { Define.EnemyCategory.����, ("������ �а���� ������!!", "korean") },
    { Define.EnemyCategory.����, ("������ ��� �ǻ��� �� �ƴ϶��", "law") },
    { Define.EnemyCategory.����, ("��...�������� �����������...?", "autonomous") },
    { Define.EnemyCategory.�ݵ�, ("�� �̾�, �� ���� ����ؼ� �� �� ����", "metal") },
    { Define.EnemyCategory.��ȭ, ("����! ���ش� �������� �� ���̳� ���!", "print") },
    { Define.EnemyCategory.����, ("���� �ڷ� ��� ������ �Դ� �ΰ� û�ұ��!", "sculpture") },
    { Define.EnemyCategory.����, ("�츮 ���� ���ϴ��� �� �׸� �����...", "urbanE") },
    { Define.EnemyCategory.�õ�, ("�õ�� ���� �뿹��� �о���!", "visualD") },
    { Define.EnemyCategory.����, ("���ڸ� ����� �а��� �ƴϾ�...", "wood") }
};
    public Define.EnemyCategory type;

    public void Init(GameObject t)
    {
        target =t;
        // target.name �� ������� Dictionary���� type ã��
        string targetName = target.name.ToLower(); // ���� �̸�
        targetName = targetName.Substring(7);
        foreach (var pair in enemyQuotes) // enemyQuotes �� �ִ� pair �� ���Ͽ�
        {
            if (pair.Value.english.ToLower() == targetName) // english�� �´� target�̸��� ã����
            {
                type = pair.Key; // Key�� ���� type
                break;
            }
        }

        
        setText();
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
        else {
            
            Managers.UI.ClosePopUp_handleTarget(Util.GetOrAddComponent<NameTag>(this.gameObject));
        
        }

    }

    private void setText()
    {
        if (enemyQuotes.TryGetValue(type, out var pair))
        {
            name.text = type.ToString(); // Key �ѱ��� ���
        }
        
    }
}
