using UnityEngine;

namespace Remnants
{
    //벽 움직임 함수
    public class DeathWall : MonoBehaviour
    {
        
        #region Variables
        //public Transform player;         // 플레이어
        [SerializeField]private float moveSpeed = 6f;     // 벽의 전진 속도
        [SerializeField]private float stopZ = 430f;       // 벽이 멈출 위치 (선택)
        private DeathWallTrigger deathWallTrigger;
        private float gameStartTime;
        #endregion

        #region Unity Method
        private void Start()
        {
            gameStartTime = Time.time;
            deathWallTrigger = GetComponentInChildren<DeathWallTrigger>();
        }
        private void Update()
        {
            if (Time.time - gameStartTime < 2f)
            {
                return;
            }
            // z축으로만 전진
            Vector3 pos = transform.position;
            pos.z += moveSpeed * Time.deltaTime;

            // 최대 이동 제한
            if (pos.z > stopZ)
            {
                pos.z = stopZ;
            }

            transform.position = pos;

            WallStop();
        }

        private void WallStop()
        {
            if (deathWallTrigger.IsCatch == true)
            {
                moveSpeed = 0f;
                
            }
        }
        #endregion
        
    }
}