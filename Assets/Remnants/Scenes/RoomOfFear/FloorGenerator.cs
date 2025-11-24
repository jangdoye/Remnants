using System.Collections.Generic;
using UnityEngine;
using System.Collections;


namespace Remnants
{
    //두려움의 방 복도 생성 낙하 함수
    public class FloorGenerator : MonoBehaviour
    {
        #region Variables
        public GameObject cubePrefab;
        public Transform player;
        private float gameStartTime;

        private int width = 3;          // 가로 큐브 개수 (보통 3)
        private float cubeSizeX = 2f;   // 가로 큐브 간격
        private float cubeSizeZ = 2f;   // 세로 큐브 간격
        [SerializeField]
        private int preGenerateLines = 200;

        [SerializeField]
        private float maxZLimit = 600f;
        [SerializeField]
        private float dropTriggerDistance = 3f;

        private int lastLineGenerated = -1;
        private Dictionary<int, List<GameObject>> floorLines = new Dictionary<int, List<GameObject>>();
        private HashSet<int> alreadyDropped = new HashSet<int>();

        #endregion

        #region Unity Event Method
        private void Start()
        {
            gameStartTime = Time.time;
            for (int i = 0; i < preGenerateLines; i++)
            {
                GenerateLine(i);
            }
        }
        private void Update()
        {
            if (Time.time - gameStartTime < 1.5f)
            {
                return;
            }
            int playerLine = Mathf.FloorToInt(player.position.z / cubeSizeZ);

            // 종점 까지 생성
            if (playerLine + 20 > lastLineGenerated && lastLineGenerated * cubeSizeZ < maxZLimit)
            {
                GenerateLine(lastLineGenerated + 1);
            }

            // 낙하 트리거
            foreach (var kvp in floorLines)
            {
                float cubeZ = kvp.Key * cubeSizeZ;
                float distance = cubeZ - player.position.z;

                if (distance > 25f && distance < dropTriggerDistance && !alreadyDropped.Contains(kvp.Key))
                {
                    DropRandomCubes(kvp.Value);
                    alreadyDropped.Add(kvp.Key);
                }
            }
        }
        #endregion

        #region Custom Method
        private void GenerateLine(int zIndex)
        {
            List<GameObject> line = new List<GameObject>();
            Vector3 basePos = transform.position;

            for (int x = 0; x < width; x++)
            {
                Vector3 pos = basePos + new Vector3((x - 1) * cubeSizeX, 0, zIndex * cubeSizeZ);
                GameObject cube = Instantiate(cubePrefab, pos, Quaternion.identity, transform);
                line.Add(cube);
            }

            floorLines[zIndex] = line;
            lastLineGenerated = zIndex;
        }

        private void DropRandomCubes(List<GameObject> line)
        {
            int dropCount = Random.Range(1, 3);
            List<GameObject> candidates = new List<GameObject>(line);

            for (int i = 0; i < dropCount && candidates.Count > 0; i++)
            {
                int idx = Random.Range(0, candidates.Count);
                GameObject cube = candidates[idx];
                candidates.RemoveAt(idx);

                StartCoroutine(DropAfterDelay(cube, 0.05f));
            }
        }

        IEnumerator DropAfterDelay(GameObject cube, float delay)
        {
            yield return new WaitForSeconds(delay);

            if (cube != null)
            {
                if (!cube.TryGetComponent(out Rigidbody rb))
                    rb = cube.AddComponent<Rigidbody>();

                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.None;
                rb.linearVelocity = Vector3.down * 20f;

                Destroy(cube, 1f);  // 일정 시간 후 제거
            }
        }
        #endregion
    }

}
