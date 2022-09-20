using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    public class ArrangeEvent : BaseScriptEvent
    {
        #region Fields
        [SerializeField]
        [LabelText("���� ����")]
        private string question;
        [SerializeField]
        [LabelText("�ܾ�"), ListDrawerSettings(HideAddButton = true, HideRemoveButton = true), DisableContextMenu]
        private Word[] words = new Word[4];
        [SerializeField, TextArea]
        [LabelText("��Ʈ")]
        private string hintStr;
        [SerializeField, TextArea]
        [LabelText("���� �� ����")]
        private string successStr;
        [SerializeField]
        [LabelText("���� �� ���� ��ȭ")]
        private ChangeVariableInTable[] successChanges = new ChangeVariableInTable[0];
        [SerializeField, TextArea]
        [LabelText("���� �� ����")]
        private string failStr;
        [SerializeField]
        [LabelText("���� �� ���� ��ȭ")]
        private ChangeVariableInTable[] failChanges = new ChangeVariableInTable[0];
        #endregion

        #region Properties
        public string Question => question;
        public Word[] Words => words;
        public string HintStr => hintStr;
        public string SuccessStr => successStr;
        public ChangeVariableInTable[] SuccessChanges => successChanges;
        public string FailStr => failStr;
        public ChangeVariableInTable[] FailChanges => failChanges;
        #endregion

        public override void OnEnter()
        {
            base.OnEnter();

            gm.arSystem.Setup(this);
        }
        protected override void OnUpdate()
        {
            gm.arSystem.OnUpdate();
        }
        public override void OnExit()
        {
            gm.arSystem.Release();

            base.OnExit();
        }

        [System.Serializable, DisableContextMenu]
        public struct Word
        {
            [LabelText("�����ܾ��ΰ�?")]
            public bool isFixedWord;
            [ShowIf("isFixedWord")]
            [LabelText("�����ܾ� ���ڿ�")]
            public string fixedWord;
            [HideIf("isFixedWord")]
            [LabelText("���ôܾ� ���ڿ���")]
            public string[] choiceWords;
            [SerializeField, HideIf("isFixedWord"), MinValue("@choiceWords.Length == 0 ? 0 : 1"), MaxValue("@choiceWords.Length")]
            [LabelText("����"), Tooltip("�߸� ���������� �ùٸ� ��")]
            public int correctIndex;
        }
    }
    [System.Serializable]
    public class ArrangeSystem
    {
        #region Fields
        [FoldoutGroup("���� UI", false)]
        [LabelText("���� �г� UI"), Tooltip("���� �г� ��Ʈ ���ӿ�����Ʈ")]
        public GameObject rootObj;
        [FoldoutGroup("���� UI")]
        [LabelText("���� TMP"), Tooltip("������ ���� ������ ������ TMP")]
        public TMP_Text questionTMP;
        [FoldoutGroup("���� UI")]
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true), DisableContextMenu]
        [LabelText("�ܾ� ������Ʈ"), Tooltip("���� �г��� �ܾ���� ������Ʈ")]
        public WordComponent[] words = new WordComponent[4];
        [FoldoutGroup("���� UI")]
        [LabelText("���ڸ� �̹��� ������Ʈ"), Tooltip("���ڸ��� �̹��� ������Ʈ")]
        public Image mizarImg;
        [FoldoutGroup("���� UI")]
        [LabelText("���̾�α� ��ȭ���� TMP"), Tooltip("���̾�α׿��� ��ȭ ������ ��Ÿ���� TMP")]
        public TMP_Text descTMP;
        [FoldoutGroup("���� UI")]
        [LabelText("���� ��ư"), Tooltip("���� �ϼ� �̺�Ʈ�� �߻���Ű�� ��ư")]
        public Button submitBtn;

        private ArrangeEvent.Word[] wordsData;
        private string successStr, failStr;
        private ChangeVariableInTable[] successChanges, failChanges;
        private bool isSubmit;
        private GameManager gm;
        #endregion

        #region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;

            submitBtn.onClick.AddListener(Submit);
        }
        public void Setup(ArrangeEvent ae)
        {
            rootObj.SetActive(true);

            questionTMP.text = ae.Question;
            descTMP.text = ae.HintStr;
            mizarImg.sprite = ResourceManager.GetResource<Sprite>(ImageReference.Mizar_Normal);

            for (int i = 0; i < 4; ++i)
                words[i].SetSetting(ae.Words[i]);

            wordsData = ae.Words;
            successStr = ae.SuccessStr;
            successChanges = ae.SuccessChanges;
            failChanges = ae.FailChanges;
            failStr = ae.FailStr;

            isSubmit = false;
        }
        public void OnUpdate()
        {
            if (isSubmit && gm.IsKeyDown())
                gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        public void Release()
        {
            rootObj.SetActive(false);
        }
        private void Submit()
        {
            if (gm.arSystem.isSubmit)
                gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
            else
            {
                gm.arSystem.isSubmit = true;
                for (int i = 0; i < 4; ++i)
                {
                    if (!gm.arSystem.wordsData[i].isFixedWord && gm.arSystem.wordsData[i].correctIndex != gm.arSystem.words[i].Index)
                    {
                        mizarImg.sprite = ResourceManager.GetResource<Sprite>(ImageReference.Mizar_Normal);
                        gm.arSystem.descTMP.text = gm.arSystem.failStr;
                        foreach (var change in gm.arSystem.failChanges)
                            change.Calculate();
                        return;
                    }
                }
                mizarImg.sprite = ResourceManager.GetResource<Sprite>(ImageReference.Mizar_Normal);
                gm.arSystem.descTMP.text = gm.arSystem.successStr;
                foreach (var change in gm.arSystem.successChanges)
                    change.Calculate();
            }
        }
        #endregion
    }
}