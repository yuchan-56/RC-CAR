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
 { // 양이 적기떄문에 Dictionary를 tuple식으로 입력.
    { Define.EnemyCategory.법학, ("법대라고 모두 판사인게 아니라고", "law") },
    { Define.EnemyCategory.시디, ("시디라 쓰고 노예라고 읽어줘!", "visualD") },
    { Define.EnemyCategory.신화, ("왜 내 학점이 2.1인지 묻지 말아줘", "autonomous") },
    { Define.EnemyCategory.기시디, ("미대 아니라고!!! 공대라고!!", "industrialD") },
    { Define.EnemyCategory.도공, ("우리과가 뭐하는지좀 그만 물어봐", "ceramics") },
    { Define.EnemyCategory.건축, ("와! 오늘은 무려 1시간10분이나 잤어!", "architect") },
    { Define.EnemyCategory.자전, ("누..누구한테 물어봐야하지..??", "mechanicalE") },
    { Define.EnemyCategory.회화, ("회화과라고 다 멍청한게 아니야!", "finearts") },
    { Define.EnemyCategory.판화, ("만세! 올해는 교수님이 한명이나 계셔!", "print") },
    { Define.EnemyCategory.조소, ("나는 코로 모든 먼지를 먹는 인간 청소기야!", "sculpture") },
    { Define.EnemyCategory.예술, ("내 얼굴이 뒤샹의 작품 같다고 놀리지마!!", "arts") },
    { Define.EnemyCategory.산디, ("산업 디자인과는 공대가 아니라니까?", "industrialD") },
    { Define.EnemyCategory.금디, ("아 미안, 나 재료비 사야해서 술 못 마셔", "metal") },
    { Define.EnemyCategory.도예, ("널 빚어주마! 마치 도자기처럼!", "ceramics") },
    { Define.EnemyCategory.목조, ("의자만 만드는 학과는 아니야..", "wood") },
    { Define.EnemyCategory.경영, ("ㅋㅋ 내가 한때 좀 놀았는데", "business") },
    { Define.EnemyCategory.영문, ("영어 읽어보라고 그만해라", "english") },
    { Define.EnemyCategory.독문, ("독일아.. 세계를 지배해줘...", "german") },
    { Define.EnemyCategory.불문, ("사실은 프랑스어를 잘 알지 못해", "french") },
    { Define.EnemyCategory.국문, ("공무원학과라고 하지마!!", "korean") },
    { Define.EnemyCategory.수교, ("수학만 잘한다고?!?! 죽어라", "education") },
    { Define.EnemyCategory.국교, ("맛춤뻡은 내가잘알고이써", "education") },
    { Define.EnemyCategory.영교, ("임용고시 떨어지면 어떡하냐고? 죽어라", "education") },
    { Define.EnemyCategory.역교, ("역사를 어디에 쓰냐고?. . 죽어라", "education") },
    { Define.EnemyCategory.교육, ("교육하는법을 교육 받고 싶지 않아!", "education") },
    { Define.EnemyCategory.경제, ("경제학과는 수학같은거 안한다니까?", "economics") }
 };


    public Define.EnemyCategory type;
    private void Start()
    {
        target = Managers.Speech.speechTmp;
        Debug.Log($"{target}의 SpeechBalloon 추출");

        // target.name 을 기반으로 Dictionary에서 type 찾기
        string targetName = target.name.ToLower(); // 영어 이름
        targetName = targetName.Substring(7);
        foreach (var pair in enemyQuotes) // enemyQuotes 에 있는 pair 에 대하여
        {
            if (pair.Value.english.ToLower() == targetName) // english에 맞는 target이름을 찾으면
            {
                Debug.Log(pair.Key);
                type = pair.Key; // Key의 값이 type
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
            text.text = pair.quote; // 대사 출력
            Name.text = type.ToString(); // Key 한국어 출력
        }
        else
        {
            Debug.LogWarning($"'{type}'에 해당하는 대사가 없습니다.");
        }
    }

}
