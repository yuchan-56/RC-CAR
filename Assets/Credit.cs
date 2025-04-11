using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class Credit : UI_Popup
{
    public Image creditImage;
    enum Buttons
    {
        Click
    }
    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        StartCoroutine(creditDown());
        GetButton((int)Buttons.Click).gameObject.AddUIEvent(Clicked);
    }

    private void Clicked(PointerEventData eventData)
    {
        Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<Credit>(this.gameObject));
    }
    IEnumerator creditDown()
    {
        float FixedY = creditImage.transform.position.y;
        while(creditImage.transform.position.y<3000)
        {
            creditImage.transform.position += Vector3.up * 200f * Time.unscaledDeltaTime;
           yield return null;
        }
    }
}
