using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGuide : UI_Popup
{
    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        Managers.UI.SetCanvasNumber(this.gameObject, 2); // SortOrder 2·Î
    }


}
