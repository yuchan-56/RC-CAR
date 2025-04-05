using UnityEngine;
using System.Collections;

public class PlayerHP : MonoBehaviour
{
    public PlayerEffect playerEffect;
    public PlayerMove playerMove;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public int currentHP = 3;
    public float animationDuration = 0.01f;
    private bool isHit = false;
    private bool isAutoRecovering = false;
    private Coroutine hpRecoverCoroutine;
    private float recoverDelayAfterHit = 5f;
    private float hpRecoverInterval = 2f;
    private Coroutine hpFadeCoroutine;


    private bool gameOver = false;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        SetHpBarAlpha(0f);
        Canvas.ForceUpdateCanvases();
        playerMove = FindObjectOfType<PlayerMove>();
        playerEffect = FindObjectOfType<PlayerEffect>(); // isHit값 받기 위한 상호작용
        UpdateHPUI();
    }

    private void Update()
    {
        if (isAutoRecovering && hpRecoverCoroutine == null)
        {
            hpRecoverCoroutine = StartCoroutine(AutoHpRecoveryLoop());
        }

        HandleHpBarFade();
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

    IEnumerator StartGameOverUI()
    {
        yield return new WaitForSeconds(2f);
        Managers.UI.ShowPopUpUI<GameOver>();
    }

    public void GetDamaged(int damageAmount)
    {
        if (Managers.Game.isHit) return;

        if (hpFadeCoroutine != null)
        {
            StopCoroutine(hpFadeCoroutine);
            hpFadeCoroutine = null;
        }
        SetHpBarAlpha(1f);

        TriggerDamage();
        currentHP -= damageAmount;

        if (currentHP < 0)
                currentHP = 0;

        UpdateHPUI();

        isAutoRecovering = false;

        if (hpRecoverCoroutine != null)
        {
            StopCoroutine(hpRecoverCoroutine);
            hpRecoverCoroutine = null;
        }


        StartCoroutine(DelayRecovery());

        if (currentHP == 0 && !gameOver)
        {
            gameOver = true;
            Debug.Log("GameOver");
            playerMove.animator.SetTrigger("IsDead");
            Time.timeScale = 0;
            StartCoroutine(StartGameOverUI());
        }
    }

    private void SetHpBarAlpha(float alpha)
    {
        var color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    private IEnumerator FadeOutHpBar()
    {
        yield return new WaitForSeconds(1f);

        float duration = 1f;
        float elapsed = 0f;
        float startAlpha = spriteRenderer.color.a;
        float endAlpha = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            SetHpBarAlpha(alpha);
            Canvas.ForceUpdateCanvases();
            yield return null;
        }

        SetHpBarAlpha(0f);
        Canvas.ForceUpdateCanvases();
        hpFadeCoroutine = null;
    }

    private void HandleHpBarFade()
    {
        if (currentHP < 3)
        {
            if (hpFadeCoroutine != null)
            {
                StopCoroutine(hpFadeCoroutine);
                hpFadeCoroutine = null;
            }

            SetHpBarAlpha(1f);
        }
        else if (currentHP == 3)
        {
            if (hpFadeCoroutine == null)
            {
                hpFadeCoroutine = StartCoroutine(FadeOutHpBar());
            }
        }
    }

    private IEnumerator DelayRecovery()
    {
        yield return new WaitForSeconds(recoverDelayAfterHit);
        isAutoRecovering = true;
    }

    private IEnumerator AutoHpRecoveryLoop()
    {
        while (isAutoRecovering && currentHP < 3)
        {
            currentHP++;
            UpdateHPUI();
            Debug.Log("HP 자동 회복됨: " + currentHP);

            yield return new WaitForSeconds(hpRecoverInterval);
        }

        hpRecoverCoroutine = null;
    }

    private void UpdateHPUI()
    {

        if (currentHP >= 0 && currentHP < sprites.Length)
        {
            spriteRenderer.sprite = sprites[currentHP];
        }
    }
}
