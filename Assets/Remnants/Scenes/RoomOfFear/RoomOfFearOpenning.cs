using UnityEngine;
using System.Collections;

namespace Remnants
{
    public class RoomOfFearOpenning : TypewriterEffect
    {
        #region Variables
        //플레이어 오브젝트
        public GameObject thePlayer;
        //페이더 객체
        public SceneFader fader;
        [SerializeField]
        private string sequence01 = "...Where am I?";
        [SerializeField]
        private string sequence02 = "I need get out of here";
        [SerializeField]
        private string sequence03 = " ";
        
        
        #endregion

        #region Unity Event Method
        private void Start()
        {
            AudioManager.Instance.PlayBgm("FearBgm");

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
            //1. 페이드인 연출 
            fader.FadeStart(2f);
            StartTyping(sequence01);
            yield return new WaitForSeconds(sequence01.Length * typingSpeed + 2f);

            StartTyping(sequence02);
            yield return new WaitForSeconds(sequence02.Length * typingSpeed + 2f);

            StartTyping(sequence03);
            yield return new WaitForSeconds(sequence03.Length * typingSpeed + 2f);

            ClearText();
        }
        #endregion

    }
}