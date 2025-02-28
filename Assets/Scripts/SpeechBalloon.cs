using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBalloon : UI_Popup
{
    public GameObject target;
    public TMP_Text text;
    public Define.EnemyCategory type;
    private void Start()
    {
        target = Managers.Speech.speechTmp;
        playerPopup();
        type = Define.EnemyCategory.����;
        setText();
        StartCoroutine(PopUping_timeSet(3));
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
            if(type==Define.EnemyCategory.����)
            {
                text.text = "������ ��� �ǻ��ΰ� �ƴ϶��";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "��! ������ ���� 1�ð�10���̳� ���!";

            }
            else if (type == Define.EnemyCategory.�濵)
            {
                text.text = "���� ���� �Ѷ� �� ��Ҵµ�";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "�����а��� ���а����� ���Ѵٴϱ�?";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "�����ϴ¹��� ���� �ް� ���� �ʾ�!";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "����j�� �����߾˰��̽�";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "�������а���� ������!!";

            }
            else if (type == Define.EnemyCategory.�ݵ�)
            {
                text.text = "�� �̾�,�� �������ؼ� �� ������";

            }
            else if (type == Define.EnemyCategory.��õ�)
            {
                text.text = "�̴� �ƴ϶��!!! ������!!";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "�츮���� ���ϴ����� �׸� �����";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "�� �����ָ�! ��ġ ���ڱ�ó��!";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "���Ͼ�.. ���踦 ��������...";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "���ڸ� ����� �а��� �ƴϾ�..";

            }
            else if (type == Define.EnemyCategory.�ҹ�)
            {
                text.text = "����� ������� �� ���� ����";

            }
            else if (type == Define.EnemyCategory.���)
            {
                text.text = "��� �����ΰ��� ���밡 �ƴ϶�ϱ�?";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "���и� ���Ѵٰ�?!?! �׾��";

            }
            else if (type == Define.EnemyCategory.�õ�)
            {
                text.text = "�õ�� ���� �뿹��� �о���!";

            }
            else if (type == Define.EnemyCategory.��ȭ)
            {
                text.text = "�� �� ������ 2.1���� ���� ������";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "���縦 ��� ���İ�?. . �׾��";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "�ӿ��� �������� ��ϳİ�? �׾��";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "���� �о��� �׸��ض�";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "�� ���� �ڼ��� ��ǰ ���ٰ� �����!!";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "��..�������� �����������..??";

            }
            else if (type == Define.EnemyCategory.����)
            {
                text.text = "���� �ڷ� ��� ������ �Դ� �ΰ� û�ұ��!";

            }
            else if (type == Define.EnemyCategory.��ȭ)
            {
                text.text = "����! ���ش� �������� �Ѹ��̳� ���!";

            }
            else if (type == Define.EnemyCategory.ȸȭ)
            {
                text.text = "ȸȭ����� �� ��û�Ѱ� �ƴϾ�!";

            }
            else 
            {
                Debug.LogWarning("�������� ���� type");

            }

    }
}
