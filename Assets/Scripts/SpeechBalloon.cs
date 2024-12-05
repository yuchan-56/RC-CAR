using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBalloon : MonoBehaviour
{
    public GameObject playerSpeech;
    public TMP_Text text;
    private void Start()
    {
        playerPopup();
    }

    private void FixedUpdate()
    {
        playerPopup();
    }
    void playerPopup()
    {
        this.transform.position = playerSpeech.transform.position;
        text.text = "playetext Å×½ºÆ®";
    }

}
