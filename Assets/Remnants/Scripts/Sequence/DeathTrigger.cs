using UnityEngine;

namespace Remnants
{
    public class DeathTrigger : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField]
        private string NowScene = "RoomOfAnger";
        #endregion
        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                fader.FadeTo(NowScene);
            }   
        }
        #endregion
    }

}
