using UnityEngine;
using System;

namespace Remnants
{
    //파일에 저장할 게임 플레이 데이터 목록
    [Serializable]
    public class PlayData
    {
        public int sceneNumber;

        public PlayData()
        {
            sceneNumber = PlayerDataManager.Instance.SceneNumber;
        }
    }
}
