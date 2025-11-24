using UnityEngine;

namespace Remnants
{
    public class AddMeshColliderToPieces : MonoBehaviour
    {
        //콜라이더 자동 적용
        public GameObject[] mirrorPieces;

        void Start()
        {
            foreach (var piece in mirrorPieces)
            {
                if (piece != null)
                {
                    MeshCollider collider = piece.GetComponent<MeshCollider>();
                    if (collider == null)
                    {
                        collider = piece.AddComponent<MeshCollider>();
                        collider.convex = true;
                    }
                }
            }
        }
    }
}