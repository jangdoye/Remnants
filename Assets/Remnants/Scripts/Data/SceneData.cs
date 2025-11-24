using UnityEngine;
using System.Collections.Generic;

namespace Remnants
{
    [System.Serializable]
    public class SceneData
    {
        // 활성화된 오브젝트 ID 목록
        public List<string> activatedObjectNames = new List<string>();
        public List<string> interactedObjectNames = new List<string>();
        // 플레이어 위치·회전
        public Vector3 playerPosition;
        public Quaternion playerRotation;
    }
}