using UnityEngine;

namespace Sample
{
    public class Interactive : MonoBehaviour
    {
        #region Variables
        //theDistance를 protected 로 중복 가능하게
        protected float theDistance;

        public GameObject extraCross;       //커서 올렸을 때 그 오브젝트에 콜라이더가 붙어있다면
        #endregion

        #region Unity Event Method
        private void Update()       //Update로 구하는 이유는 실시간으로 구해야 하기 때문
        {
            //오브젝트와 플레이어 사이간 거리 구하기
            theDistance = PlayerCasting.distanceFromTarget;
        }
        private void OnMouseOver()
        {
            extraCross.SetActive(true);

            if (theDistance <= 2f)
            {
                ShowActionUI();

            }
        }
        private void OnMouseExit()
        {
            extraCross.SetActive(false);
        }
        #endregion

        #region Custom Method
        private void ShowActionUI()
        {

        }
        #endregion
    }

}
