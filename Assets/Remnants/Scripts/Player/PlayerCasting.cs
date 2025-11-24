using UnityEngine;

namespace Remnants
{
    public class PlayerCasting : MonoBehaviour
    {
        #region Variables
        public static float distanceFromTarget;     //타겟까지의 거리
        [SerializeField] private float toTarget;    //인스펙터 창 디버깅 용
        #endregion

        #region Unity Event Method
        private void Start()
        {
            distanceFromTarget = Mathf.Infinity;
            toTarget = distanceFromTarget;
        }
        private void Update()
        {
            //레이를 쏘아 거리구하기
            RaycastHit hit;

            if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.forward), out hit))
            {
                distanceFromTarget = hit.distance;
                toTarget = distanceFromTarget;
            }
        }
        private void OnDrawGizmosSelected()
        {
            //레이를 쏘아 거리구하기
            RaycastHit hit;             //레이 hit 정보를 저장하는 변수

            float maxDistance = 100f;   //max distance 지정
            bool isHit = Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.forward), out hit, maxDistance);

            Gizmos.color = Color.red;       //레이를 빨간색으로 지정
            if (isHit)      //isHit 가 true 라면
            {
                Gizmos.DrawRay(this.transform.position, this.transform.forward * hit.distance);     //hit.distance 만큼 레이 그리기
                //Debug.Log("hit distance");
            }
            else    //false라면 
            {
                Gizmos.DrawRay(this.transform.position, this.transform.forward * maxDistance);      //maxDistance 만큼 레이 그리기
                //Debug.Log("max distance");
            }
        }
        #endregion
    }

}
