using System.Collections;
using UnityEngine;

namespace Remnants
{
    public class FirstTriggerSequenceText : TypewriterEffect
    {
        #region Variables
        //SequenceText에 출력될 text들
        [TextArea]
        public string sequenceOne;
        [TextArea]
        public string petSequenceOne;
        [TextArea]
        public string petSequenceTwo;
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                StartCoroutine(PlaySequence());
            }
        }
        #endregion

        #region Custom Method
        private IEnumerator PlaySequence()
        {
            StartTyping(sequenceOne);
            yield return new WaitForSeconds(sequenceOne.Length * typingSpeed + 2f);

            StartTyping(petSequenceOne);
            yield return new WaitForSeconds(petSequenceOne.Length * typingSpeed + 2f);

            StartTyping(petSequenceTwo);
            yield return new WaitForSeconds(petSequenceTwo.Length * typingSpeed + 2f);

            ClearText();
        }
        #endregion
    }

}
