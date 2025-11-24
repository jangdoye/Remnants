using System.Collections;
using TMPro;
using UnityEngine;

namespace Remnants
{
    public class RoomOfAngerExit : Interactive
    {
        #region Variablse
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "Lobby";

        public Animator animator;

        [SerializeField]
        private string sequnce = "나는 그 죄를 모두 기억하고, 내 안에서 껴안기로 했다";

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
            StartCoroutine(Clear());            
        }

        IEnumerator Clear()
        {
            typewriterEffect.StartTyping(sequnce);

            this.GetComponent<CapsuleCollider>().enabled = false;

            yield return new WaitForSeconds(sequnce.Length * typewriterEffect.typingSpeed + 2f);

            typewriterEffect.ClearText();

            animator.SetBool("IsClear", true);

            yield return new WaitForSeconds(1f);

            AudioManager.Instance.StopBgm();

            fader.FadeTo(loadToScene);
        }
        #endregion
    }

}
