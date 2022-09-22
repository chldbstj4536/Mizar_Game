using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YS
{
    public class SaveLoad
    {
        private const string AesKey = "Yqmgp21ySH+WX6BdQCbuG1Weu0IXmBhM9gQsK7jxQoY=";
        private const string AesIV = "IFw2csnrFl4ZmXe/XihYrA==";

        /// <summary>
        /// 새로운 게임을 시작한다.
        /// </summary>
        /// <param name="saveIndex">새로운 게임을 저장할 저장슬롯</param>
        public static void OnNewGame(int saveIndex)
        {
            // 새로운 게임이므로, saveIndex에 해당하는 세이브데이터를 지우고 새 게임으로 덮어쓴다 (아직 어떻게 새로운 게임에 대한 세이브데이터를 설정할지 못정함)
            OnOverwriteGame(saveIndex, SaveLoadData.NewData);

            // 빈 세이브파일에 새로운 게임을 시작하는것은 로드게임과 같은 기능을 하므로 로드게임시 호출되는 함수와 같은 함수 호출
            OnStartGame(saveIndex);
        }
        /// <summary>
        /// newData를 saveIndex슬롯에 덮어쓴다
        /// </summary>
        /// <param name="saveIndex">저장할 save파일 번호</param>
        /// <param name="newData">새롭게 저장할 데이터</param>
        public static void OnOverwriteGame(int saveIndex, SaveLoadData newData)
        {
            SaveData(saveIndex, newData);
        }

        /// <summary>
        /// 선택된 save파일의 정보를 읽고 해당 정보를 바탕으로 게임을 시작한다
        /// </summary>
        /// <param name="saveIndex">save파일 번호</param>
        public static void OnStartGame(int saveIndex)
        {
            GameObject initDataObj = new GameObject("InGameInitData");
            var initData = initDataObj.AddComponent<InGameInitData>();
            initData.data = LoadData(saveIndex);
            GameObject.DontDestroyOnLoad(initDataObj);

            SceneManager.LoadScene(1);
        }
        private static void SaveData(int index, SaveLoadData saveData)
        {
            string path = Path.SaveData + index.ToString();
            try
            {
                if (!Directory.Exists(Path.AppDataFolder))
                    Directory.CreateDirectory(Path.AppDataFolder);
                if (!File.Exists(path))
                    File.Create(path).Close();

                AES256CBC aes = new AES256CBC(AesKey, AesIV);

                StreamWriter sw = new StreamWriter(path, false);
                sw.WriteLine(aes.Encrypt(JsonUtility.ToJson(saveData)));
                sw.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        public static SaveLoadData LoadData(int index)
        {
            SaveLoadData result;
            string path = Path.SaveData + index.ToString();
            try
            {
                if (!Directory.Exists(Path.AppDataFolder))
                    Directory.CreateDirectory(Path.AppDataFolder);
                if (!File.Exists(path))
                    return SaveLoadData.NewData;

                StreamReader sr = new StreamReader(path);
                var aes = new AES256CBC(AesKey, AesIV);
                string line = aes.Decrypt(sr.ReadLine());
                Debug.Log(line);

                result = JsonUtility.FromJson<SaveLoadData>(line);
                sr.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return SaveLoadData.NewData;
            }

            return result;
        }
        public static bool ExistSaveFile(int index)
        {
            return File.Exists(Path.SaveData + index.ToString());
        }
        public static void WriteSaveData(int index, TMP_Text tmp)
        {
            if (ExistSaveFile(index))
                tmp.text = LoadData(index).saveTime;
        }
    }

    [Serializable]
    public struct SaveLoadData
    {
        // 저장 시간
        public string saveTime;
        // 배경
        public BackgroundData bgData;
        // 배경 페이드 효고 여부
        public bool isFadeOut;
        // 보유 아이템
        public List<ITEM_INDEX> invenItems;
        // 변수 상태
        public List<VariableData> variableDatas;
        // 진행중인 스크립트 번호
        public int scriptIndex;

        public static SaveLoadData NewData => new SaveLoadData(0, new BackgroundData(), false, new List<ITEM_INDEX>(), ResourceManager.GetResource<ScriptData>(ResourcePath.ScriptDataPath).VariableDatas);

        public SaveLoadData(int scriptIndex, BackgroundData bgData, bool isFadeOut, List<ITEM_INDEX> invenItems, List<VariableData> variableDatas)
        {
            saveTime = DateTime.Now.ToString();
            this.bgData = bgData;
            this.isFadeOut = isFadeOut;
            this.invenItems = invenItems;
            this.variableDatas = variableDatas;
            this.scriptIndex = scriptIndex;
        }
    }
}