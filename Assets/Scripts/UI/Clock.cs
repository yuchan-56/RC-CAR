using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public Sprite[] images;  // 15���� �̹��� �迭
    public float timerDuration = 60f;  // Ÿ�̸� ���� �ð� (60��)
    private float currentTime;  // ���� �ð�
    private int currentImageIndex = 0;  // ���� �̹��� �ε���
    private float changeInterval = 4f;  // �̹��� ���� ���� (4��)
    private float nextChangeTime;  // ���� �̹��� ���� �ð�
    public Image imageComponent;
    // �̹����� ǥ�õ� ��������Ʈ ������
    // Start is called before the first frame update
    void Start()
    {
        currentTime = timerDuration;
        nextChangeTime = changeInterval;
        imageComponent.sprite = images[currentImageIndex];  // �̹��� ����
        currentImageIndex++;  // ���� �̹����� �̵�


    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime<=timerDuration-nextChangeTime)
            {
                ChangeImage();
                nextChangeTime += changeInterval;  // ���� �̹��� ���� �ð� ����
            }


        }
        else if(currentTime<=0)
        {
            // Ÿ�̸Ӱ� ������ ���� ���� �ִϸ��̼��̳� ȭ�� ���� ���� ó��
            // TriggerGameOver();
        }
    }
    void ChangeImage()
    {
        // �̹��� �迭���� ���� �ε����� �ش��ϴ� �̹����� ����
        imageComponent.sprite = images[currentImageIndex];  // �̹��� ����
        currentImageIndex++;  // ���� �̹����� �̵�

    }
}
