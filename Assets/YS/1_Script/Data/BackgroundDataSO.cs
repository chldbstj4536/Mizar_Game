using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "BackgroundData", menuName = ("AddData/BackgroundData"))]
    public class BackgroundDataSO : ScriptableObject
    {
        [LabelText("배경 데이터들"), SerializeField]
        [ListDrawerSettings(HideAddButton = true), DisableContextMenu]
        private List<BackgroundData> datas;

        private Dictionary<string, BackgroundData> dataMap = new Dictionary<string, BackgroundData>();

        private static BackgroundDataSO instance;

        public static BackgroundDataSO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = ResourceManager.GetResource<BackgroundDataSO>(ResourcePath.BackgroundData);
                    instance.Initialize();
                }
                return instance;
            }
        }
        public BackgroundData this[string key] => dataMap[key];

        private static string[] Names
        {
            get
            {
                string[] names = new string[Instance.datas.Count];
                for (int i = 0; i < Instance.datas.Count; ++i)
                    names[i] = Instance.datas[i].name;
                return names;
            }
        }

        private void Initialize()
        {
            foreach (var data in datas)
                dataMap.Add(data.name, data);
        }

#if UNITY_EDITOR
        [BoxGroup("변환", true, true)]
        [ValidateInput("Validate", "중복된 이름이 있습니다", ContinuousValidationCheck = true)]
        [SerializeField, LabelText("배경 이름")]
        private string key;
        [BoxGroup("변환", true, true)]
        [SerializeField, LabelText("배경 프리팹"), OnValueChanged(nameof(ChangeBGObj))]
        private GameObject bgObj;
        [BoxGroup("변환")]
        [Button("데이터로 변환하기")]
        private void ConvertToData()
        {
            BackgroundData data = new BackgroundData();
            data.name = key;
            data.img = bgObj.GetComponent<Image>().sprite;
            data.items = new List<BackgroundItemData>(bgObj.transform.childCount);
            for (int i = 0; i < bgObj.transform.childCount; ++i)
                data.items.Add(new BackgroundItemData(bgObj.transform.GetChild(i).GetComponent<Item>()));

            foreach (var d in datas)
                if (!Validate())
                    throw new UnityException("중복된 키입니다.");

            datas.Add(data);

            key = "";
            bgObj = null;
        }
        private void ChangeBGObj()
        {
            key = bgObj.name;
        }
        private bool Validate()
        {
            try
            {
                foreach (var data in datas)
                    if (key == data.name)
                        return false;
            }
            catch
            {
                return false;
            }
            return true;
        }
#endif
    }
}