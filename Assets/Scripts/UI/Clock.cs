using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Clock : MonoBehaviour
{
    public Sprite[] images;  // 15개의 이미지 배열
    public float timerDuration;  // 타이머 지속 시간 (60초)
    private float currentTime;  // 현재 시간
    private int currentImageIndex = 0;  // 현재 이미지 인덱스
    private float changeInterval;  // 이미지 변경 간격 (4초)
    private float nextChangeTime;  // 다음 이미지 변경 시간
    public Image imageComponent;
    public Image failImage;
    public Image SuccessImage;

    bool dead = false;
    //private List<GameObject> enemies;
    // 이미지가 표시될 스프라이트 렌더러
    // Start is called before the first frame update
    void Start()
    {
        currentTime = timerDuration;
        changeInterval = timerDuration / 14;
        nextChangeTime = changeInterval;
        imageComponent.sprite = images[currentImageIndex];  // 이미지 변경
        currentImageIndex++;  // 다음 이미지로 이동
        failImage.gameObject.SetActive(false);
        SuccessImage.gameObject.SetActive(false);
        //enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime<=timerDuration-nextChangeTime && currentImageIndex<=13)
            {
                ChangeImage();
                nextChangeTime += changeInterval;  // 다음 이미지 변경 시간 설정

            }
            /*if(enemies.Count==0)//몹 보스가 다 죽었을때, success가 나오도록
            {

                SuccessImage.gameObject.SetActive(true);
                Time.timeScale = 0;
                Debug.Log("Game Clear!");
                TriggerSuccess();

            }*/
           


        }
        else if(currentTime<=0&&dead==false)
        {
            // 타이머가 끝나면 게임 오버 애니메이션이나 화면 변경 등을 처리
            TriggerGameOver();
            dead = true;
        }
    }
    void ChangeImage()
    {
        // 이미지 배열에서 현재 인덱스에 해당하는 이미지를 변경
        imageComponent.sprite = images[currentImageIndex];  // 이미지 변경
        currentImageIndex++;  // 다음 이미지로 이동

    }
    void TriggerGameOver()
    {
        
        failImage.gameObject.SetActive(true);
         Time.timeScale = 0;
        PlayerHP playerhp = FindFirstObjectByType<PlayerHP>();
        playerhp.dieAnimation();
        StartCoroutine(playerhp.StartGameOverUI());
        Debug.Log("Game Over!");


    
    }
    void TriggerSuccess()
    {
        SuccessImage.gameObject.SetActive(true);
        Time.timeScale = 0;
        Debug.Log("Game Clear!");

    }
    /*public void EnemyDied(GameObject enemy)
    {
        // 적 제거 시 리스트에서 삭제
        enemies.Remove(enemy);
    }*/
}
