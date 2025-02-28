using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public Image image;
    public Sprite[] sprites;
    public int currentHP = 3;

    void Start()
    {
        if (image == null)
            image = GetComponent<Image>();

        UpdateHPUI();
    }

    public void GetDamaged(int damageAmount)
    {
        currentHP -= damageAmount;

        if (currentHP < 0)
            currentHP = 0;

        UpdateHPUI();

        if (currentHP == 0)
        {
            Debug.Log("GameOver");
        }
    }

    public void HpRecovery()
    {
        if (currentHP >= 3)
        {
            Debug.Log("HP already Full");
            return;
        }

        currentHP++;
        UpdateHPUI();
    }

    private void UpdateHPUI()
    {
        if (currentHP >= 0 && currentHP < sprites.Length)
        {
            image.sprite = sprites[currentHP];
        }
    }
}
