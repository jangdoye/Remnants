using UnityEngine;

namespace Remnants
{
    // 플레이어를 일정 시간 동안 특정 방향으로 밀어주는 기능을 담당하는 클래스
    public class PushHandler : MonoBehaviour
    {
        #region Variables

        private CharacterController controller;  // 밀어낼 대상 플레이어의 CharacterController
        private Vector3 dir;                     // 밀어낼 방향
        private float distance;                  // 밀어낼 거리(속도에 반영됨)
        private float duration;                  // 밀림이 지속될 시간
        private float timer;                     // 현재까지 경과한 시간

        #endregion

        #region Unity Event Method

        // 매 프레임마다 호출되어 밀림 효과를 적용
        private void Update()
        {
            if (controller == null) return; // 대상이 없으면 실행하지 않음

            if (timer < duration)
            {
                // 지정된 방향으로 일정 시간 동안 이동 (프레임 보정)
                controller.Move(dir * distance * Time.deltaTime);
                timer += Time.deltaTime; // 시간 누적
            }
            else
            {
                // 밀림이 끝났으면 이 핸들러 오브젝트를 제거
                Destroy(gameObject);
            }
        }

        #endregion

        #region Custom Method

        /// <summary>
        /// 외부에서 밀기 정보를 받아 초기화하는 메서드
        /// </summary>
        /// <param name="controller">밀어낼 대상</param>
        /// <param name="dir">방향</param>
        /// <param name="distance">거리</param>
        /// <param name="duration">지속 시간</param>
        public void StartPush(CharacterController controller, Vector3 dir, float distance, float duration)
        {
            this.controller = controller;
            this.dir = dir;
            this.distance = distance;
            this.duration = duration;
            this.timer = 0f; // 타이머 초기화
        }

        #endregion
    }

}
