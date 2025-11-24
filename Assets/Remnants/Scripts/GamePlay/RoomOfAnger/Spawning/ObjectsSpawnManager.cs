using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

namespace Remnants
{
    public class ObjectsSpawnManager : MonoBehaviour
    {
        #region Variables

        [Header("스폰 위치 및 프리팹")]
        public List<Transform> spawnPoints;
        public List<GameObject> spheres;
        public GameObject finalShpere;

        [Header("연결")]
        public GameObject actionUI;
        public TextMeshProUGUI actionText;
        public TextMeshProUGUI actionKey;
        public GameObject extraCross;
        public Transform playerTransform;
        public AudioManager audioManager;
        public AudioMixer audioMixer;

        [Header("설정")]
        [SerializeField] private float spawnInterval = 3f;        
        [SerializeField] private float finalDestroyAfter = 5f;

        #endregion

        #region Unity Event Method

        #endregion

        #region Custom Method
        public void StartSpawning()
        {
            InvokeRepeating(nameof(SpawnRandomSphere), 0f, spawnInterval);
        }

        public void StopAndSpawnFinal()
        {
            CancelInvoke(nameof(SpawnRandomSphere));
            SpawnFinalSphere();
        }

        private void SpawnRandomSphere()
        {
            if (spawnPoints.Count == 0 || spheres.Count == 0)
                return;

            // 랜덤 위치 및 랜덤 프리팹 선택
            int pointIndex = Random.Range(0, spawnPoints.Count);
            int sphereIndex = Random.Range(0, spheres.Count);

            GameObject instance = Instantiate(spheres[sphereIndex], spawnPoints[pointIndex].position, Quaternion.identity);

            AssignUIToSphere(instance);
        }

        private void SpawnFinalSphere()
        {
            if (spawnPoints.Count == 0 || spheres.Count == 0)
                return;

            // 마지막 구체는 마지막 위치에서 소환 (가장 끝에 위치한 스폰포인트로 지정 가능)
            Transform spawnPoint = spawnPoints[spawnPoints.Count - 1];
            GameObject instance = Instantiate(finalShpere, spawnPoint.position, Quaternion.identity);

            AssignUIToSphere(instance);

            // 일정 시간 후 파괴
            Destroy(instance, finalDestroyAfter);
        }


        private void AssignUIToSphere(GameObject instance)
        {
            BreakSphere interactive = instance.GetComponent<BreakSphere>();
            DistanceBgm distanceBgm = instance.GetComponent<DistanceBgm>();
            if (interactive != null)
            {
                if (actionUI != null) interactive.actionUI = actionUI;
                if (actionText != null) interactive.actionText = actionText;
                if (actionKey != null) interactive.actionKey = actionKey;
                if (extraCross != null) interactive.extraCross = extraCross;
            }
            if(distanceBgm != null)
            {
                if (playerTransform != null) distanceBgm.player = playerTransform;
                if (audioManager != null) distanceBgm.audioManager = audioManager;
                if (audioMixer != null) distanceBgm.audioMixer = audioMixer;
            }
        }
        #endregion
    }
}
