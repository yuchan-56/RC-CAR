using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class BossJoin : UI_Popup
    {
        public Image image;
        private void Start()
        {
            StartCoroutine(showImage());
        }

        IEnumerator showImage()
        {
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(0.4f);
            Time.timeScale = 1;
            Managers.UI.ClosePopUpUI(Util.GetOrAddComponent<BossJoin>(this.gameObject));
        }
        public void setBossImage(string boss) // Graphic,Programming,Sound,System
        {
            image.sprite = Resources.Load<Sprite>($"EnemyBoss/{boss}");

        }
    }
