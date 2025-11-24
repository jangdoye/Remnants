using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using TMPro;

// 문 열린 후 페이드 아웃 효과 부모 클래스
namespace Remnants
{
    public abstract class FadeTriggerBase : MonoBehaviour
    {
        #region Variables
        // 플레이어 오브젝트
        public GameObject thePlayer;
        // 페이드 할 이미지
        public Image fadeImage;

        // 페이드 아웃 지속 시간
        [SerializeField]
        protected float fadeDuration = 2f;

        // 엔딩 대사 (배열 처리)
        public TextMeshProUGUI[] endingLines;

        // 대사 한 줄 페이드 효과 지속 시간
        [SerializeField]
        private float textFadeDuration = 1f;

        // 대사 한 줄 유지 시간
        [SerializeField]
        private float textDisplayDuration = 2f;

        // Main Menu Button
        public GameObject mainMenuButton;

        // 버튼 페이드용 캔버스 그룹
        public CanvasGroup menuButtonCanvasGroup;

        // 참조
        private AudioManager audioManager;
        #endregion

        #region Property
        // 자식 클래스에서 사용할 페이드 색상
        protected abstract Color FadeColor { get; }

        // 자식 클래스에서 사용할 BGM 이름
        protected abstract string EndingBgmName { get; }
        #endregion

        #region Unity Event Method
        private void Start()
        {
            // 참조
            audioManager = AudioManager.Instance;
        }
        private void Awake()
        {
            // 모든 텍스트 비활성화 + 알파값 0
            foreach (var line in endingLines)
            {
                line.gameObject.SetActive(false);
                Color c = line.color;
                c.a = 0f;
                line.color = c;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            // 플레이어 체크
            if (other.tag == "Player")
            {
                // 트리거 해제
                this.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(SequencePlayer());

                // BGM 중지
                audioManager.StopBgm();
            }
        }
        #endregion

        #region Custom Method
        IEnumerator SequencePlayer()
        {
            // 플레이 캐릭터 비활성화(플레이 멈춤)
            PlayerInput input = thePlayer.GetComponent<PlayerInput>();
            input.enabled = false;

            // 페이드 아웃 효과 연출
            yield return StartCoroutine(FadeOutImage(FadeColor, fadeDuration));

            // 페이드 아웃 후 BGM 재생
            if(!string.IsNullOrEmpty(EndingBgmName))
            {
                audioManager.PlayBgm(EndingBgmName);
            }

            // 대사 표시 코루틴 시작
            yield return StartCoroutine(PlayEndingLines());
        }

        IEnumerator FadeOutImage(Color color, float duration)
        {
            float elapsed = 0f;

            // 시작 색상 - 투명
            Color startColor = color;
            startColor.a = 0f;

            // 종료 색상 - 완전 불투명
            Color endColor = color;
            endColor.a = 1f;

            // 페이드 이미지 활성화
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = startColor;

            // 경과 시간에 따라 점점 불투명해짐
            while (elapsed < duration)
            {
                fadeImage.color = Color.Lerp(startColor, endColor, (elapsed / duration));
                elapsed += Time.deltaTime;
                yield return null;
            }

            // 최종 색상
            fadeImage.color = endColor;
        }

        // 엔딩 대사 플레이
        IEnumerator PlayEndingLines()
        {
            for (int i = 0; i < endingLines.Length; i++) 
            {
                    // 이전 대사 비활성화
                    foreach (var l in endingLines)
                    {
                        l.gameObject.SetActive(false);
                    }

                    // 마지막 대사만 페이드 인만 하고 유지
                    if(i == endingLines.Length - 1)
                    {
                        yield return StartCoroutine(FadeTextIn(endingLines[i]));

                        // 메인 메뉴 버튼 보여주기
                        menuButtonCanvasGroup.gameObject.SetActive(true);
                        yield return StartCoroutine(FadeInButton(menuButtonCanvasGroup, 1.2f));
                    }
                    else
                    {
                        yield return StartCoroutine(FadeTextInOut(endingLines[i]));
                    } 
                }
            }

        // 엔딩 대사 나타나기, 숨기기
        IEnumerator FadeTextInOut(TextMeshProUGUI text)
        {
            // 시작 전 모든 텍스트 숨김
            text.gameObject.SetActive(true);

            Color color = text.color;
            color.a = 0f;
            text.color = color;

            // 대사 나타나기
            float elapsed = 0f;
            while (elapsed < textFadeDuration)
            {
                color.a = Mathf.Lerp(0f, 1f, (elapsed / textFadeDuration));
                text.color = color;
                elapsed += Time.deltaTime;
                yield return null;
            }
            color.a = 1f;
            text.color = color;

            // 유지 시간
            yield return new WaitForSeconds(textDisplayDuration);

            // 대사 사라지기
            elapsed = 0f;
            while (elapsed < textFadeDuration)
            {
                color.a = Mathf.Lerp(1f, 0f, (elapsed / textFadeDuration));
                text.color = color;
                elapsed += Time.deltaTime;
                yield return null;
            }
            color.a = 0f;
            text.color = color;
            text.gameObject.SetActive(false);
        }

        // 대사 나타나기만
        IEnumerator FadeTextIn(TextMeshProUGUI text)
        {
            // 시작 전 모든 텍스트 숨김
            text.gameObject.SetActive(true);

            Color color = text.color;
            color.a = 0f;
            text.color = color;

            // 대사 나타나기
            float elapsed = 0f;
            while (elapsed < textFadeDuration)
            {
                color.a = Mathf.Lerp(0f, 1f, (elapsed / textFadeDuration));
                text.color = color;
                elapsed += Time.deltaTime;
                yield return null;
            }
            color.a = 1f;
            text.color = color;
        }

        // 버튼 페이드 인
        IEnumerator FadeInButton(CanvasGroup canvasGroup, float duration)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float smoothAlpha = Mathf.SmoothStep(0f, 1f, t);
                canvasGroup.alpha = smoothAlpha;

                elapsed += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        #endregion
    }
}