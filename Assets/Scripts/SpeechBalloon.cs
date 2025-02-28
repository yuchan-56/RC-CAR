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
        type = Define.EnemyCategory.법학;
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
            if(type==Define.EnemyCategory.법학)
            {
                text.text = "법대라고 모두 판사인게 아니라고";

            }
            else if (type == Define.EnemyCategory.건축)
            {
                text.text = "와! 오늘은 무려 1시간10분이나 잤어!";

            }
            else if (type == Define.EnemyCategory.경영)
            {
                text.text = "ㅋㅋ 내가 한때 좀 놀았는데";

            }
            else if (type == Define.EnemyCategory.경제)
            {
                text.text = "경제학과는 수학같은거 안한다니까?";

            }
            else if (type == Define.EnemyCategory.교육)
            {
                text.text = "교육하는법을 교육 받고 싶지 않아!";

            }
            else if (type == Define.EnemyCategory.국교)
            {
                text.text = "맛춤뻡은 내가잘알고이써";

            }
            else if (type == Define.EnemyCategory.국문)
            {
                text.text = "공무원학과라고 하지마!!";

            }
            else if (type == Define.EnemyCategory.금디)
            {
                text.text = "아 미안,나 재료비사야해서 술 못마셔";

            }
            else if (type == Define.EnemyCategory.기시디)
            {
                text.text = "미대 아니라고!!! 공대라고!!";

            }
            else if (type == Define.EnemyCategory.도공)
            {
                text.text = "우리과가 뭐하는지좀 그만 물어봐";

            }
            else if (type == Define.EnemyCategory.도예)
            {
                text.text = "널 빚어주마! 마치 도자기처럼!";

            }
            else if (type == Define.EnemyCategory.독문)
            {
                text.text = "독일아.. 세계를 지배해줘...";

            }
            else if (type == Define.EnemyCategory.목조)
            {
                text.text = "의자만 만드는 학과는 아니야..";

            }
            else if (type == Define.EnemyCategory.불문)
            {
                text.text = "사실은 프랑스어를 잘 알지 못해";

            }
            else if (type == Define.EnemyCategory.산디)
            {
                text.text = "산업 디자인과는 공대가 아니라니까?";

            }
            else if (type == Define.EnemyCategory.수교)
            {
                text.text = "수학만 잘한다고?!?! 죽어라";

            }
            else if (type == Define.EnemyCategory.시디)
            {
                text.text = "시디라 쓰고 노예라고 읽어줘!";

            }
            else if (type == Define.EnemyCategory.신화)
            {
                text.text = "왜 내 학점이 2.1인지 묻지 말아줘";

            }
            else if (type == Define.EnemyCategory.역교)
            {
                text.text = "역사를 어디에 쓰냐고?. . 죽어라";

            }
            else if (type == Define.EnemyCategory.영교)
            {
                text.text = "임용고시 떨어지면 어떡하냐고? 죽어라";

            }
            else if (type == Define.EnemyCategory.영문)
            {
                text.text = "영어 읽어보라고 그만해라";

            }
            else if (type == Define.EnemyCategory.예술)
            {
                text.text = "내 얼굴이 뒤샹의 작품 같다고 놀리지마!!";

            }
            else if (type == Define.EnemyCategory.자전)
            {
                text.text = "누..누구한테 물어봐야하지..??";

            }
            else if (type == Define.EnemyCategory.조소)
            {
                text.text = "나는 코로 모든 먼지를 먹는 인간 청소기야!";

            }
            else if (type == Define.EnemyCategory.판화)
            {
                text.text = "만세! 올해는 교수님이 한명이나 계셔!";

            }
            else if (type == Define.EnemyCategory.회화)
            {
                text.text = "회화과라고 다 멍청한게 아니야!";

            }
            else 
            {
                Debug.LogWarning("지정되지 않은 type");

            }

    }
}
