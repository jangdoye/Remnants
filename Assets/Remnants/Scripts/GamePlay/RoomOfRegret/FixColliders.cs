using UnityEngine;

namespace Remnants
{
    public class FixColliders : MonoBehaviour
    {
        //MeshCollider 와 Rigidbody에 Convex, Kinematic 자동으로 켜지게 설정
        public bool setConvex = true;
        public bool makeKinematic = true;

        void Start()
        {
            foreach (MeshCollider mc in GetComponentsInChildren<MeshCollider>())
            {
                if (setConvex && !mc.convex)
                    mc.convex = true;
            }

            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                if (makeKinematic && !rb.isKinematic)
                    rb.isKinematic = true;
            }
        }
    }
}