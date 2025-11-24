using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Remnants
{
    public class LockDoor : Interactive
    {
        #region Variables
        //public AudioManager audioManager;

        [SerializeField]
        private string sequence = "";

        private TypewriterEffect typewriterEffect;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            typewriterEffect = this.GetComponent<TypewriterEffect>();
        }
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            StartCoroutine(StartTrigger());
        }

        IEnumerator StartTrigger()
        {
            typewriterEffect.StartTyping(sequence);

            yield return new WaitForSeconds(sequence.Length * typewriterEffect.typingSpeed + 2f);

            typewriterEffect.ClearText();
        }
        #endregion
    }

}
