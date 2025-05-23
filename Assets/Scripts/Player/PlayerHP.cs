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
    private bool isAutoRecovering = false;
    private Coroutine hpRecoverCoroutine;
    private float recoverDelayAfterHit = 10f;
    private float hpRecoverInterval = 2f;
    private Coroutine hpFadeCoroutine;


    private bool gameOver = false;
    private Rigidbody2D rb;
    void Start()
    {
        rb = playerMove.gameObject.GetComponent<Rigidbody2D>();
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
            SoundManager.Instance.SFXPlay("Player Hit");
            StartCoroutine(ResetDamageState());
        }
    }

    private IEnumerator ResetDamageState()  
    {
        
        yield return new WaitForSeconds(animationDuration);
        playerMove.animator.SetTrigger("GetDamagedDisable");
    }

    public IEnumerator StartGameOverUI()
    {
        StartCoroutine(CheckGroundFallingWhenDead());
        yield return new WaitForSecondsRealtime(1.3f);
        Managers.UI.ShowPopUpUI<GameOver>();
    }
    IEnumerator CheckGroundFallingWhenDead()
    {
        // 물리엔진 수동 모드
        Physics2D.autoSimulation = false;

        

        while (playerMove.isground == false)
        {
            Physics2D.Simulate(Time.unscaledDeltaTime);

            yield return null; // 중요! 프레임 넘겨줘야 함
        }
        Physics2D.autoSimulation = true;

        yield return null;
    }

    public void GetDamaged(int damageAmount)
    {
        
        if (Managers.Game.isHit) return;


        if(Managers.Game.gage < 100)
        {
            Managers.Game.gage += 5;
        }

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

        if (currentHP <= 0 && !gameOver)
        {
            gameOver = true;
            foreach (var param in playerMove.animator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger)
                {
                    playerMove.animator.ResetTrigger(param.name);
                }
            }
            
            dieAnimation();
            Time.timeScale = 0;
            StartCoroutine(StartGameOverUI());
        }
    }

    public void dieAnimation()
    {
        playerMove.animator.SetTrigger("IsDead");
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
