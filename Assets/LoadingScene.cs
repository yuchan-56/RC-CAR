using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScene : MonoBehaviour
{
    public Image LoadingPanel;
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
        Managers.Scene.LoadScene(scene);

        yield return new WaitForSecondsRealtime(0.3f);
        Debug.Log("�̰��� �����߽��ϴ�");
        LoadingPanel.DOFade(0f, 0.3f).SetUpdate(true);

        Time.timeScale = 1;
    }
}
