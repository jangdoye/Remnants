using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Remnants
{
    public class RoomOfVoidOpenning : MonoBehaviour
    {
        #region Variables
        //플레이어 오브젝트
        public GameObject thePlayer;
        public AudioManager audioManager;
        //페이더 객체
        public SceneFader fader;

        private TypewriterEffect typewriterEffect;

        [SerializeField]
        private string sequence01 = "...Where am I?";

        [SerializeField]
        private string sequence02 = "I need get out of here";

        #endregion

        #region Unity Event Method
        private void Start()
        {
            PlayerDataManager.Instance.SceneNumber = SceneManager.GetActiveScene().buildIndex;
            SaveLoad.SaveData();

            typewriterEffect = this.GetComponent<TypewriterEffect>();

            //커서 제어
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            audioManager.Play("VoidBgm");

            //오프닝 연출 시작
            StartCoroutine(SequencePlay());
        }
        #endregion

        #region Custom Method
        //오프닝 연출 코루틴 함수
        IEnumerator SequencePlay()
        {
            //1. 페이드인 연출 (1초 대기후 페인드인 효과)
            fader.FadeStart(1f);

            typewriterEffect.StartTyping(sequence01);
            yield return new WaitForSeconds(3f);

            typewriterEffect.StartTyping(sequence02);
            yield return new WaitForSeconds(3f);

            typewriterEffect.ClearText();

        }
        #endregion
    }
}

