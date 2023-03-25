using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "ClueData", menuName = ("AddData/ClueData"))]
    public class ClueDataSO : ScriptableObject
    {
        [LabelText("�ܼ� �����͵�"), SerializeField]
        [ListDrawerSettings(HideAddButton = true), DisableContextMenu]
        private List<ClueData> datas;

        private Dictionary<string, ClueData> dataMap = new Dictionary<string, ClueData>();

        private static ClueDataSO instance;

        public static ClueDataSO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = ResourceManager.GetResource<ClueDataSO>(ResourcePath.ClueData);
                    instance.Initialize();
                }
                return instance;
            }
        }
        public ClueData this[string key] => dataMap[key];

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
        [SerializeField, LabelText("�ܼ� �̸�")]
        private string key;
        [BoxGroup("��ȯ", true, true)]
        [SerializeField, LabelText("�ܼ� ������"), OnValueChanged(nameof(ChangeBGObj))]
        private GameObject clueObj;
        [BoxGroup("��ȯ")]
        [Button("�����ͷ� ��ȯ�ϱ�")]
        private void ConvertToData()
        {
            ClueData data = new ClueData();
            data.name = key;
            data.img = clueObj.GetComponent<Image>().sprite;
            data.fds= new List<FingerprintData>(clueObj.transform.childCount);
            for (int i = 0; i < clueObj.transform.childCount; ++i)
                data.fds.Add(new FingerprintData(clueObj.transform.GetChild(i).GetComponent<Image>()));

            foreach (var d in datas)
                if (!Validate())
                    throw new UnityException("�ߺ��� Ű�Դϴ�.");

            datas.Add(data);

            key = "";
            clueObj = null;
        }
        private void ChangeBGObj()
        {
            key = clueObj.name;
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