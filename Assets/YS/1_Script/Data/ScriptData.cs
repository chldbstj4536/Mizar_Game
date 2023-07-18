using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "ScriptData", menuName = ("AddData/ScriptData"))]
    public class ScriptData : ScriptableObject
    {
        [SerializeReference]
        [ListDrawerSettings(ShowIndexLabels = true, NumberOfItemsPerPage = 20), Searchable]
        private List<BaseScriptEvent> scripts;
        private BaseScriptEvent curScript;
        private int curIndex;
        [LabelText("Ŀ���� ������"), SerializeField]
        [ListDrawerSettings(HideAddButton = true), DisableContextMenu]
        private List<VariableData> varDatas;

        public BaseScriptEvent CurrentScript => curScript;
        public int CurrentIndex => curIndex;
        public List<VariableData> VariableDatas => varDatas;

        public void SetScript(int index)
        {
            curScript?.OnExit();
            curIndex = index;
            if (curIndex >= scripts.Count)
            {
                GameObject loadDataObj = new GameObject("LoadData");
                var loadData = loadDataObj.AddComponent<LoadingData>();
                loadData.loadingScene = LOADING_SCENE.TITLE;
                GameObject.DontDestroyOnLoad(loadDataObj);

                SceneManager.LoadScene(0);
                return;
            }

            curScript = scripts[curIndex = index];
            curScript.OnEnter();
        }
        public void Clear()
        {
            curScript = null;
        }

#if UNITY_EDITOR
        [HorizontalGroup("���� ����", LabelWidth = 75)]
        [BoxGroup("���� ����/���� �߰�"), SerializeField, ValidateInput(nameof(ValidateHasNotKey), "�����ϴ� �������Դϴ�.")]
        [LabelText("������")]
        private string addKey;
        [BoxGroup("���� ����/���� �߰�"), SerializeReference]
        [LabelText("���� ������")]
        private ADDABLE_TYPE addValue;
        [BoxGroup("���� ����/���� �߰�"), SerializeField]
        [Button(Name = "�߰�")]
        private void AddVarInTable()
        {
            if (ValidateHasKey(addKey))
                throw new UnityException("�����ϴ� �������Դϴ�.");

            VariableData data = new VariableData { name = addKey, value = null };

            switch (addValue)
            {
                case ADDABLE_TYPE.BOOL:
                    data.value = new BoolVariable(false);
                    break;
                case ADDABLE_TYPE.INT:
                    data.value = new IntVariable(0);
                    break;
                case ADDABLE_TYPE.FLOAT:
                    data.value = new FloatVariable(0.0f);
                    break;
            }

            varDatas.Add(data);
            addKey = "";
        }
        [HorizontalGroup("���� ����")]
        [BoxGroup("���� ����/���� ����"), SerializeField, ValidateInput(nameof(ValidateHasKey), "�������� �ʴ� �������Դϴ�.")]
        [LabelText("������")]
        private string removeKey;
        [BoxGroup("���� ����/���� ����"), SerializeField]
        [Button(Name = "����")]
        private void RemoveVarInTable()
        {
            if (ValidateHasNotKey(removeKey))
                throw new UnityException("�������� �ʴ� �������Դϴ�.");

            foreach (var data in varDatas)
            {
                if (data.name == removeKey)
                {
                    varDatas.Remove(data);
                    break;
                }
            }

            removeKey = "";
        }
        private bool ValidateHasNotKey(string newKey)
        {
            HashSet<string> set = new HashSet<string>();

            foreach (var data in varDatas)
                set.Add(data.name);

            return set.Add(newKey);
        }
        private bool ValidateHasKey(string newKey)
        {
            return !ValidateHasNotKey(newKey);
        }
        [BoxGroup("��ũ��Ʈ ����", true, true)]
        [SerializeField, Min(0), MaxValue("@scripts.Count - 1")]
        [LabelText("������ �ε��� ��ġ")]
        private int insertIndex;
        [BoxGroup("��ũ��Ʈ ����")]
        [Button(Name = "��ũ��Ʈ ����")]
        private void InsertScript()
        {
            scripts.Insert(insertIndex, null);
        }

        [BoxGroup("��ũ��Ʈ ��ġ �ٲٱ�", true, true)]
        [SerializeField, Min(0), MaxValue("@scripts.Count - 1")]
        [LabelText("�ٲ� �ε��� ��ġ")]
        private Vector2Int swapIndex;
        [BoxGroup("��ũ��Ʈ ��ġ �ٲٱ�")]
        [Button(Name = "��ũ��Ʈ ��ġ �ٲٱ�")]
        private void SwapScript()
        {
            (scripts[swapIndex.x], scripts[swapIndex.y]) = (scripts[swapIndex.y], scripts[swapIndex.x]);
        }
#endif
    }
}