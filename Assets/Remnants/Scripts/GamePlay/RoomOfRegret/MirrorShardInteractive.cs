using System;
using System.Collections;
using UnityEngine;

namespace Remnants
{
    public class MirrorShardInteractive : Interactive
    {
        #region Variables
        private TypewriterEffect typewriterEffect;

        [SerializeField] private int index; // 조각 인덱스


        [SerializeField]
        private string sequence;
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
            StartCoroutine(Typing());    
        }

        IEnumerator Typing()
        {   
            // 퍼즐 매니저에 조각 수집 요청
            PuzzleManager.Instance.CollectPiece(index);

            this.gameObject.GetComponent<BoxCollider>().enabled = false;

            typewriterEffect.StartTyping(sequence);
            yield return new WaitForSeconds(sequence.Length * typewriterEffect.typingSpeed + 2f);
            typewriterEffect.ClearText();

            // 오브젝트 제거 
            Destroy(this.gameObject);
        }
        #endregion
    }
}