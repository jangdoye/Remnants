using UnityEngine;
using UnityEngine.AI;

namespace Remnants
{
    public class FollowPlayer : MonoBehaviour
    {
        #region Variables
        //플레이어를 따라가게 하기 위한 NavMeshAgent
        private NavMeshAgent navMeshAgent;
        private GameObject target;

        [SerializeField]
        private bool isStop = false;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            navMeshAgent = this.GetComponent<NavMeshAgent>();

            // Player 태그를 가진 오브젝트를 찾아서 target에 할당
            target = GameObject.FindWithTag("Player");

            if (target == null)
                Debug.LogError("Player 레이어를 가진 오브젝트를 찾을 수 없습니다!");

        }
        private void Update()
        {
            FollowToPlayer();
        }
        #endregion

        #region Custom Method
        //레이어가 Player인 오브젝트를 따라가는 코드
        private void FollowToPlayer()
        {
            //target(플레이어)가 null이 아니라면
            if (target == null)
                return;

            //목표물 따라가기
            if (isStop)
                return;
            navMeshAgent.SetDestination(target.transform.position);
        }
        #endregion
    }
}

