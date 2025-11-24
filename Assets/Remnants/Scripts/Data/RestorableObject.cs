using UnityEngine;

namespace Remnants
{
    public class RestorableObject : MonoBehaviour
    {
        // 이 오브젝트를 식별할 고유한 ID (에디터에서 수동으로 지정)
        public string objectID;

        private void Start()
        {
            // 씬이 로드될 때마다 이 오브젝트를 SceneObjectRegistry에 등록
            // => 비활성화 상태에서도 복원 시 찾을 수 있도록
            if (SceneObjectRegistry.Instance != null)
            {
                SceneObjectRegistry.Instance.Register(this);
            }
        }
    }
}
