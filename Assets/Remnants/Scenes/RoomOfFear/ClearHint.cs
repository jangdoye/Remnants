using TMPro;
using UnityEngine;
using System.Collections;

namespace Remnants
{
    public class ClearHint : TypewriterEffect
    {
        #region Variables  
        [SerializeField]
        private string sequence01 = "앞에 더이상 길이 없어!!";

        [SerializeField]
        private string sequence02 = "고민하지마! 벽쪽으로 힘것 뛰어!";

        [SerializeField]
        private string sequence03 = "한번 확인해봐!";
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                StartCoroutine(GameClearHint());
            }
        }
        #endregion

        #region Custom Method
        IEnumerator GameClearHint()
        {
            StartTyping(sequence01);
            yield return new WaitForSeconds(sequence01.Length * typingSpeed + 1.5f);

            StartTyping(sequence02);
            yield return new WaitForSeconds(sequence02.Length * typingSpeed + 1.5f);

            StartTyping(sequence03);
            yield return new WaitForSeconds(sequence02.Length * typingSpeed + 1.5f);

            ClearText();
        }
        #endregion
    }
}