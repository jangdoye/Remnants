using UnityEngine;
using UnityEngine.InputSystem;

namespace Remnants
{
    // 엔딩 룸만의 pause UI
    public class EndingRoomPauseUI : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "MainMenu";

        public GameObject pauseUI;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            // PlayerInput 없이도 ESC 키 감지
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Toggle();
            }
        }
        #endregion

        #region Custom Method

        //esc키 누르면 UI 활성화, 다시 esc 키 누르면 UI 비활성화 - 토글키
        public void Toggle()
        {
            pauseUI.SetActive(!pauseUI.activeSelf);

            if (pauseUI.activeSelf)  //창이 열린 상태
            {
                Time.timeScale = 0f;
            }
            else //창이 닫힌 상태
            {
                Time.timeScale = 1f;
            }
        }


        //메뉴가기 버튼 호출 
        public void Menu()
        {
            Time.timeScale = 1f;

            fader.FadeTo(loadToScene);
        }
        #endregion
    }
}

