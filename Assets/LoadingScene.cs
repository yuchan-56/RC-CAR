using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScene : MonoBehaviour
{
    public Image LoadingPanel;
    public Image running;
    public Image loading;
    public Sprite[] runSprites;
    public Sprite[] loadingSprties;
    public static LoadingScene Instance
    {
        get
        {
            return instance;
        }
    }
    private static LoadingScene instance;

    void Start()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }
    public void GoLoading(string scene)
    {
        if (LoadingPanel == null)
        {
            Debug.LogError("LoadingPanel is not assigned!");
        }

        if (Managers.Scene == null)
        {
            Debug.LogError("Managers.Scene is not initialized!");
        }

        Debug.Log(scene);
        StartCoroutine(Loading(scene));
    }
    IEnumerator Loading(string scene)
    {
        LoadingPanel.DOFade(1f, 0.5f).SetUpdate(true);

        yield return new WaitForSecondsRealtime(0.5f);
        setActive(true);

        Managers.Scene.LoadScene(scene);


        yield return new WaitForSecondsRealtime(2f);
        Debug.Log("이곳에 진입했습니다");

        setActive(false);

        LoadingPanel.DOFade(0f, 0.3f).SetUpdate(true);
        Managers.UI.ReSet();
    }

    IEnumerator Animation_RunGo()
    {
        running.sprite = runSprites[5];
        for (int i = 0; i < runSprites.Length; i++)
        {
            running.sprite = runSprites[i];
            yield return new WaitForSecondsRealtime(0.1f);
        }
        StartCoroutine(Animation_RunGo());


    }
    IEnumerator Animation_LoadGo()
    {
        loading.sprite = loadingSprties[3];
        for (int i = 0; i < loadingSprties.Length; i++)
        {
            loading.sprite = loadingSprties[i];
            yield return new WaitForSecondsRealtime(0.1f);
        }
        StartCoroutine(Animation_LoadGo());
    }
    void setActive(bool tf)
    {
        running.gameObject.SetActive(tf);
        loading.gameObject.SetActive(tf);
        if(tf==true)
        {
            StartCoroutine(Animation_RunGo());
            StartCoroutine(Animation_LoadGo());
        }
        else { StopAllCoroutines(); }
    }
}

