using UnityEngine;
using UnityEngine.AI;

namespace Sample
{
    public class FollowPet : MonoBehaviour
    {
        #region Variables
        private NavMeshAgent navMeshAgent;
        private GameObject target;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            navMeshAgent = this.GetComponent<NavMeshAgent>();
        }
        private void Update()
        {
            FollowPlayer();
        }
        #endregion

        #region Custom Method
        private void FollowPlayer()
        {
            target = GameObject.Find("Player");

            if (target != null)
                navMeshAgent.SetDestination(target.transform.position);
        }
        #endregion
    }
}

