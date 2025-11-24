using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Remnants
{
    public class HandRandomMover : MonoBehaviour
    {
        [Header("움직일 관절들")]
        public string[] jointNames = { "Hand", "RightForeArm" };
        private List<Transform> joints = new List<Transform>(); // 손가락 관절들 (손가락1, 손가락2 등)

        [Header("회전 범위 설정")]
        [SerializeField] private float minAngle = -20f;
        [SerializeField] private float maxAngle = 20f;

        [Header("움직임 속도 설정")]
        [SerializeField] private float transitionDuration = 0.5f;
        [SerializeField] private float rangeSpeed = 0.5f;

        private void Awake()
        {
            joints.Clear();
            foreach (Transform child in GetComponentsInChildren<Transform>())
            {
                foreach (string name in jointNames)
                {
                    if (child.name == name)
                    {
                        joints.Add(child);
                        break;
                    }
                }
            }
        }
        void Start()
        {
            StartCoroutine(RandomMoveLoop());
        }

        IEnumerator RandomMoveLoop()
        {
            while (true)
            {
                List<Quaternion> startRotations = new List<Quaternion>();
                List<Quaternion> targetRotations = new List<Quaternion>();

                // 시작/목표 회전값 저장
                foreach (Transform joint in joints)
                {
                    startRotations.Add(joint.localRotation);

                    Vector3 randomEuler = new Vector3(
                        Random.Range(minAngle, maxAngle),
                        Random.Range(minAngle, maxAngle),
                        Random.Range(minAngle, maxAngle)
                    );
                    targetRotations.Add(Quaternion.Euler(randomEuler));
                }

                float elapsed = 0f;
                while (elapsed < transitionDuration)
                {
                    elapsed += Time.deltaTime;
                    float t = elapsed / transitionDuration;

                    for (int i = 0; i < joints.Count; i++)
                    {
                        joints[i].localRotation = Quaternion.Lerp(startRotations[i], targetRotations[i], t);
                    }

                    yield return null;
                }

                yield return new WaitForSeconds(rangeSpeed); // 다음 랜덤값까지 대기
            }
        }
    }
}