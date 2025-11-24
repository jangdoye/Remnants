using UnityEngine;

namespace Remnants
{
    public class DestoryTrigger : MonoBehaviour
    {
        #region Variables

        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            
            if (other.tag == "Sphere")
            {                
                Destroy(other.gameObject);
            }
        }
        #endregion

        #region Custom Method

        #endregion
    }

}
