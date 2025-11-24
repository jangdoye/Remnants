using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Remnants
{
    /// <summary>
    /// 특정 트리거 구역에 플레이어가 진입했을 때 이벤트(텍스트 출력, 펫 활성화 등)를 발생시키는 클래스
    /// </summary>
    public class FirstTrigger : TypewriterEffect // 부모 클래스 Trigger를 상속
    {
        #region Variables
        private PlayerInput player;

        // 등장시킬 펫 오브젝트
        public GameObject pet;

        [SerializeField]
        private List<GameObject> soundsObjects;

        [SerializeField]
        private int activeIndex = 0;

        [SerializeField]
        private string sequence;

        [SerializeField]
        private bool IsStop = false;
        // 펫을 활성화할지 여부를 설정하는 변수 (Inspector에서 설정 가능)
        [SerializeField]
        private bool enabledPet = false;

        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                player = other.GetComponent<PlayerInput>();
                StartCoroutine(StartTrigger());
                if (soundsObjects == null || soundsObjects.Count < 2)
                {
                    Debug.LogWarning($"soundsObjects 리스트가 비어있거나 충분하지 않습니다. 현재 Count: {soundsObjects?.Count ?? 0}");
                    return;
                }
                DistanceBgm stopSounds = soundsObjects[0].GetComponent<DistanceBgm>();
                foreach (var stopSound in stopSounds.sounds)
                {
                    stopSounds.audioManager.Stop(stopSound);
                }
                soundsObjects[0].SetActive(false);

                soundsObjects[1].SetActive(true);

                stopSounds.FalsePlaying();
            }
        }
        #endregion

        #region Custom Method

        /// <summary>
        /// 트리거가 시작될 때 호출되는 코루틴 함수
        /// </summary>
        IEnumerator StartTrigger()
        {
            // 플레이어가 존재할 경우
            if (IsStop)
            {
                // 플레이어 비활성화 (트리거 연출 중 조작 방지)
                player.enabled = false;

                // 트리거 재실행 방지
                DisableAllColliders();

                // 연출용 텍스트 출력
                StartTyping(sequence);

                // 연출 시간 대기
                yield return new WaitForSeconds(sequence.Length * typingSpeed + 2f);

                // 플레이어 다시 활성화
                player.enabled = true;

                // 펫 활성화 설정이 true이고, pet 오브젝트가 존재하면 활성화
                if (enabledPet && pet != null)
                {
                    pet.SetActive(true);
                }

                // 연출 텍스트 제거
                ClearText();
            }
            else
            {
                // 플레이어가 존재하지 않을 경우에도 연출은 진행됨

                // 트리거 재실행 방지
                DisableAllColliders();

                if (sequence != "")
                {
                    // 연출용 텍스트 출력
                    StartTyping(sequence);
                }
                // 연출 시간 대기
                yield return new WaitForSeconds(sequence.Length * typingSpeed + 2f);

                // 펫 활성화 설정이 true이고, pet 오브젝트가 존재하면 활성화
                if (enabledPet && pet != null)
                {
                    pet.SetActive(true);
                }

                // 연출 텍스트 제거
                ClearText();
            }
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
