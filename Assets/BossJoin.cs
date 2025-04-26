using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class BossJoin : UI_Popup
    {
        public Image image;
        IEnumerator showImage()
        {
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(2f);
            Time.timeScale = 1;
            Managers.UI.ClosePopUp_handleTarget(Util.GetOrAddComponent<BossJoin>(this.gameObject));
    }
        public void setBossImage(string boss) // Graphic,Programming,Sound,System
        {
            image.sprite = Resources.Load<Sprite>($"EnemyBoss/{boss}");
            StartCoroutine(showImage());
    }
    }
