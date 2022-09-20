using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class ChoiceEvent : BaseScriptEvent
    {
        [LabelText("������"), Tooltip("������")]
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
        [LabelText("������ ����"), Tooltip("������ ����")]
        public string str;
        [LabelText("������ ��Ȱ��ȭ ����"), Tooltip("�ش� ���ǿ� �ش��Ѵٸ� ��Ȱ��ȭ")]
        public CompareVariableInTable[] cmps;
        [LabelText("���� �� �̵��� �̺�Ʈ ��ȣ"), Tooltip("�ش� ������ ���ý� �̵��� �̺�Ʈ ��ȣ")]
        public int nextIdx;
    }
    [System.Serializable]
    public class ChoiceSystem
    {
        #region Fields
        [FoldoutGroup("������ UI", false)]
        [LabelText("������ �г� UI"), Tooltip("������ UI ��Ʈ ���ӿ�����Ʈ")]
        public GameObject choiceUI;
        [FoldoutGroup("������ UI")]
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true), DisableContextMenu]
        [LabelText("������ ��ư��"), Tooltip("������ ��ư�鿡 ���� RectTransform")]
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
            // ��� �������� ��Ȱ��ȭ�ϰ�
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