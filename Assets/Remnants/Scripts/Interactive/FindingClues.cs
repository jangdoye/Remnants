using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Remnants
{
    //단서를 찾는 Interactive의 상속받는 클래스
    public class FindingClues : Interactive
    {
        #region Variables
        //참조
        private DisapperEffect disapperEffect;

        [SerializeField]
        private string sequence = "Find Clue";
        private string notClueText = "이게 아니야..";

        [SerializeField]
        private bool isClue = false;

        private TypewriterEffect typewriterEffect;
        #endregion

        #region Property
        public bool IsClue
        {
            get
            {
                return isClue;
            }
        }
        #endregion

        #region Unity Event Method
        private void Start()
        {
            disapperEffect = this.GetComponent<DisapperEffect>();
            typewriterEffect = this.GetComponent<TypewriterEffect>();
        }
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            StartCoroutine(FindingClue());
        }

        IEnumerator FindingClue()
        {
            if (!IsClue)
            {
                typewriterEffect.StartTyping(notClueText);
                yield return new WaitForSeconds(notClueText.Length * typewriterEffect.typingSpeed + 3f);
                typewriterEffect.ClearText();
            }
            else
            {
                typewriterEffect.StartTyping(sequence);
                yield return new WaitForSeconds(sequence.Length * typewriterEffect.typingSpeed + 3f);
                typewriterEffect.ClearText();

                if (disapperEffect != null)
                {
                    AudioManager.Instance.Play("FadeOutSound");
                    disapperEffect.StartDisapper();
                }
            }

        }


        #endregion
    }

}