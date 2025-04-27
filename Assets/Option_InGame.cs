using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static LoadingScene;
using UnityEngine;

public class Option_InGame : UI_Popup
{
    enum Buttons
    {
        GoBack,
        Close,
        RePlay
    }
    public override void Init()
    {
        Time.timeScale = 0;
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Close).gameObject.AddUIEvent(GoClose);
        GetButton((int)Buttons.RePlay).gameObject.AddUIEvent(GoRePlay);
        GetButton((int)Buttons.GoBack).gameObject.AddUIEvent(GoGoBack);
    }
    // Start is called before the first frame update
    void GoClose(PointerEventData eventData)
    {
        Time.timeScale = 1;
        SoundManager.Instance.SFXPlay("Button Click");
        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<Option_InGame>(this.gameObject));
    }

    void GoRePlay(PointerEventData eventData)
    {
        Time.timeScale = 1;
        SoundManager.Instance.SFXPlay("Button Click");
        Scene scene = SceneManager.GetActiveScene();
        LoadingScene.Instance.GoLoading($"{scene.name}");
        
    }
    void GoGoBack(PointerEventData eventData)
    {
        Time.timeScale = 1;
        SoundManager.Instance.SFXPlay("Button Click");
        LoadingScene.Instance.GoLoading("MainTitle");
    }

    void Start()
    {
        Init();
        Managers.UI.SetCanvasNumber(this.gameObject, 1000);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
