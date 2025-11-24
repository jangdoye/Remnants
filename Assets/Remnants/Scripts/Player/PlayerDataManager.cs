using UnityEngine;

namespace Remnants
{
    //플레이어 데이터 관리 클래스 - 싱글톤(다음 씬에서 데이터 보존)
    public class PlayerDataManager : PersistanceSingleton<PlayerDataManager>
    {
        #region Variables
        private int sceneNumber;        //플레이중인 씬 빌드 번호
        #endregion

        #region Property

        //플레이중인 씬 빌드 번호
        public int SceneNumber
        {
            get
            {
                return sceneNumber;
            }
            set
            {
                sceneNumber = value;
            }
        }

        #endregion

        #region Unity Event Method
        protected override void Awake()
        {
            base.Awake();

            //플레이 데이터 초기화 
            InitPlayerData();
        }
        #endregion

        #region Custom Method
        //플레이 데이터 초기화 
        public void InitPlayerData(PlayData pData = null)
        {
            if(pData != null)
            {
                sceneNumber = pData.sceneNumber;


                //....
            }
            else
            {
                sceneNumber = -1;
            }   
        }
        #endregion

    }
}
