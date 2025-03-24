using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHP : MonoBehaviour
{
    public PlayerEffect playerEffect;
    public PlayerMove playerMove;
    public Image image;
    public Sprite[] sprites;
    public int currentHP = 10;
    public float animationDuration = 0.2f;
    private bool isHit = false;


    private bool gameOver = false;
    void Start()
    {
        if (image == null)
            image = GetComponent<Image>();

        playerMove = FindObjectOfType<PlayerMove>();
        playerEffect = FindObjectOfType<PlayerEffect>(); // isHit값 받기 위한 상호작용
        UpdateHPUI();
    }

    public void TriggerDamage()
    {
        if (playerMove.animator != null)    
        {
            playerMove.animator.SetTrigger("GetDamaged");
            StartCoroutine(ResetDamageState());
        }
    }

    private IEnumerator ResetDamageState()
    {
        Debug.Log("강제 중지");
        yield return new WaitForSeconds(animationDuration);
        playerMove.animator.SetTrigger("GetDamagedDisable");
    }

    public void GetDamaged(int damageAmount)
    {
        if (Managers.Game.isHit) return;


        TriggerDamage();
        currentHP -= damageAmount;

        if (currentHP < 0)
                currentHP = 0;

        UpdateHPUI();

        if (currentHP == 0 && !gameOver)
        {
            gameOver = true;
            Debug.Log("GameOver");
            playerMove.animator.SetTrigger("Dead");
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
