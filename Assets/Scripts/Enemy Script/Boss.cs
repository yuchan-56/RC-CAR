using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    // HP
    public float hp = 100f;
    public Slider hpSlider;
    public GameObject hpSliderPrefab; // Slider 프리팹 연결

    // Start is called before the first frame update
    void Start()
    {
        if (hpSliderPrefab != null)
        {
            GameObject sliderInstance = Instantiate(hpSliderPrefab, GameObject.Find("EnemyHPCanvas").transform);
            hpSlider = sliderInstance.GetComponent<Slider>();

            hpSlider.maxValue = hp;
            hpSlider.value = hp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // HP 슬라이더 위치 업데이트
        if (hpSlider != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 4f, 0));
            hpSlider.transform.position = screenPosition;
        }
    }
}
