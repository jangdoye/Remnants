using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

namespace Remnants
{
    //액자 상호작용
    public class TouchPaint : Interactive
    {
        #region Variables
        //액자 오브젝트
        public GameObject realPicture;  //눈 그림이 있는 액자
        public GameObject nextPicture;  //다음 액자

        public ParticleSystem theParticle;
        public Transform CameraRoot;
        public SceneFader fader;
        [SerializeField] private string loadToScene = "Room1";

        private bool hasInteracted = false;  // 상호작용을 했는지 확인
        private string objectID;
        public bool IsInteracted => unInteractive;
        public void SetInteracted(bool interacted)
        {
            unInteractive = interacted;
        }
        #endregion

        #region Unity Event Method
        void Start()
        {
            SceneStateSaver.Instance.SaveCurrentSceneState();

            // ID 가져오기
            var restorable = GetComponent<RestorableObject>();
            if (restorable != null)
                objectID = restorable.objectID;

            // 이전에 상호작용했던 오브젝트라면 비활성화
            string sceneName = SceneManager.GetActiveScene().name;
            var data = GameStateManager.Instance.GetSceneData(sceneName);
            if (data != null && data.interactedObjectNames.Contains(objectID))
            {
                unInteractive = true;  // 더 이상 상호작용 안되게
            }
        }
        protected override void DoAction()
        {
            // 이미 상호작용했거나, 상호작용 불가능한 상태라면 무시
            if (hasInteracted || unInteractive) return;
            hasInteracted = true;
            unInteractive = true;  // 더 이상 상호작용 안되게

            StartCoroutine(TouchingPaint());

        }
        #endregion

        #region Custom Method
        IEnumerator TouchingPaint()
        {
            // 1. 거울 파티클의 방향을 CameraRoot 쪽으로! (실시간 LookAt)
            if (theParticle != null && CameraRoot != null)
            {
                theParticle.transform.LookAt(CameraRoot.position);
                theParticle.gameObject.SetActive(true); // 혹시 비활성화였다면
                theParticle.Play();
            }

            AudioManager.Instance.Play("PaintSound");
            //플레이어 빨려들기 타겟 지정 
            var playerSuck = CameraRoot.GetComponent<PlayerBlackholeSuck>();         
            playerSuck.targetObject = this.transform; 
            playerSuck.StartSuck();

            // 액자 상태 변경
            nextPicture.SetActive(true);
            yield return new WaitForSeconds(1f);
            realPicture.SetActive(true);
            //씬 데이터 저장
            string sceneName = SceneManager.GetActiveScene().name;
            GameStateManager.Instance.MarkObjectActivated(sceneName, realPicture.GetComponent<RestorableObject>().objectID);
            GameStateManager.Instance.MarkObjectActivated(sceneName, nextPicture.GetComponent<RestorableObject>().objectID);
            GameStateManager.Instance.MarkObjectInteracted(sceneName, this.GetComponent<RestorableObject>().objectID);
            SceneStateSaver.Instance.SaveCurrentSceneState();

 
            // 씬 전환
            fader.FadeTo(loadToScene);

        }
        #endregion
    }
}
