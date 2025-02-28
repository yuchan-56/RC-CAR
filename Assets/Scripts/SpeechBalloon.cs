using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBalloon : UI_Popup
{
    public GameObject target;
    public TMP_Text text;
    private void Start()
    {
        target = Managers.Speech.speechTmp;
        playerPopup();
        StartCoroutine(PopUping_timeSet(3));
    }

    private void FixedUpdate()
    {
        playerPopup();
    }
    void playerPopup()
    {
        this.transform.position = target.transform.position;
        text.text = "playetext Test";
    }

    IEnumerator PopUping_timeSet(int time)
    {
        yield return new WaitForSeconds(time);

        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<SpeechBalloon>(this.gameObject));
        yield return null;
    }
   
}
