using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YS
{
    public class SaveDataManager : Singleton<SaveDataManager>
    {
        private const string AesKey = "Yqmgp21ySH+WX6BdQCbuG1Weu0IXmBhM9gQsK7jxQoY=";
        private const string AesIV = "IFw2csnrFl4ZmXe/XihYrA==";

        private SaveData data;
        public uint UnlockChapter
        {
            get { return data.unlockChapter; }
            set { if (value > data.unlockChapter) data.unlockChapter = value; }
        }

        public SaveDataManager() { LoadData(); }


        /// <summary>
        /// 처음부터 게임을 시작한다
        /// </summary>
        public void StartGame()
        {
            StartGame(InGameSaveData.NewSaveData);
        }
        /// <summary>
        /// 선택된 save파일의 정보를 읽고 해당 정보를 바탕으로 게임을 시작한다
        /// </summary>
        /// <param name="saveIndex">save파일 번호</param>
        public void StartGameWithSave(int saveIndex)
        {
            StartGame(data[saveIndex]);
        }
        private void StartGame(InGameSaveData data)
        {
            GameObject initDataObj = new GameObject("InGameInitData");
            var initData = initDataObj.AddComponent<InGameInitData>();
            initData.data = data;
            GameObject.DontDestroyOnLoad(initDataObj);

            GameObject loadDataObj = new GameObject("LoadData");
            var loadData = loadDataObj.AddComponent<LoadingData>();
            loadData.loadingScene = LOADING_SCENE.GAME;
            GameObject.DontDestroyOnLoad(loadDataObj);

            SceneManager.LoadScene(0);
        }
        /// <summary>
        /// index 세이브 슬롯에 saveData를 저장합니다.
        /// </summary>
        /// <param name="index">세이브 슬롯</param>
        /// <param name="saveData">현재 게임의 데이터</param>
        public void SaveInGameData(int index, InGameSaveData saveData)
        {
            data[index] = saveData;
            SaveData();
        }
        private void SaveData()
        {
            try
            {
                if (!Directory.Exists(Path.AppDataFolder))
                    Directory.CreateDirectory(Path.AppDataFolder);
                if (!File.Exists(Path.SaveData))
                {
                    File.Create(Path.SaveData).Close();
                    data.unlockChapter = 1;
                }

                AES256CBC aes = new AES256CBC(AesKey, AesIV);

                StreamWriter sw = new StreamWriter(Path.SaveData, false);
                string line = JsonUtility.ToJson(data);
                Debug.Log(line);
                sw.WriteLine(aes.Encrypt(line));
                sw.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        public void LoadData()
        {
            try
            {
                if (!Directory.Exists(Path.AppDataFolder))
                    Directory.CreateDirectory(Path.AppDataFolder);
                if (!File.Exists(Path.SaveData))
                    SaveData();

                StreamReader sr = new StreamReader(Path.SaveData);
                var aes = new AES256CBC(AesKey, AesIV);
                string line = aes.Decrypt(sr.ReadLine());
                Debug.Log(line);

                data = JsonUtility.FromJson<SaveData>(line);
                sr.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        public InGameSaveData GetInGameSaveData(int index)
        {
            return data[index];
        }
    }

    /// <summary>
    /// 세이브되는 정보들에 대한 구조체
    /// </summary>
    [System.Serializable]
    public struct InGameSaveData
    {
        // 진행중인 스크립트 번호
        public int scriptIndex;
        // 배경
        public BackgroundData bgData;
        // 배경 페이드 효고 여부
        public bool isFadeOut;
        // 보유 아이템
        public List<ITEM_INDEX> invenItems;
        // 배경 음악
        public BGMData bgmData;
        // 변수 상태
        public List<VariableData> variableDatas;
        // 저장 시간
        public string saveTime;

        public static InGameSaveData NewSaveData => new InGameSaveData
        {
            scriptIndex = 0,
            bgData = new BackgroundData(),
            isFadeOut = false,
            invenItems = new List<ITEM_INDEX>(),
            bgmData = new BGMData(),
            variableDatas = ResourceManager.GetResource<ScriptData>(ResourcePath.ScriptData).VariableDatas,
            saveTime = "No Data"
        };

        public InGameSaveData(int scriptIndex, BackgroundData bgData, bool isFadeOut, List<ITEM_INDEX> invenItems, BGMData bgmData, List<VariableData> variableDatas)
        {
            this.scriptIndex = scriptIndex;
            this.bgData = bgData;
            this.isFadeOut = isFadeOut;
            this.invenItems = invenItems;
            this.bgmData = bgmData;
            this.variableDatas = variableDatas;
            saveTime = DateTime.Now.ToString();
        }

    }
    [Serializable]
    public struct SaveData
    {
        public uint unlockChapter;
        public List<InGameSaveData> inGameSaveDatas;

        public InGameSaveData this[int index]
        {
            get { CheckExistInGameSaveData(index); return inGameSaveDatas[index]; }
            set { CheckExistInGameSaveData(index); inGameSaveDatas[index] = value; }
        }
        private void CheckExistInGameSaveData(int index)
        {
            inGameSaveDatas ??= new List<InGameSaveData>();
            while (inGameSaveDatas.Count <= index)
                inGameSaveDatas.Add(InGameSaveData.NewSaveData);
        }
    }
}