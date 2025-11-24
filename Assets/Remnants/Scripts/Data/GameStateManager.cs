using System.Collections.Generic;
using UnityEngine;

namespace Remnants
{
    //씬별로 오브젝트 활성 상태를 저장
    public class GameStateManager : MonoBehaviour
    {
        #region Variables
        public static GameStateManager Instance;

        // 씬 이름 -> 그 씬의 상태 정보 저장
        public Dictionary<string, SceneData> savedScenes = new Dictionary<string, SceneData>();
        #endregion

        #region Unity Event Method

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject); // 씬 바뀌어도 살아있음
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        #endregion

        #region Custom Method
        private SceneData GetOrCreateSceneData(string sceneName)
        {
            if (!savedScenes.TryGetValue(sceneName, out var data))
            {
                data = new SceneData();
                savedScenes[sceneName] = data;
            }
            return data;
        }

        public void MarkObjectActivated(string sceneName, string objectName)
        {
            var data = GetOrCreateSceneData(sceneName);
            if (!data.activatedObjectNames.Contains(objectName))
                data.activatedObjectNames.Add(objectName);
        }

        public void MarkObjectInteracted(string sceneName, string objectID)
        {
            var data = GetOrCreateSceneData(sceneName);
            if (!data.interactedObjectNames.Contains(objectID))
                data.interactedObjectNames.Add(objectID);
        }
        public void SavePlayerState(string sceneName, Vector3 pos, Quaternion rot)
        {
            var data = GetOrCreateSceneData(sceneName);
            data.playerPosition = pos;
            data.playerRotation = rot;
        }
        public SceneData GetSceneData(string sceneName)
        {
            if (savedScenes.ContainsKey(sceneName))
                return savedScenes[sceneName];
            return null;
        }
        #endregion
    }
}