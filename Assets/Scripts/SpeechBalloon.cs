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
        "법대라고 모두 판사인게 아니라고",  // 0 - 법학
        "시디라 쓰고 노예라고 읽어줘!",  // 1 - 시디
        "왜 내 학점이 2.1인지 묻지 말아줘",  // 2 - 신화
        "미대 아니라고!!! 공대라고!!",  // 3 - 기시디
        "우리과가 뭐하는지좀 그만 물어봐",  // 4 - 도공
        "와! 오늘은 무려 1시간10분이나 잤어!",  // 5 - 건축
        "누..누구한테 물어봐야하지..??",  // 6 - 자전
        "회화과라고 다 멍청한게 아니야!",  // 7 - 회화
        "만세! 올해는 교수님이 한명이나 계셔!",  // 8 - 판화
        "나는 코로 모든 먼지를 먹는 인간 청소기야!",  // 9 - 조소
        "내 얼굴이 뒤샹의 작품 같다고 놀리지마!!",  // 10 - 예술
        "산업 디자인과는 공대가 아니라니까?",  // 11 - 산디
        "아 미안, 나 재료비 사야해서 술 못 마셔",  // 12 - 금디
        "널 빚어주마! 마치 도자기처럼!",  // 13 - 도예
        "의자만 만드는 학과는 아니야..",  // 14 - 목조
        "ㅋㅋ 내가 한때 좀 놀았는데",  // 15 - 경영
        "영어 읽어보라고 그만해라",  // 16 - 영문
        "독일아.. 세계를 지배해줘...",  // 17 - 독문
        "사실은 프랑스어를 잘 알지 못해",  // 18 - 불문
        "공무원학과라고 하지마!!",  // 19 - 국문
        "수학만 잘한다고?!?! 죽어라",  // 20 - 수교
        "맛춤뻡은 내가잘알고이써",  // 21 - 국교
        "임용고시 떨어지면 어떡하냐고? 죽어라",  // 22 - 영교
        "역사를 어디에 쓰냐고?. . 죽어라",  // 23 - 역교
        "교육하는법을 교육 받고 싶지 않아!",  // 24 - 교육
        "경제학과는 수학같은거 안한다니까?"  // 25 - 경제
    };

    public Define.EnemyCategory type;
    private void Start()
    {
        target = Managers.Speech.speechTmp;
        setText();
        StartCoroutine(PopUping_timeSet(3));

        type = (Define.EnemyCategory)1; // 1은 시디 임시 지정
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
        int index = (int)type;  // Enum을 Index로 변환
        if (index >= 0 && index < enemyQuotes.Count)
        {
            text.text = enemyQuotes[index];
        }
        else
        {
            Debug.LogWarning("유효하지 않은 EnemyCategory 인덱스: " + index);
        }
    }
}
