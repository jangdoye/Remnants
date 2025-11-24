using UnityEngine;
using System.Collections;

namespace Remnants
{
    //공허의방 출구에 가까워지면 발생하는 Trigger
    public class SoundTrigger : TypewriterEffect
    {
        #region Variables
        public GameObject lastTrigger;

        [SerializeField]
        private string sequence01 = "";
        [SerializeField]
        private string sequence02 = "";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            lastTrigger.SetActive(false);
        }
        private void OnTriggerEnter(Collider other)
        {
            //플레이어 체크
            if (other.tag == "Player")
            {
                //트리거 해제
                this.GetComponent<SphereCollider>().enabled = false;
                lastTrigger.SetActive(true);
                StartCoroutine(SequencePlayer());
            }
        }
        #endregion

        #region Custom Method
        IEnumerator SequencePlayer()
        {
            StartTyping(sequence01);
            yield return new WaitForSeconds(sequence01.Length * typingSpeed + 2f);

            StartTyping(sequence02);
            yield return new WaitForSeconds(sequence02.Length * typingSpeed + 2f);

            ClearText();
        }
        #endregion

    }
}