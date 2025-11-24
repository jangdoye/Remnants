using System.Collections;
using TMPro;
using UnityEngine;

namespace Remnants
{
    // TextMeshPro 텍스트에 한 글자씩 출력되도록 타이핑 효과를 적용하는 컴포넌트
    public class TypewriterEffect : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        // 타이핑 효과를 적용할 대상 텍스트 (TextMeshProUGUI 컴포넌트)
        protected TMP_Text targetText;

        // 글자 하나당 출력 지연 시간 (초)
        [SerializeField]
        public float typingSpeed = 0.05f;

        // 현재 실행 중인 타이핑 코루틴을 저장
        protected Coroutine typingCoroutine;
        #endregion

        #region Unity Event Method
        // 유니티 기본 이벤트 메서드는 현재 사용되지 않음
        #endregion

        #region Custom Method

        /// <summary>
        /// 타이핑 효과 시작. 기존 출력 중인 효과가 있다면 중단하고 새로 시작함.
        /// </summary>
        /// <param name="text">타이핑할 텍스트</param>
        public virtual void StartTyping(string text)
        {
            // 이미 실행 중인 코루틴이 있다면 중지
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            // 새 타이핑 시작
            typingCoroutine = StartCoroutine(TypeText(text));
        }

        /// <summary>
        /// 내부 코루틴: 텍스트를 한 글자씩 출력함.
        /// </summary>
        /// <param name="text">출력할 전체 텍스트</param>
        protected virtual IEnumerator TypeText(string text)
        {
            targetText.text = "";

            // 글자 하나씩 추가하며 타이핑 효과 생성
            foreach (char letter in text)
            {
                targetText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        /// <summary>
        /// 현재 실행 중인 타이핑을 강제로 중지함.
        /// </summary>
        public void StopTyping()
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }
        }

        /// <summary>
        /// 출력 중인 텍스트를 즉시 제거함.
        /// </summary>
        public void ClearText()
        {
            targetText.text = "";
        }

        #endregion
    }
}
