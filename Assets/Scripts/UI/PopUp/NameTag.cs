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
          //prefab 폴더가 하나 더 많아서 신화를 그냥 chemicalE로 퉁치고 mechanicalE를 없앰.
    { Define.EnemyCategory.건축, ("와! 오늘은 무려 1시간 10분이나 잤어!", "architect") },
    { Define.EnemyCategory.예술, ("내 얼굴이 뒤샹의 작품 같다고 놀리지 마!", "arts") },
    { Define.EnemyCategory.신화, ("왜 내 학점이 2.1인지 묻지 말아줘...", "chemicalE") },
    { Define.EnemyCategory.경영, ("ㅋㅋ 내가 한때 좀 놀았는데", "business") },
    { Define.EnemyCategory.도예, ("널 빚어주마! 마치 도자기처럼!", "ceramics") },
    { Define.EnemyCategory.경제, ("경제학과는 수학 같은거 안 한다니까?", "economics") },
    { Define.EnemyCategory.교육, ("교육하는 법을 교육받고 싶지 않아!", "education") },
    { Define.EnemyCategory.영문, ("영어 읽어보라고 그만해라", "english") },
    { Define.EnemyCategory.회화, ("회화과라고 다 멍청한 게 아니야!", "finearts") },
    { Define.EnemyCategory.불문, ("사실은 프랑스어를 잘 알지 못해", "french") },
    { Define.EnemyCategory.독문, ("독일아... 세계를 지배해줘...", "german") },
    { Define.EnemyCategory.산디, ("산업디자인과는 공대가 아니라니까?", "industrialID") },
    { Define.EnemyCategory.국문, ("공무원 학과라고 하지마!!", "korean") },
    { Define.EnemyCategory.법학, ("법대라고 모두 판사인 게 아니라고", "law") },
    { Define.EnemyCategory.자전, ("누...누구한테 물어봐야하지...?", "autonomous") },
    { Define.EnemyCategory.금디, ("아 미안, 나 재료비 사야해서 술 못 마셔", "metal") },
    { Define.EnemyCategory.판화, ("만세! 올해는 교수님이 한 분이나 계셔!", "print") },
    { Define.EnemyCategory.조소, ("나는 코로 모든 먼지를 먹는 인간 청소기야!", "sculpture") },
    { Define.EnemyCategory.도공, ("우리 과가 뭐하는지 좀 그만 물어봐...", "urbanE") },
    { Define.EnemyCategory.시디, ("시디라 쓰고 노예라고 읽어줘!", "visualD") },
    { Define.EnemyCategory.목조, ("의자만 만드는 학과는 아니야...", "wood") }
};
    public Define.EnemyCategory type;

    public void Init(GameObject t)
    {
        target =t;
        // target.name 을 기반으로 Dictionary에서 type 찾기
        string targetName = target.name.ToLower(); // 영어 이름
        targetName = targetName.Substring(7);
        foreach (var pair in enemyQuotes) // enemyQuotes 에 있는 pair 에 대하여
        {
            if (pair.Value.english.ToLower() == targetName) // english에 맞는 target이름을 찾으면
            {
                type = pair.Key; // Key의 값이 type
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
            name.text = type.ToString(); // Key 한국어 출력
        }
        else
        {
            Debug.LogWarning($"'{type}'에 해당하는 대사가 없습니다.");
        }
    }
}
