using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class ChoiceEvent : BaseScriptEvent
    {
        [LabelText("선택지"), Tooltip("선택지")]
        public ChoiceData[] choices;

        public override void OnEnter()
        {
            base.OnEnter();

            gm.choiceSystem.Setup(choices);
        }
        protected override void OnUpdate() { }
        public override void OnExit()
        {
            gm.choiceSystem.Release();

            base.OnExit();
        }
    }
    [System.Serializable]
    public struct ChoiceData
    {
        [LabelText("선택지 내용"), Tooltip("선택지 내용")]
        public string str;
        [LabelText("선택지 비활성화 여부"), Tooltip("해당 조건에 해당한다면 비활성화")]
        public CompareVariableInTable[] cmps;
        [LabelText("선택 후 이동될 이벤트 번호"), Tooltip("해당 선택지 선택시 이동할 이벤트 번호")]
        public int nextIdx;
    }
    [System.Serializable]
    public class ChoiceSystem
    {
        #region Fields
        [FoldoutGroup("선택지 UI", false)]
        [LabelText("선택지 패널 UI"), Tooltip("선택지 UI 루트 게임오브젝트")]
        public GameObject choiceUI;
        [FoldoutGroup("선택지 UI")]
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true), DisableContextMenu]
        [LabelText("선택지 버튼들"), Tooltip("선택지 버튼들에 대한 RectTransform")]
        public RectTransform[] choiceBtns = new RectTransform[5];

        private TMP_Text[] choiceTMPs;
        private ChoiceData[] choices;
        #endregion

        #region Methods
        public void Initialize()
        {
            choiceTMPs = new TMP_Text[choiceBtns.Length];
            for (int i = 0; i < choiceBtns.Length; ++i)
            {
                int index = i;
                choiceTMPs[i] = choiceBtns[i].GetChild(0).GetComponent<TMP_Text>();
                choiceBtns[i].GetComponent<Button>().onClick.AddListener(() => { GameManager.Instance.choiceSystem.OnChooseChoice(index); });
            }
        }
        public void Setup(ChoiceData[] choices)
        {
            choiceUI.SetActive(true);
            this.choices = choices;
            SetChoice();
        }
        private void SetChoice()
        {
            float padding = (1 - (choices.Length * 0.15f)) / (choices.Length + 1);
            float height = 1.0f;
            for (int i = 0; i < choices.Length; ++i)
            {
                choiceBtns[i].GetComponent<Button>().interactable = !CompareVariableInTable.Compare(choices[i].cmps);

                choiceTMPs[i].SetText(choices[i].str);
                choiceBtns[i].gameObject.SetActive(true);
                height -= padding;
                choiceBtns[i].anchorMax = new Vector2(1.0f, height);
                height -= 0.15f;
                choiceBtns[i].anchorMin = new Vector2(0.0f, height);
            }
        }
        public void OnChooseChoice(int index)
        {
            // 모든 선택지들 비활성화하고
            for (int i = 0; i < choices.Length; ++i)
                choiceBtns[i].gameObject.SetActive(false);

            GameManager.Instance.scriptData.SetScript(choices[index].nextIdx);
        }
        public void Release()
        {
            choiceUI.SetActive(false);
        }
        #endregion
    }
}