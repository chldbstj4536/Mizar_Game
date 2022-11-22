using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "BackgroundData", menuName = ("AddData/BackgroundData"))]
    public class BackgroundDataSO : ScriptableObject
    {
        [LabelText("��� �����͵�"), SerializeField]
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
        [BoxGroup("��ȯ", true, true)]
        [ValidateInput("Validate", "�ߺ��� �̸��� �ֽ��ϴ�", ContinuousValidationCheck = true)]
        [SerializeField, LabelText("��� �̸�")]
        private string key;
        [BoxGroup("��ȯ", true, true)]
        [SerializeField, LabelText("��� ������"), OnValueChanged(nameof(ChangeBGObj))]
        private GameObject bgObj;
        [BoxGroup("��ȯ")]
        [Button("�����ͷ� ��ȯ�ϱ�")]
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
                    throw new UnityException("�ߺ��� Ű�Դϴ�.");

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