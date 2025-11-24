using UnityEngine;
using UnityEngine.InputSystem;

namespace Remnants
{
    public class PauseUI : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "MainMenu";

        public GameObject pauseUI;
        public PlayerInput playerInput;
        #endregion

        #region Custom Method
        //new Input 연결
        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.started)
            {              
                Toggle();
            }
        }

        //esc키 누르면 UI 활성화, 다시 esc 키 누르면 UI 비활성화 - 토글키
        public void Toggle()
        {
            // "Story" 태그가 붙은 오브젝트가 현재 활성화되어 있다면 PauseUI를 띄우지 않음
            GameObject storyObject = GameObject.FindGameObjectWithTag("Story");
            if (storyObject != null && storyObject.activeInHierarchy)
            {
                return; // 그냥 빠져나감
            }

            pauseUI.SetActive(!pauseUI.activeSelf);

            if (pauseUI.activeSelf)  //창이 열린 상태
            {
                Time.timeScale = 0f;

                //커서 제어
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else //창이 닫힌 상태
            {
                Time.timeScale = 1f;

                //커서 제어
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }


        //메뉴가기 버튼 호출  
        public void Menu()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 1f;

            fader.FadeTo(loadToScene);
        }
        #endregion
    }
}