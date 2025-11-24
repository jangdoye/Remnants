using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Remnants
{
    public class SceneObjectRegistry : MonoBehaviour
    {
        #region Variables
        // 싱글톤 인스턴스
        public static SceneObjectRegistry Instance;
        // 오브젝트 ID 매핑
        private Dictionary<string, GameObject> objects = new Dictionary<string, GameObject>();
        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
                SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // 씬 전환 시 기존 레지스트리 초기화
            objects.Clear();
            RegisterAllInScene();
        }
        #endregion

        #region Custom Methods
        // RestorableObject 등록
        public void Register(RestorableObject obj)
        {
            if (!string.IsNullOrEmpty(obj.objectID) && !objects.ContainsKey(obj.objectID))
            {
                objects.Add(obj.objectID, obj.gameObject);
                //Debug.Log($"[SceneObjectRegistry] Registered object: {obj.objectID}");
            }
            else
            {
                //Debug.LogWarning($"[SceneObjectRegistry] Duplicate or invalid objectID: {obj.objectID}");
            }
        }

        // ID로 게임오브젝트 반환
        public GameObject GetObjectByID(string id)
        {
            if (objects.TryGetValue(id, out var obj))
                return obj;
            return null;
        }

        // 모든 등록된 ID 리스트 반환
        public List<string> GetAllObjectIDs()
        {
            return new List<string>(objects.Keys);
        }

        // 씬 내 모든 RestorableObject 찾아 등록
        public void RegisterAllInScene()
        {
            var scene = SceneManager.GetActiveScene();
            var roots = scene.GetRootGameObjects();

            foreach (var root in roots)
            {
                // 비활성 포함하여 자식 오브젝트까지 모두 탐색
                var comps = root.GetComponentsInChildren<RestorableObject>(true);
                foreach (var r in comps)
                {
                    Register(r);
                }
            }
        }
        #endregion
    }
}
