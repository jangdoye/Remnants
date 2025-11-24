using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Remnants
{
    //플레이 씬 오프닝 연출
    public class AOpenning : MonoBehaviour
    {
        #region Variables
        //플레이어 오브젝트
        public GameObject thePlayer;
        //페이더 객체
        public SceneFader fader;

        [SerializeField]
        private string sequence01 = "여기가 어디야?";

        [SerializeField]
        private bool isWhite = false;

        //AOpenning을 사용 할 때에는 이 typewriterEffect를 같이 사용해야 오류가 안 생김
        private TypewriterEffect typewriterEffect;

        [SerializeField]
        private string bgmName;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            typewriterEffect = this.GetComponent<TypewriterEffect>();
        }

        private void Start()
        {
            PlayerDataManager.Instance.SceneNumber = SceneManager.GetActiveScene().buildIndex;
            SaveLoad.SaveData();

            //커서 제어
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //오프닝 연출 시작
            StartCoroutine(SequencePlay());
        }
        #endregion

        #region Custom Method
        //오프닝 연출 코루틴 함수
        IEnumerator SequencePlay()
        {
            PlayerInput input = thePlayer.GetComponent<PlayerInput>();
            input.enabled = false;

            // 페이드인 연출 (1초 대기후 페인드인 효과)
            fader.FadeStart(1f, isWhite);

            AudioManager.Instance.PlayBgm(bgmName);

            //화면 하단에 시나리오 텍스트 화면 출력
            if(sequence01 != "")
            {
                typewriterEffect.StartTyping(sequence01);
                yield return new WaitForSeconds(sequence01.Length * typewriterEffect.typingSpeed + 3f);
                typewriterEffect.ClearText();
            }
            
            input.enabled = true;
        }
        #endregion
    }
}