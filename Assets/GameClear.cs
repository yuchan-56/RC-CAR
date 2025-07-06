using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : UI_Popup
{
    enum Buttons
    {
        ClearButton
    }

    private void Start()
    {
        Init();
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.ClearButton).gameObject.AddUIEvent(GameClearClicked);

        // ¿˙¿Â
        int stage = PlayerPrefs.GetInt("StageData");
        if (stage == 1 && SceneManager.GetActiveScene().name == "S1_1") stage++;
        if (stage == 2 && SceneManager.GetActiveScene().name == "S1_2") stage++;
        if (stage == 3 && SceneManager.GetActiveScene().name == "S1_3") stage++;
        if (stage == 4 && SceneManager.GetActiveScene().name == "S1_4") stage++;
        PlayerPrefs.SetInt("StageData", stage);
        PlayerPrefs.Save();

        SoundManager.Instance.AudioStop();
        SoundManager.Instance.SFXPlay("Stage Clear");
    }
    void GameClearClicked(PointerEventData eventData)
    {
        SoundManager.Instance.SFXPlay("Button Click");
        LoadingScene.Instance.GoLoading("MainTitle");
        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<GameClear>(this.gameObject));
        Managers.Clear();
        Destroy(this.gameObject);
    }
}
