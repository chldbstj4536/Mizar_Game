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
        [LabelText("커스텀 변수들"), SerializeField]
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
        [HorizontalGroup("변수 조작", LabelWidth = 75)]
        [BoxGroup("변수 조작/변수 추가"), SerializeField, ValidateInput(nameof(ValidateHasNotKey), "존재하는 변수명입니다.")]
        [LabelText("변수명")]
        private string addKey;
        [BoxGroup("변수 조작/변수 추가"), SerializeReference]
        [LabelText("변수 데이터")]
        private ADDABLE_TYPE addValue;
        [BoxGroup("변수 조작/변수 추가"), SerializeField]
        [Button(Name = "추가")]
        private void AddVarInTable()
        {
            if (ValidateHasKey(addKey))
                throw new UnityException("존재하는 변수명입니다.");

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
        [HorizontalGroup("변수 조작")]
        [BoxGroup("변수 조작/변수 삭제"), SerializeField, ValidateInput(nameof(ValidateHasKey), "존재하지 않는 변수명입니다.")]
        [LabelText("변수명")]
        private string removeKey;
        [BoxGroup("변수 조작/변수 삭제"), SerializeField]
        [Button(Name = "삭제")]
        private void RemoveVarInTable()
        {
            if (ValidateHasNotKey(removeKey))
                throw new UnityException("존재하지 않는 변수명입니다.");

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
        [BoxGroup("스크립트 삽입", true, true)]
        [SerializeField, Min(0), MaxValue("@scripts.Count - 1")]
        [LabelText("삽입할 인덱스 위치")]
        private int insertIndex;
        [BoxGroup("스크립트 삽입")]
        [Button(Name = "스크립트 삽입")]
        private void InsertScript()
        {
            scripts.Insert(insertIndex, null);
        }

        [BoxGroup("스크립트 위치 바꾸기", true, true)]
        [SerializeField, Min(0), MaxValue("@scripts.Count - 1")]
        [LabelText("바꿀 인덱스 위치")]
        private Vector2Int swapIndex;
        [BoxGroup("스크립트 위치 바꾸기")]
        [Button(Name = "스크립트 위치 바꾸기")]
        private void SwapScript()
        {
            (scripts[swapIndex.x], scripts[swapIndex.y]) = (scripts[swapIndex.y], scripts[swapIndex.x]);
        }
#endif
    }
}