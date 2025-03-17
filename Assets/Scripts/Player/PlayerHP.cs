using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHP : MonoBehaviour
{
    public PlayerMove playerMove;
    public Image image;
    public Sprite[] sprites;
    public int currentHP = 3;
    public float animationDuration = 1.0f;

    void Start()
    {
        if (image == null)
            image = GetComponent<Image>();

        UpdateHPUI();
    }

    public void TriggerDamage()
    {
        if (playerMove.animator != null)
        {
            playerMove.animator.SetBool("GetDamaged", true);
            StartCoroutine(ResetDamageState());
        }
    }

    private IEnumerator ResetDamageState()
    {
        yield return new WaitForSeconds(animationDuration);
        playerMove.animator.SetBool("GetDamaged", false);
    }

    public void GetDamaged(int damageAmount)
    {
        TriggerDamage();
        currentHP -= damageAmount;

        if (currentHP < 0)
            currentHP = 0;

        UpdateHPUI();

        if (currentHP == 0)
        {
            Debug.Log("GameOver");
            Time.timeScale = 0;
            Managers.UI.ShowPopUpUI<GameOver>();
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
