using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Remnants
{
    public class SecondTrigger : TypewriterEffect
    {
        #region Variables
        //참조
        private PlayerController playerController;

        [SerializeField]
        private string sequence;
        [SerializeField]
        private Vector3 lookRotationEuler;
        private Quaternion originTransfrom;
        private float rotationTime;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(StartTrigger());
            }
        }
        #endregion

        #region Custom Method
        IEnumerator StartTrigger()
        {
            // 플레이어 트랜스폼 참조
            Transform playerTransform = playerController.transform;

            // 현재 회전값 저장 (되돌릴 때 사용)
            originTransfrom = playerTransform.rotation;

            // 플레이어 컨트롤러 비활성화 (움직임 차단)
            playerController.enabled = false;

            // 목표 회전값 (절대값 기준)
            Quaternion targetRotation = Quaternion.Euler(0f, lookRotationEuler.y, 0f);

            // 회전에 걸릴 시간
            float duration = 0.5f;
            float elapsed = 0f;

            // ▷ 부드럽게 플레이어 회전 (현재 → 목표)
            while (elapsed < duration)
            {
                // 회전 보간 처리 (Slerp로 부드럽게)
                playerTransform.rotation = Quaternion.Slerp(originTransfrom, targetRotation, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // 마지막 프레임에서 목표 회전값 정확히 고정
            playerTransform.rotation = targetRotation;

            DisableAllColliders();

            // ▷ 텍스트 출력 (3초 동안)
            // 연출용 텍스트 출력
            StartTyping(sequence);

            // 연출 시간 대기
            yield return new WaitForSeconds(sequence.Length * typingSpeed + 2f);
            ClearText();

            // ▷ 다시 원래 방향으로 천천히 회전 복구
            elapsed = 0f;
            while (elapsed < duration)
            {
                playerTransform.rotation = Quaternion.Slerp(targetRotation, originTransfrom, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // 원래 회전값으로 정확히 복귀
            playerTransform.rotation = originTransfrom;

            // ▷ 플레이어 컨트롤러 다시 활성화
            playerController.enabled = true;
        }

        /// <summary>
        /// 이 트리거에 붙은 모든 BoxCollider를 비활성화하는 함수 (재진입 방지용)
        /// </summary>
        private void DisableAllColliders()
        {
            // 해당 게임 오브젝트에 있는 모든 BoxCollider 가져오기
            BoxCollider[] colliders = GetComponents<BoxCollider>();

            // 각각 비활성화
            foreach (BoxCollider collider in colliders)
            {
                collider.enabled = false;
            }
        }
        #endregion
    }

}
