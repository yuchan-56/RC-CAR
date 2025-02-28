using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBalloon : UI_Popup
{
    public GameObject target;
    public TMP_Text text;

    public static readonly List<string> enemyQuotes = new List<string>
    {
        "������ ��� �ǻ��ΰ� �ƴ϶��",  // 0 - ����
        "�õ�� ���� �뿹��� �о���!",  // 1 - �õ�
        "�� �� ������ 2.1���� ���� ������",  // 2 - ��ȭ
        "�̴� �ƴ϶��!!! ������!!",  // 3 - ��õ�
        "�츮���� ���ϴ����� �׸� �����",  // 4 - ����
        "��! ������ ���� 1�ð�10���̳� ���!",  // 5 - ����
        "��..�������� �����������..??",  // 6 - ����
        "ȸȭ����� �� ��û�Ѱ� �ƴϾ�!",  // 7 - ȸȭ
        "����! ���ش� �������� �Ѹ��̳� ���!",  // 8 - ��ȭ
        "���� �ڷ� ��� ������ �Դ� �ΰ� û�ұ��!",  // 9 - ����
        "�� ���� �ڼ��� ��ǰ ���ٰ� �����!!",  // 10 - ����
        "��� �����ΰ��� ���밡 �ƴ϶�ϱ�?",  // 11 - ���
        "�� �̾�, �� ���� ����ؼ� �� �� ����",  // 12 - �ݵ�
        "�� �����ָ�! ��ġ ���ڱ�ó��!",  // 13 - ����
        "���ڸ� ����� �а��� �ƴϾ�..",  // 14 - ����
        "���� ���� �Ѷ� �� ��Ҵµ�",  // 15 - �濵
        "���� �о��� �׸��ض�",  // 16 - ����
        "���Ͼ�.. ���踦 ��������...",  // 17 - ����
        "����� ������� �� ���� ����",  // 18 - �ҹ�
        "�������а���� ������!!",  // 19 - ����
        "���и� ���Ѵٰ�?!?! �׾��",  // 20 - ����
        "����j�� �����߾˰��̽�",  // 21 - ����
        "�ӿ��� �������� ��ϳİ�? �׾��",  // 22 - ����
        "���縦 ��� ���İ�?. . �׾��",  // 23 - ����
        "�����ϴ¹��� ���� �ް� ���� �ʾ�!",  // 24 - ����
        "�����а��� ���а����� ���Ѵٴϱ�?"  // 25 - ����
    };

    public Define.EnemyCategory type;
    private void Start()
    {
        target = Managers.Speech.speechTmp;
        playerPopup();
        setText();
        StartCoroutine(PopUping_timeSet(3));

        type = (Define.EnemyCategory)1; // 1�� �õ� �ӽ� ����
    }

    private void FixedUpdate()
    {
        playerPopup();
    }
    void playerPopup()
    {
        this.transform.position = target.transform.position;

    }

    IEnumerator PopUping_timeSet(int time)
    {
        yield return new WaitForSeconds(time);

        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<SpeechBalloon>(this.gameObject));
        yield return null;
    }

    void setText()
    {
        int index = (int)type;  // Enum�� Index�� ��ȯ
        if (index >= 0 && index < enemyQuotes.Count)
        {
            text.text = enemyQuotes[index];
        }
        else
        {
            Debug.LogWarning("��ȿ���� ���� EnemyCategory �ε���: " + index);
        }
    }
}
