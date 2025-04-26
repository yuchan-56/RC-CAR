using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Clock : MonoBehaviour
{
    public Sprite[] images;  // 15���� �̹��� �迭
    public float timerDuration;  // Ÿ�̸� ���� �ð� (60��)
    private float currentTime;  // ���� �ð�
    private int currentImageIndex = 0;  // ���� �̹��� �ε���
    private float changeInterval;  // �̹��� ���� ���� (4��)
    private float nextChangeTime;  // ���� �̹��� ���� �ð�
    public Image imageComponent;
    public Image failImage;
    public Image SuccessImage;
    //private List<GameObject> enemies;
    // �̹����� ǥ�õ� ��������Ʈ ������
    // Start is called before the first frame update
    void Start()
    {
        currentTime = timerDuration;
        changeInterval = timerDuration / 14;
        nextChangeTime = changeInterval;
        imageComponent.sprite = images[currentImageIndex];  // �̹��� ����
        currentImageIndex++;  // ���� �̹����� �̵�
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
                nextChangeTime += changeInterval;  // ���� �̹��� ���� �ð� ����

            }
            /*if(enemies.Count==0)//�� ������ �� �׾�����, success�� ��������
            {

                SuccessImage.gameObject.SetActive(true);
                Time.timeScale = 0;
                Debug.Log("Game Clear!");
                TriggerSuccess();

            }*/
           


        }
        else if(currentTime<=0)
        {
            // Ÿ�̸Ӱ� ������ ���� ���� �ִϸ��̼��̳� ȭ�� ���� ���� ó��
            TriggerGameOver();
        }
    }
    void ChangeImage()
    {
        // �̹��� �迭���� ���� �ε����� �ش��ϴ� �̹����� ����
        imageComponent.sprite = images[currentImageIndex];  // �̹��� ����
        currentImageIndex++;  // ���� �̹����� �̵�

    }
    void TriggerGameOver()
    {
        failImage.gameObject.SetActive(true);
         Time.timeScale = 0;
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
        // �� ���� �� ����Ʈ���� ����
        enemies.Remove(enemy);
    }*/
}
