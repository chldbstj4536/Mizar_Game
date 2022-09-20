using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    public class InferenceEvent : BaseScriptEvent
    {
        #region Fields
        [SerializeField]
        [LabelText("�߸��� ������ ĳ����")]
        private Sprite charImg;
        [SerializeField]
        [LabelText("�߸��� ������")]
        private ITEM_INDEX itemIndex;
        [SerializeField]
        [LabelText("��������")]
        private InferenceDialogData[] choiceDatas = new InferenceDialogData[1];
        [FoldoutGroup("Ʋ�ȴ� ������ ���ý� ������ ����", false), SerializeField]
        [HideLabel]
        private DialogEvent twiceFailDialogData;
        [FoldoutGroup("���ھ ���� ���� ����", false), SerializeField]
        [FoldoutGroup("���ھ ���� ���� ����/�ѹ��� ����"), HideLabel]
        private ChangeVariableInTable[] perfectChange = new ChangeVariableInTable[0];
        [FoldoutGroup("���ھ ���� ���� ����"), SerializeField]
        [FoldoutGroup("���ھ ���� ���� ����/�ι����� ����"), HideLabel]
        private ChangeVariableInTable[] greatChange = new ChangeVariableInTable[0];
        [FoldoutGroup("���ھ ���� ���� ����"), SerializeField]
        [FoldoutGroup("���ھ ���� ���� ����/����"), HideLabel]
        private ChangeVariableInTable[] failChange = new ChangeVariableInTable[0];
        [SerializeField, MaxValue("@choiceDatas.Length - 1")]
        [LabelText("����"), Tooltip("�߸� ���������� �ùٸ� ��")]
        private uint correctIndex;
        #endregion

        #region Properties
        public Sprite CharacterImage => charImg;
        public ITEM_INDEX ItemIndex => itemIndex;
        public InferenceDialogData[] ChoiceDatas => choiceDatas;
        public DialogEvent TwiceFailDialogData => twiceFailDialogData;
        public ChangeVariableInTable[] PerfectChange => perfectChange;
        public ChangeVariableInTable[] GreatChange => greatChange;
        public ChangeVariableInTable[] FailChange => failChange;
        public uint CorrectIndex => correctIndex;
        #endregion

        public override void OnEnter()
        {
            base.OnEnter();

            gm.ifSystem.Setup(this);
        }
        protected override void OnUpdate()
        {
            gm.ifSystem.OnUpdate();
        }
        public override void OnExit()
        {
            gm.ifSystem.Release();

            base.OnExit();
        }
    }
    [System.Serializable]
    public struct InferenceDialogData
    {
        [LabelText("������ ����")]
        public string choiceStr;
        [LabelText("���� �� ��ȭ����")]
        public DialogEvent[] dialogs;
    }
    [System.Serializable]
    public class InferenceSystem
    {
        #region Fields
        [FoldoutGroup("�߸� UI", false)]
        [LabelText("�߸� �г� UI"), Tooltip("���� �г� ��Ʈ ���ӿ�����Ʈ")]
        public GameObject rootObj;
        [FoldoutGroup("�߸� UI")]
        [LabelText("�߸� ĳ���� �̹���"), Tooltip("�߸��ϴ� ĳ���� �̹��� ������Ʈ")]
        public Image character;
        [FoldoutGroup("�߸� UI")]
        [LabelText("�߸� ������ �̹���"), Tooltip("�߸��ϴ� ������ �̹��� ������Ʈ")]
        public Image item;
        [FoldoutGroup("�߸� UI")]
        [LabelText("������ ���� TMP"), Tooltip("�߸� ������ ���� TMP ������Ʈ")]
        public TMP_Text tmp_itemDesc;
        [FoldoutGroup("�߸� UI")]
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true), DisableContextMenu]
        [LabelText("�߸� ��������"), Tooltip("�߸��� ���� �������� ��ư ������Ʈ��")]
        public Button[] choiceBtns = new Button[3];

        private TMP_Text[] choiceTMPs;
        private InferenceDialogData[] choiceDatas;
        private DialogEvent twiceFailDialogData;
        private ChangeVariableInTable[] perfectChange;
        private ChangeVariableInTable[] greatChange;
        private ChangeVariableInTable[] failChange;
        private uint correctIndex;
        private int lastChoiceIndex;
        private int curDialogIndex;
        private bool bExit;
        private GameManager gm;
        #endregion

        #region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;
            choiceTMPs = new TMP_Text[choiceBtns.Length];
            for (uint i = 0; i < choiceBtns.Length; ++i)
            {
                uint index = i;
                choiceTMPs[i] = choiceBtns[i].transform.GetChild(0).GetComponent<TMP_Text>();
                choiceBtns[i].onClick.AddListener(() => { GameManager.Instance.ifSystem.Choice(index); });
            }
        }
        public void Setup(InferenceEvent ie)
        {
            rootObj.SetActive(true);
            character.sprite = ie.CharacterImage;
            character.SetNativeSize();
            float width = (character.rectTransform.parent as RectTransform).rect.width;
            float ratio = width / character.rectTransform.rect.width;
            float height = character.rectTransform.rect.height * ratio;

            character.rectTransform.sizeDelta = new Vector2(width, height);

            item.sprite = gm.itemData[ie.ItemIndex].img;
            tmp_itemDesc.text = gm.itemData[ie.ItemIndex].desc;

            choiceDatas = ie.ChoiceDatas;
            for (int i = 0; i < choiceDatas.Length; ++i)
            {
                choiceTMPs[i].text = choiceDatas[i].choiceStr;
                choiceBtns[i].gameObject.SetActive(true);
            }
            twiceFailDialogData = ie.TwiceFailDialogData;

            perfectChange = ie.PerfectChange;
            greatChange = ie.GreatChange;
            failChange = ie.FailChange;

            correctIndex = ie.CorrectIndex;
            lastChoiceIndex = -1;
            curDialogIndex = 0;
            bExit = false;
        }
        public void OnUpdate()
        {
            if (!rootObj.activeInHierarchy && gm.IsKeyDown())
                OnDialogEvent();
        }
        public void Release()
        {
            rootObj.SetActive(false);
            for (int i = 0; i < choiceDatas.Length; ++i)
                choiceBtns[i].gameObject.SetActive(false);
        }
        private void Choice(uint choiceIndex)
        {
            curDialogIndex = 0;
            rootObj.SetActive(false);

            // ����
            if (choiceIndex == correctIndex)
            {
                bExit = true;

                // �ѹ��� ����
                if (lastChoiceIndex == -1)
                    foreach (var changeVar in perfectChange)
                        changeVar.Calculate();
                // �ι����� ����
                else
                    foreach (var changeVar in greatChange)
                        changeVar.Calculate();
            }
            // ����
            else if (lastChoiceIndex != -1)
            {
                bExit = true;

                foreach (var changeVar in failChange)
                    changeVar.Calculate();

                // ���� �������� ����
                if (lastChoiceIndex == choiceIndex)
                {
                    gm.dialogSystem.Setup(twiceFailDialogData);
                    return;
                }
            }

            lastChoiceIndex = (int)choiceIndex;

            gm.dialogSystem.Setup(choiceDatas[lastChoiceIndex].dialogs[curDialogIndex]);
        }
        /// <summary>
        /// ���̾�α� �̺�Ʈ �߻��� ȣ��Ǵ� �Լ�
        /// </summary>
        public void OnDialogEvent()
        {
            // ���콺 Ŭ���� Ÿ������ �ȳ����ٸ� Ÿ���� ������, Ÿ������ �� �Ǿ��ִ� ���¶�� ���� ���̾�α� ����
            if (!gm.dialogSystem.scriptTMP.IsDoneTyping)
                gm.dialogSystem.scriptTMP.SkipTyping();
            else if (++curDialogIndex == choiceDatas[lastChoiceIndex].dialogs.Length)
            {
                gm.dialogSystem.Release();

                if (bExit) gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
                else rootObj.SetActive(true);
            }
            else
                gm.dialogSystem.Setup(choiceDatas[lastChoiceIndex].dialogs[curDialogIndex]);
        }
        #endregion
    }
}