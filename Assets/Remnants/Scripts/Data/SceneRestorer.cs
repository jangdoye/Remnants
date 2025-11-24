using UnityEngine;
using UnityEngine.SceneManagement;

namespace Remnants
{
    //씬 로드 시 오브젝트 상태 복원
    public class SceneRestorer : MonoBehaviour
    {
        void Start()
        {
            SceneObjectRegistry.Instance.RegisterAllInScene();

            //Debug.Log("[SceneRestorer] Start() 호출됨");
            if (SceneObjectRegistry.Instance == null)
            {
                Debug.LogError("SceneObjectRegistry.Instance 가 null 입니다!");
                return;
            }

            var allIDs = SceneObjectRegistry.Instance.GetAllObjectIDs();

            if (allIDs.Count == 0)
            {
                //Debug.LogWarning("등록된 오브젝트 ID가 하나도 없습니다!");
            }

            //foreach (var key in allIDs)
            //{
            //    //Debug.Log($"등록된 오브젝트 ID: {key}");
            //}
            string sceneName = SceneManager.GetActiveScene().name;

            SceneData data = GameStateManager.Instance.GetSceneData(sceneName);
            if (data != null)
            {
                //  오브젝트 활성화 복원
                foreach (var name in data.activatedObjectNames)
                {
                    GameObject obj = SceneObjectRegistry.Instance.GetObjectByID(name);
                    if (obj != null)
                    {
                        obj.SetActive(true);
                    }
                }

                // 플레이어 복원
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = data.playerPosition;
                    player.transform.rotation = data.playerRotation;
                }

                //  상호작용했던 오브젝트는 다시 상호작용 못 하게
                foreach (var id in data.interactedObjectNames)
                {
                    GameObject obj = SceneObjectRegistry.Instance.GetObjectByID(id);
                    if (obj != null)
                    {
                        var interactive = obj.GetComponent<Interactive>();
                        if (interactive != null)
                        {
                            interactive.SetInteracted(true); // 비활성화 처리
                        }
                    }
                }
            }

        }
    }
}