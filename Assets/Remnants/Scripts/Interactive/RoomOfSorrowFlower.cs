using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Remnants
{
    public class RoomOfSorrowFlower : Interactive
    {
        #region Variables
        public SceneFader whiteFader;
        [SerializeField]
        private string loadToScene = "RoomOfSorrow_2";

        [SerializeField]
        private string sequence = "Sequence";

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
            StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            typewriterEffect.StartTyping(sequence);

            yield return new WaitForSeconds(sequence.Length * typewriterEffect.typingSpeed + 2f);

            typewriterEffect.ClearText();

            whiteFader.FadeTo(loadToScene);
        }
        #endregion
    }

}
