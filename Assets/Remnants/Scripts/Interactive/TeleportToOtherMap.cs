using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Remnants
{
    public class TeleportToOtherMap : Interactive
    {
        private TypewriterEffect typewriterEffect;

        [SerializeField] private Transform otherMap;
        [SerializeField] private Volume urpVolume;
        [SerializeField] private Image[] crackImages;
        [SerializeField] private CanvasGroup fadePanel;

        [SerializeField] private float overlayInterval = 0.02f;
        [SerializeField] private float overlayHold = 0.5f;
        [SerializeField] private float fallDelay = 0.05f;
        [SerializeField] private float fallDistance = 300f;
        [SerializeField] private float fallTime = 1f;
        [SerializeField] private float fadeDuration = 1f;

        [SerializeField]
        private string sequence;

        private ColorAdjustments _colorAdjustments;
        private static bool _goToOtherMap = true;
        private bool _hasUsed;

        private void Awake()
        {
         
            if (urpVolume != null && urpVolume.profile.TryGet(out _colorAdjustments))
                _colorAdjustments.saturation.overrideState = true;

        }

        private void Start()
        {
            typewriterEffect = this.GetComponent<TypewriterEffect>();
        }

        protected override void DoAction()
        {
            if (!_hasUsed || (_hasUsed && !_goToOtherMap))
            {
                _hasUsed = true;
                TeleportState.Instance.HasVisitedRoom = true;

                StartCoroutine(MirrorBreakThenTeleport());

                if (TryGetComponent<Collider>(out var col)) col.enabled = false;
            }
        }
        private IEnumerator MirrorBreakThenTeleport()
        {
            typewriterEffect.StartTyping(sequence);
            yield return new WaitForSeconds(sequence.Length * typewriterEffect.typingSpeed + 2f);
            typewriterEffect.ClearText();

            // 모든 조각을 한 번에 켜기
            for (int i = 0; i < crackImages.Length; i++)
            {
                crackImages[i].gameObject.SetActive(true);
            }
            //  잠시 홀드
            yield return new WaitForSeconds(0.3f);

            if (_goToOtherMap)
            {
                AudioManager.Instance.Play("Mirrorbreaking");
            }
            else
            {
                
            }
            // 컬러 ↔ 흑백
            if (_goToOtherMap)
            {
                _colorAdjustments.saturation.value = -100f;
            }
            else
            {
                yield return FadeToColor();
            }

            //  원래 TeleportRoutine 실행
            yield return TeleportRoutine();

            StartCoroutine(CrackFallRoutine());
            yield return new WaitForSeconds(fallDelay * crackImages.Length + fallTime);

        }
        private IEnumerator TeleportRoutine()
        {


            //  순간이동
            TeleportPlayer();
          

            //  화면 원복
            yield return Fade(1, 0);
            fadePanel.gameObject.SetActive(false);

            _goToOtherMap = !_goToOtherMap;
            if (_goToOtherMap)
                gameObject.SetActive(false);
        }

        private IEnumerator Fade(float from, float to)
        {
            fadePanel.gameObject.SetActive(true);
            fadePanel.alpha = from;
            float t = 0;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                fadePanel.alpha = Mathf.Lerp(from, to, t / fadeDuration);
                yield return null;
            }
            fadePanel.alpha = to;
        }

        private IEnumerator FadeToColor()
        {
            float start = _colorAdjustments.saturation.value;
            float t = 0, dur = 2f;
            while (t < dur)
            {
                t += Time.deltaTime;
                _colorAdjustments.saturation.value = Mathf.Lerp(start, 0, t / dur);
                yield return null;
            }
            _colorAdjustments.saturation.value = 0;
        }

        private void TeleportPlayer()
        {
            var player = GameObject.FindWithTag("Player");
            var cc = player.GetComponent<CharacterController>();
            if (cc) cc.enabled = false;
            player.transform.position = otherMap.position + Vector3.up * .5f;
            if (cc) cc.enabled = true;
        }


        private IEnumerator CrackFallRoutine()
        {
            // crackImages 인덱스 리스트를 랜덤하게 섞기
            var indices = Enumerable.Range(0, crackImages.Length).ToList();
            for (int i = 0; i < indices.Count; i++)
            {
                int j = Random.Range(i, indices.Count);
                // swap
                var tmp = indices[i];
                indices[i] = indices[j];
                indices[j] = tmp;
            }

            //  셔플된 순서대로 각각 랜덤 낙하
            foreach (int idx in indices)
            {
                var img = crackImages[idx];
                var rt = img.rectTransform;

                //  랜덤한 짧은 지연 
                yield return new WaitForSeconds(Random.Range(0f, fallDelay));

                //  랜덤 거리, 랜덤 시간
                float thisDistance = Random.Range(fallDistance * 0.5f, fallDistance * 1.5f);
                float thisTime = Random.Range(fallTime * 0.8f, fallTime * 1.2f);

                // 랜덤 회전
                float thisAngle = Random.Range(-45f, 45f);

              
                rt.DOAnchorPosY(rt.anchoredPosition.y - thisDistance, thisTime)
                  .SetEase(Ease.InQuad);
                img.DOFade(0, thisTime);
                rt.DOLocalRotate(new Vector3(0, 0, thisAngle), thisTime);
            }

            // 3) 모두 애니 완료될 때까지 충분히 대기
            yield return new WaitForSeconds(fallTime * 1.5f);
        }

    }
}
