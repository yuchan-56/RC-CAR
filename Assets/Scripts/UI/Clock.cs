using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public Sprite[] images;  // 15개의 이미지 배열
    public float timerDuration = 60f;  // 타이머 지속 시간 (60초)
    private float currentTime;  // 현재 시간
    private int currentImageIndex = 0;  // 현재 이미지 인덱스
    private float changeInterval = 4f;  // 이미지 변경 간격 (4초)
    private float nextChangeTime;  // 다음 이미지 변경 시간
    public Image imageComponent;
    // 이미지가 표시될 스프라이트 렌더러
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        nextChangeTime = changeInterval;


    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime>=nextChangeTime)
            {
                ChangeImage();
                nextChangeTime += changeInterval;  // 다음 이미지 변경 시간 설정
            }


        }
        else if(currentTime==60f)
        {
            // 타이머가 끝나면 게임 오버 애니메이션이나 화면 변경 등을 처리
            // TriggerGameOver();
        }
    }
    void ChangeImage()
    {
        // 이미지 배열에서 현재 인덱스에 해당하는 이미지를 변경
        imageComponent.sprite = images[currentImageIndex];  // 이미지 변경
        currentImageIndex++;  // 다음 이미지로 이동

    }
}
