using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using System;

public class UI_Manager
{
    public bool isPopuping = false;
    int _order = 10;

    Stack<UI_Popup> _popUpStack = new Stack<UI_Popup>();

    UI_Scene _sceneUI = null;

    public void ReSet()
    {
        isPopuping = false;
    }
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_root");
            if (root == null)
            {
                root = new GameObject { name = "@UI_root" };
            }
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true, bool renderScreen = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        ///// rendermode 를 결정해서 설정하자 
        if(renderScreen==true)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        else { canvas.renderMode = RenderMode.ScreenSpaceCamera; } 

        ///
        canvas.worldCamera = Camera.main;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 5;
        }
    }
    
    public void SetCanvasMost(GameObject go) // 캔버스 SortOrder을 가장 위로 올려주는 함수
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
      
        canvas.worldCamera = Camera.main;
        canvas.overrideSorting = true;

 
        canvas.sortingOrder = 199;
    }

    public void SetCanvasNumber(GameObject go,int sortNumber) // 캔버스 SortOrder을 sortNumber로 설정
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.worldCamera = Camera.main;
        canvas.overrideSorting = true;


        canvas.sortingOrder = sortNumber;
    }


    public T ShowAnyUI<T>(string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }
        GameObject go = Managers.Resource.Instantiate($"UI/{name}");
        T anyUI = Util.GetOrAddComponent<T>(go);
        return anyUI;
    }
    public T ShowPopUpUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/PopUP/{name}");
        T popUp = Util.GetOrAddComponent<T>(go);
        _popUpStack.Push(popUp);
        go.transform.SetParent(Root.transform);

        return popUp;
    }

    public T ShowPopUpUI_handleTarget<T>(GameObject target) where T : UI_Popup // target을 호출하는 대신 T를 PopUp하고, target을 PopUp으로 넘겨주는 함수
    {
        string name;
        name = typeof(T).Name;
     

        GameObject go = Managers.Resource.Instantiate($"UI/PopUP/{name}");
        T popUp = Util.GetOrAddComponent<T>(go);
        go.transform.SetParent(Root.transform);
        var method = typeof(T).GetMethod("Init", new Type[] { typeof(GameObject) }); // Init함수가 있다면 자동실행
        if (method != null)
        {
            method.Invoke(popUp, new object[] { target }); // { } 는 넘길 매개변수 목록 
        }
        return popUp;
    }

    public void ClosePopUp_handleTarget(UI_Popup popUp)
    {
        Managers.Resource.Destroy(popUp.gameObject);
    }


    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;
        go.transform.SetParent(Root.transform);
        return sceneUI;
    }

    public void ClosePopUpUI()
    {
        if (_popUpStack.Count == 0)
        {
            return;
        }
        
        UI_Popup popUP = _popUpStack.Pop();
        
        if(popUP==null)
        {
            return;
        }
        Managers.Resource.Destroy(popUP.gameObject);
        
        popUP = null;

        _order--;
    }
    public void ClosePopUpUI(UI_Popup popUp)
    {
        if (_popUpStack.Count == 0)
        {
            return;
        }
        if (_popUpStack.Peek() != popUp)
        {
            
            
            return;
        }
        ClosePopUpUI();
    }
    
   
    public void CloseAllPopUPUI()
    {
        while (_popUpStack.Count > 0)
        {
            ClosePopUpUI();
        }
        Time.timeScale = 1;
    }
    public void Clear()
    {
        CloseAllPopUPUI();
        _sceneUI = null;
    }
}
