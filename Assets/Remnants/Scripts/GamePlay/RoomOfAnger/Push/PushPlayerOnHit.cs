using UnityEngine;
using UnityEngine.SceneManagement;

namespace Remnants
{
    // 플레이어와 충돌 시 밀어낸 후 자기 자신을 제거하는 컴포넌트
    public class PushPlayerOnHit : MonoBehaviour
    {
        #region Variables
        private SceneFader fader;

        [SerializeField]
        private bool finalSphere = false;

        [Header("밀어내는 설정")]
        [SerializeField] private float pushDistance = 1.5f;   // 플레이어를 밀어낼 거리
        [SerializeField] private float pushDuration = 0.3f;   // 밀림에 걸리는 시간

        #endregion

        #region Unity Event Method

        private void Start()
        {
            fader = FindFirstObjectByType<SceneFader>();
        }

        // 오브젝트가 충돌했을 때 호출되는 메서드
        private void OnCollisionEnter(Collision collision)
        {
            // 충돌한 오브젝트의 레이어가 7번(플레이어)인지 확인
            if (collision.gameObject.layer == 7)
            {
                // CharacterController 컴포넌트를 가져옴 (없으면 리턴)
                CharacterController controller = collision.gameObject.GetComponent<CharacterController>();
                if (controller == null) return;

                if (!finalSphere)
                {
                    // 밀어낼 방향 계산 (수평 방향만)
                    Vector3 dir = (collision.transform.position - transform.position).normalized;
                    dir.y = 0f;

                    // 밀림 처리를 위해 임시 오브젝트 생성 후 PushHandler 컴포넌트 추가
                    GameObject pusher = new GameObject("TempPushHandler");
                    PushHandler handler = pusher.AddComponent<PushHandler>();

                    // 밀림 시작 (플레이어 컨트롤러, 방향, 거리, 시간 전달)
                    handler.StartPush(controller, dir, pushDistance, pushDuration);

                    // 이 오브젝트는 한번만 작동하므로 자기 자신 제거
                    Destroy(gameObject);
                }
                else
                {
                    Scene currentScene = SceneManager.GetActiveScene();
                    fader.FadeTo(currentScene.name);
                }
            }
        }

        #endregion
    }
}
