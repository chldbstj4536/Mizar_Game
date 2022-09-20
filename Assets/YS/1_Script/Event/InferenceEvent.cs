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
        [LabelText("추리를 진행할 캐릭터")]
        private Sprite charImg;
        [SerializeField]
        [LabelText("추리할 아이템")]
        private ITEM_INDEX itemIndex;
        [SerializeField]
        [LabelText("선택지들")]
        private InferenceDialogData[] choiceDatas = new InferenceDialogData[1];
        [FoldoutGroup("틀렸던 선택지 선택시 나오는 문구", false), SerializeField]
        [HideLabel]
        private DialogEvent twiceFailDialogData;
        [FoldoutGroup("스코어에 따른 변수 제어", false), SerializeField]
        [FoldoutGroup("스코어에 따른 변수 제어/한번에 성공"), HideLabel]
        private ChangeVariableInTable[] perfectChange = new ChangeVariableInTable[0];
        [FoldoutGroup("스코어에 따른 변수 제어"), SerializeField]
        [FoldoutGroup("스코어에 따른 변수 제어/두번만에 성공"), HideLabel]
        private ChangeVariableInTable[] greatChange = new ChangeVariableInTable[0];
        [FoldoutGroup("스코어에 따른 변수 제어"), SerializeField]
        [FoldoutGroup("스코어에 따른 변수 제어/실패"), HideLabel]
        private ChangeVariableInTable[] failChange = new ChangeVariableInTable[0];
        [SerializeField, MaxValue("@choiceDatas.Length - 1")]
        [LabelText("정답"), Tooltip("추리 선택지들중 올바른 답")]
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
        [LabelText("선택지 내용")]
        public string choiceStr;
        [LabelText("선택 후 대화내용")]
        public DialogEvent[] dialogs;
    }
    [System.Serializable]
    public class InferenceSystem
    {
        #region Fields
        [FoldoutGroup("추리 UI", false)]
        [LabelText("추리 패널 UI"), Tooltip("조사 패널 루트 게임오브젝트")]
        public GameObject rootObj;
        [FoldoutGroup("추리 UI")]
        [LabelText("추리 캐릭터 이미지"), Tooltip("추리하는 캐릭터 이미지 컴포넌트")]
        public Image character;
        [FoldoutGroup("추리 UI")]
        [LabelText("추리 아이템 이미지"), Tooltip("추리하는 아이템 이미지 컴포넌트")]
        public Image item;
        [FoldoutGroup("추리 UI")]
        [LabelText("아이템 설명 TMP"), Tooltip("추리 아이템 설명 TMP 컴포넌트")]
        public TMP_Text tmp_itemDesc;
        [FoldoutGroup("추리 UI")]
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true), DisableContextMenu]
        [LabelText("추리 선택지들"), Tooltip("추리에 대한 선택지의 버튼 컴포넌트들")]
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

            // 성공
            if (choiceIndex == correctIndex)
            {
                bExit = true;

                // 한번에 성공
                if (lastChoiceIndex == -1)
                    foreach (var changeVar in perfectChange)
                        changeVar.Calculate();
                // 두번만에 성공
                else
                    foreach (var changeVar in greatChange)
                        changeVar.Calculate();
            }
            // 실패
            else if (lastChoiceIndex != -1)
            {
                bExit = true;

                foreach (var changeVar in failChange)
                    changeVar.Calculate();

                // 같은 선택지로 실패
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
        /// 다이얼로그 이벤트 발생시 호출되는 함수
        /// </summary>
        public void OnDialogEvent()
        {
            // 마우스 클릭시 타이핑이 안끝났다면 타이핑 끝내고, 타이핑이 다 되어있는 상태라면 다음 다이얼로그 설정
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