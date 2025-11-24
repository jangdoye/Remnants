using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

namespace Remnants
{
    //타이틀 UI 콘트롤러
    public class TitleUI : MonoBehaviour
    {
        #region Variables
        //참조
        private AudioManager audioManager;
        public Animator doorAnimator;
        public Transform cameraTarget;
        public TextMeshProUGUI titleText;
        public GameObject anyKey;
        [SerializeField] 
        private float cameraMoveSpeed = 2f;

        public Image fadeImage; // Canvas의 흰색 Panel Image
        public float fadeDuration = 2f;

        private bool isTriggered = false;

        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            audioManager = AudioManager.Instance;
            //배경음 플레이
            audioManager.PlayBgm("TitleBgm");

            StartCoroutine(StartText());
        }
        private void Update()
        {
            if (!isTriggered && Input.anyKeyDown)
            {
                StartSequence();
            }

            // 카메라가 이동 중이면 타겟 쪽으로 이동
            if (isTriggered)
            {
                Camera.main.transform.position = Vector3.Lerp(
                    Camera.main.transform.position,
                    cameraTarget.position,
                    Time.deltaTime * cameraMoveSpeed
                );
            }
        }
        #endregion

        #region Custom Method
        private void StartSequence()
        {
            doorAnimator.SetTrigger("DoorOpen");
            StartCoroutine(FadeToWhiteAndLoad());
        }
        IEnumerator StartText()
        {
            yield return new WaitForSeconds(1f);
            anyKey.SetActive(true);

        }
        IEnumerator FadeToWhiteAndLoad()
        {
            float timer = 0f;
            Color color = fadeImage.color;

            yield return new WaitForSeconds(0.7f);
            isTriggered = true;

            audioManager.PlayBgm("TitleLight");
            // 2초에 걸쳐 알파 증가
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                fadeImage.color = new Color(color.r, color.g, color.b, alpha);
                anyKey.SetActive(false);

                float titleAlpha = Mathf.Lerp(0f, 4f, timer / fadeDuration);
                titleText.color = new Color(color.r, color.g, color.b, 1- titleAlpha);

                yield return null;
            }
            
            yield return new WaitForSeconds(0.5f); // 약간의 여유
            audioManager.StopBgm();
            SceneManager.LoadScene("MainMenu");
        }
        #endregion
    }
}