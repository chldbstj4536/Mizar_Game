using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using TMPro;
using System;

namespace YS
{
    public class InGameUIManager : SingletonMono<InGameUIManager>
    {
        public enum INGAME_UI_STATE
        {
            GAME,
            INVEN,
            MENU,
            SAVE,
            LOAD,
            SETTING,
            LOG,
            EXIT
        }

        #region Field
        [BoxGroup("패널", true, true)]
        [LabelText("인벤토리 UI")]
        public InventoryComponent invenComp;
        [BoxGroup("패널")]
        [LabelText("메인메뉴 UI")]
        public GameObject ui;
        [BoxGroup("패널")]
        [LabelText("메뉴 패널 UI")]
        public SlideEffect menuPanel;
        [BoxGroup("패널")]
        [LabelText("세이브 패널 UI")]
        public SlideEffect savePanel;
        [BoxGroup("패널")]
        [LabelText("로드 패널 UI")]
        public SlideEffect loadPanel;
        [BoxGroup("패널")]
        [LabelText("로그 패널 UI")]
        public GameObject logPanel;

        [BoxGroup("UI", true, true)]
        [LabelText("메뉴 버튼")]
        public Button menuBtn;
        [BoxGroup("UI")]
        [LabelText("인벤토리 버튼")]
        public Button invenBtn;
        [BoxGroup("UI/인벤토리", true, true)]
        [LabelText("인벤토리 닫기 버튼")]
        public Button invenExitBtn;
        [BoxGroup("UI/메뉴", true, true)]
        [LabelText("저장 버튼")]
        public Button saveBtn;
        [BoxGroup("UI/메뉴/저장", true, true)]
        [LabelText("슬롯1")]
        public Button slot1SaveBtn;
        [BoxGroup("UI/메뉴/저장")]
        [LabelText("슬롯2")]
        public Button slot2SaveBtn;
        [BoxGroup("UI/메뉴/저장")]
        [LabelText("슬롯3")]
        public Button slot3SaveBtn;
        [BoxGroup("UI/메뉴")]
        [LabelText("불러오기 버튼")]
        public Button loadBtn;
        [BoxGroup("UI/메뉴/불러오기", true, true)]
        [LabelText("퀵슬롯")]
        public Button quickSlotLoadBtn;
        [BoxGroup("UI/메뉴/불러오기")]
        [LabelText("슬롯1")]
        public Button slot1LoadBtn;
        [BoxGroup("UI/메뉴/불러오기")]
        [LabelText("슬롯2")]
        public Button slot2LoadBtn;
        [BoxGroup("UI/메뉴/불러오기")]
        [LabelText("슬롯3")]
        public Button slot3LoadBtn;
        [BoxGroup("UI/메뉴")]
        [LabelText("설정 버튼")]
        public Button settingBtn;
        [BoxGroup("UI/메뉴")]
        [LabelText("설정 컴포넌트")]
        public SettingComponent settingComp;
        [BoxGroup("UI/메뉴")]
        [LabelText("로그 버튼")]
        public Button logBtn;
        [BoxGroup("UI/메뉴")]
        [LabelText("종료 버튼")]
        public Button exitBtn;

        // UI 상태 변수
        private StateStack stateStack = new StateStack();
        private bool isInMenu = false;
        private Coroutine coroutineTextPreview;
        #endregion

        #region Properties
        public bool IsShowingInventory => stateStack.CurrentState == (int)INGAME_UI_STATE.INVEN;
        [ShowInInspector]
        public INGAME_UI_STATE CurrentState => (INGAME_UI_STATE)stateStack.CurrentState;
        public StateStack StateStack => stateStack;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            Setting.LoadSetting();
            //SaveLoad.WriteSaveData(0, quickSlotLoadBtn.transform.GetChild(0).GetComponent<TMP_Text>());
            //SaveLoad.WriteSaveData(1, slot1SaveBtn.transform.GetChild(0).GetComponent<TMP_Text>());
            //SaveLoad.WriteSaveData(1, slot1LoadBtn.transform.GetChild(0).GetComponent<TMP_Text>());
            //SaveLoad.WriteSaveData(2, slot2SaveBtn.transform.GetChild(0).GetComponent<TMP_Text>());
            //SaveLoad.WriteSaveData(2, slot2LoadBtn.transform.GetChild(0).GetComponent<TMP_Text>());
            //SaveLoad.WriteSaveData(3, slot3SaveBtn.transform.GetChild(0).GetComponent<TMP_Text>());
            //SaveLoad.WriteSaveData(3, slot3LoadBtn.transform.GetChild(0).GetComponent<TMP_Text>());
        }
        private void Start()
        {
            stateStack.OnPushEvent += PushEvent;
            stateStack.OnPopEvent += PopEvent;

            stateStack.PushState((int)INGAME_UI_STATE.GAME);

            // 버튼들에 이벤트 등록
            menuBtn.onClick.AddListener(() => { OnPush(INGAME_UI_STATE.MENU); });

            invenBtn.onClick.AddListener(() => { OnPush(INGAME_UI_STATE.INVEN); });
            invenExitBtn.onClick.AddListener(() => { stateStack.PopState(); });

            saveBtn.onClick.AddListener(() => { OnPush(INGAME_UI_STATE.SAVE); });
            slot1SaveBtn.onClick.AddListener(() =>
            {
                SaveDataManager.Instance.SaveInGameData(1, GameManager.Instance.CurrentData);
                slot1SaveBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = DateTime.Now.ToString();
                slot1LoadBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = DateTime.Now.ToString();
            });

            loadBtn.onClick.AddListener(() => { OnPush(INGAME_UI_STATE.LOAD); });
            quickSlotLoadBtn.onClick.AddListener(() => { SaveDataManager.Instance.StartGameWithSave(0); });

            settingBtn.onClick.AddListener(() => { OnPush(INGAME_UI_STATE.SETTING); });
            settingComp.OnHideWindowEvent += () => { stateStack.PopState(); };

            logBtn.onClick.AddListener(() => { OnPush(INGAME_UI_STATE.LOG); });
            exitBtn.onClick.AddListener(() => { OnPush(INGAME_UI_STATE.EXIT); });
        }

        private void Update()
        {
            // esc키 눌리면 UI Pop
            if (stateStack.CurrentState != (int)INGAME_UI_STATE.GAME && Input.GetKeyDown(KeyCode.Escape))
                stateStack.PopState();
        }
        #endregion

        #region Methods
        #region State Methods
        /// <summary>
        /// UI 상태 추가 전 이벤트
        /// </summary>
        /// <param name="pushState">추가될 상태</param>
        private void OnPush(INGAME_UI_STATE pushState)
        {
            if (CurrentState == INGAME_UI_STATE.SETTING && pushState == INGAME_UI_STATE.SETTING)
                return;

            if (stateStack.Contains((int)pushState))
                stateStack.PopState((uint)pushState);
            else
                stateStack.PushState((uint)pushState);
        }
        private void PushEvent(int pushState)
        {
            switch ((INGAME_UI_STATE)pushState)
            {
                case INGAME_UI_STATE.INVEN:
                    ui.SetActive(false);
                    invenComp.OpenInventory();
                    break;
                case INGAME_UI_STATE.MENU:
                    menuPanel.gameObject.SetActive(true);
                    menuPanel.SetSlide(Vector3.zero, false);
                    break;
                case INGAME_UI_STATE.SAVE:
                    savePanel.gameObject.SetActive(true);
                    savePanel.SetSlide(new Vector2(0.0f, -340.0f), false);
                    break;
                case INGAME_UI_STATE.LOAD:
                    loadPanel.gameObject.SetActive(true);
                    loadPanel.SetSlide(new Vector2(0.0f, -340.0f), false);
                    break;
                case INGAME_UI_STATE.SETTING:
                    settingComp.ShowWindow();
                    break;
                case INGAME_UI_STATE.LOG:
                    logPanel.SetActive(true);
                    break;
                case INGAME_UI_STATE.EXIT:
                    ExitGame();
                    break;
            }
        }
        /// <summary>
        /// UI상태 전단계로 가기
        /// </summary>
        /// <param name="popState">제거된 상태</param>
        public void PopEvent(int popState)
        {
            switch ((INGAME_UI_STATE)popState)
            {
                case INGAME_UI_STATE.INVEN:
                    invenComp.CloseInventory();
                    ui.SetActive(true);
                    break;
                case INGAME_UI_STATE.MENU:
                    menuPanel.SetSlide(new Vector2(-500.0f, 0.0f), true);
                    break;
                case INGAME_UI_STATE.SAVE:
                    savePanel.SetSlide(new Vector2(-500.0f, -340.0f), true);
                    break;
                case INGAME_UI_STATE.LOAD:
                    loadPanel.SetSlide(new Vector2(-500.0f, -340.0f), true);
                    break;
                case INGAME_UI_STATE.LOG:
                    logPanel.SetActive(false);
                    break;
                case INGAME_UI_STATE.EXIT:
                    SceneManager.LoadScene(0);
                    break;
            }
        }
        public void ExitGame()
        {
            SceneManager.LoadScene(0);
            // 타이틀 씬으로 돌아가게 구현
            // 필요하다면 저장할건지 물어보는것도 생각해야할듯
        }
        #endregion
        #endregion
    }
}