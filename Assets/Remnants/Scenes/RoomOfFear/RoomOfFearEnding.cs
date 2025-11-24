using System.Collections;
using UnityEngine;

namespace Remnants
{
    public class RoomOfFearEnding : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "Lobby";

        //public AudioSource doorBang;    //문여는 소리
        //public AudioSource bgm01;       //배경음
        #endregion

        #region Custom Method
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                GameClear();
            }
        }
        private void GameClear()
        {
            //씬 클리어 처리
            AudioManager.Instance.StopBgm();

            //커서제어
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
            //메인메뉴가기
            fader.FadeTo(loadToScene);
        }
        #endregion

    }
}