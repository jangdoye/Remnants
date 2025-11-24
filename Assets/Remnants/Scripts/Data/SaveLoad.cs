using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;   //이진화

namespace Remnants
{
    //게임 데이터를 파일에 저장/가져오기 기능 구현 - 이진화 포맷 적용
    public class SaveLoad
    {
        //저장하기
        public static void SaveData()
        {
            //파일 이름, 경로 지정
            string path = Application.persistentDataPath + "/pData.arr";

            //저장할 데이터를 이진화 준비
            BinaryFormatter formatter = new BinaryFormatter();

            //파일 접근 - FileMode.Create : 존재하면 파일 가져오기 없으면 새로 만들라
            FileStream fs = new FileStream(path, FileMode.Create);

            //저장할 데이터를 준비 : 생성자를 통해 세이브 데이터 셋팅
            PlayData playData = new PlayData();
            //Debug.Log($"Save SceneNumber: {playData.sceneNumber}");

            //준비한 데이터를 이진화 저장
            formatter.Serialize(fs, playData);

            //파일에 접근하면 항상 파일을 닫아주어야 한다
            fs.Close();
        }

        //가져오기 - 저장된 데이터를 반환값으로 받아온다
        public static PlayData LoadData()
        {
            PlayData playData = null;
            string path = Application.persistentDataPath + "/pData.arr";

            if (File.Exists(path))
            {
                FileInfo fileInfo = new FileInfo(path);

                //파일이 비어 있으면 역직렬화 시도 금지
                if (fileInfo.Length == 0)
                {
                    Debug.LogWarning("저장 파일이 비어 있어 역직렬화할 수 없습니다.");
                    return null;
                }

                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        playData = formatter.Deserialize(fs) as PlayData;
                        Debug.Log($"로드 성공: 씬 번호 {playData.sceneNumber}");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("역직렬화 중 오류 발생: " + e.Message);
                }
            }
            else
            {
                Debug.Log("저장 파일이 없습니다.");
            }

            return playData;
        }
    }
}
