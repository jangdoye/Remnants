using UnityEngine;
using UnityEngine.SceneManagement;

namespace Remnants
{
    public class SceneStateSaver : MonoBehaviour
    {
        #region Variables
        //싱글톤 인스턴스
        public static SceneStateSaver Instance;
        //참조
        private GameObject player;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);      // 씬이 바뀌어도 살아있게 함
            }
            else
            {
                Destroy(this.gameObject);                // 중복 방지
            }
        }
        #endregion

        #region Custom Method
        //현재 씬 저장하는 함수
        public void SaveCurrentSceneState()
        {
            string sceneName = SceneManager.GetActiveScene().name;

            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                GameStateManager.Instance.SavePlayerState(sceneName, player.transform.position, player.transform.rotation);
            }
            
        }
        #endregion
    }
}