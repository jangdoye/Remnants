using UnityEngine;

namespace Remnants
{
    public class TeleportState : MonoBehaviour
    {
        public static TeleportState Instance;
        public bool HasVisitedRoom = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }     
}