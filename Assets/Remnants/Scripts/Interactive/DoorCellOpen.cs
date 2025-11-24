using UnityEngine;

namespace Remnants
{
    public class DoorCellOpen : Interactive
    {
        #region Variables
        public Animator animator;
        #endregion

        #region Unity Event Method

        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            AudioManager.Instance.Play("CreakyDoorOpen");
            animator.SetBool("IsOpen", true);   //애니메이터 작동

            this.GetComponent<BoxCollider>().enabled = false;   //애니메이터 작동하면 Collider 삭제
        }
        #endregion
    }

}
