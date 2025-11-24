using UnityEngine;
using UnityEngine.Audio;

namespace Remnants
{
    public class PlayerBlackholeSuck : MonoBehaviour
    {
        public Transform targetObject;   // 빨려갈 액자/포탈 등
        public float duration = 1.0f;
        public AnimationCurve curve = null;

        public ParticleSystem blackholeParticle;

        private Vector3 startPos;
        private Quaternion startRot;
        private float timer = 0f;
        private bool isSucking = false;

        private Camera cam;
        private float startFOV;
        public float endFOV = 90f;  // 필요하면 FOV 효과

        void Start()
        {
            if (curve == null)
                curve = AnimationCurve.EaseInOut(0, 0, 1, 1.2f);

            cam = Camera.main;
            if (cam != null)
                startFOV = cam.fieldOfView;
        }

        public void StartSuck()
        {
            if (isSucking || targetObject == null) return;

            startPos = transform.position;
            startRot = transform.rotation;
            timer = 0f;
            isSucking = true;
        }

        void Update()
        {
            if (!isSucking || targetObject == null) return;

            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            float ct = curve.Evaluate(t);

            //  위치 이동 (targetObject 앞으로)
            Vector3 targetPos = targetObject.position + targetObject.forward * 0.3f;
            transform.position = Vector3.Lerp(startPos, targetPos, ct);

            //  회전 (targetObject 바라보게)
            Quaternion targetRot = Quaternion.LookRotation(targetObject.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, ct);

            //  FOV(화각) 변화 (선택)
            if (cam != null)
                cam.fieldOfView = Mathf.Lerp(startFOV, endFOV, ct);

            //  화면 흔들림(Shake) 효과
            float shakeStrength = 0.07f * (1 - t);
            if (shakeStrength > 0.01f)
            {
                Vector3 shake = Random.insideUnitSphere * shakeStrength;
                transform.position += shake;
            }

            //  종료 처리
            if (t >= 1.0f)
            {
                isSucking = false;
                if (cam != null)
                    cam.fieldOfView = startFOV;

                // 파티클 끄기(옵션)
                if (blackholeParticle != null)
                    blackholeParticle.Stop();
            }
        }
    }
}