using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using TMPro;

namespace YS
{
    public class TitleManager : Singleton<TitleManager>
    {
        #region Field
        [BoxGroup("패널", true, true)]
        [LabelText("터치시작 패널")]
        public GameObject clickToStartPanel;
        [BoxGroup("패널")]
        [LabelText("메뉴 패널"), Tooltip("모든 메뉴들의 상위 패널")]
        public GameObject menuPanel;
        [BoxGroup("패널")]
        [LabelText("설정 컴포넌트")]
        public SettingComponent settingComp;
        [BoxGroup("패널")]
        [LabelText("메인 패널")]
        public GameObject mainPanel;
        [BoxGroup("패널")]
        [LabelText("게임시작 패널")]
        public GameObject startPanel;
        [BoxGroup("패널")]
        [LabelText("불러오기 패널")]
        public GameObject loadPanel;
        [BoxGroup("패널")]
        [LabelText("사진첩 패널")]
        public GameObject galleryPanel;

        [BoxGroup("메인 메뉴", true, true)]
        [LabelText("설정 버튼")]
        public Button settingBtn;
        [BoxGroup("메인 메뉴")]
        [LabelText("뒤로가기 버튼")]
        public Button backBtn;
        [BoxGroup("메인 메뉴")]
        [LabelText("게임시작 버튼")]
        public Button startBtn;
        [BoxGroup("메인 메뉴")]
        [LabelText("불러오기 버튼")]
        public Button loadBtn;
        [BoxGroup("메인 메뉴")]
        [LabelText("사진첩 버튼")]
        public Button galleryBtn;

        [BoxGroup("메인 메뉴/게임시작", true, true)]
        [LabelText("슬롯1 버튼")]
        public Button slot1StartBtn;
        [BoxGroup("메인 메뉴/게임시작")]
        [LabelText("슬롯2 버튼")]
        public Button slot2StartBtn;
        [BoxGroup("메인 메뉴/게임시작")]
        [LabelText("슬롯3 버튼")]
        public Button slot3StartBtn;

        [BoxGroup("메인 메뉴/불러오기", true, true)]
        [LabelText("퀵슬롯 버튼")]
        public Button quickSlotLoadBtn;
        [BoxGroup("메인 메뉴/불러오기")]
        [LabelText("슬롯1 버튼")]
        public Button slot1LoadBtn;
        [BoxGroup("메인 메뉴/불러오기")]
        [LabelText("슬롯2 버튼")]
        public Button slot2LoadBtn;
        [BoxGroup("메인 메뉴/불러오기")]
        [LabelText("슬롯3 버튼")]
        public Button slot3LoadBtn;

        private Coroutine coroutineTextPreview;
        private StateStack stateStack = new StateStack();
        [ShowInInspector]
        private int currentState => stateStack.CurrentState;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            Setting.LoadSetting();
            SaveLoadData saveData;
            saveData = SaveLoad.LoadData(0);
            if (saveData.saveTime != null && saveData.saveTime != "")
                quickSlotLoadBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = saveData.saveTime;

            saveData = SaveLoad.LoadData(1);
            if (saveData.saveTime != null && saveData.saveTime != "")
            {
                slot1StartBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = saveData.saveTime;
                slot1LoadBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = saveData.saveTime;
            }
            saveData = SaveLoad.LoadData(2);
            if (saveData.saveTime != null && saveData.saveTime != "")
            {
                slot2StartBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = saveData.saveTime;
                slot2LoadBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = saveData.saveTime;
            }
            saveData = SaveLoad.LoadData(3);
            if (saveData.saveTime != null && saveData.saveTime != "")
            {
                slot3StartBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = saveData.saveTime;
                slot3LoadBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = saveData.saveTime;
            }
        }
        private void Start()
        {
            stateStack.OnAfterPushEvent += PushEvent;
            stateStack.OnPopEvent += PopEvent;

            stateStack.PushState((int)TITLE_UI_STATE.TOUCH_TO_START);

            settingBtn.onClick.AddListener(() => { stateStack.PushState((int)TITLE_UI_STATE.SETTING); });
            settingComp.OnHideWindowEvent += () => { stateStack.PopState(); };
            backBtn.onClick.AddListener(() => { stateStack.PopState(); });
            startBtn.onClick.AddListener(() => { stateStack.PushState((int)TITLE_UI_STATE.GAMESTART); });
            loadBtn.onClick.AddListener(() => { stateStack.PushState((int)TITLE_UI_STATE.LOAD); });
            galleryBtn.onClick.AddListener(() => { stateStack.PushState((int)TITLE_UI_STATE.GALLERY); });

            slot1StartBtn.onClick.AddListener(() => { SaveLoad.OnNewGame(1); });
            slot2StartBtn.onClick.AddListener(() => { SaveLoad.OnNewGame(2); });
            slot3StartBtn.onClick.AddListener(() => { SaveLoad.OnNewGame(3); });

            quickSlotLoadBtn.onClick.AddListener(() => { SaveLoad.OnStartGame(0); });
            slot1LoadBtn.onClick.AddListener(() => { SaveLoad.OnStartGame(1); });
            slot2LoadBtn.onClick.AddListener(() => { SaveLoad.OnStartGame(2); });
            slot3LoadBtn.onClick.AddListener(() => { SaveLoad.OnStartGame(3); });
        }
        private void Update()
        {
            if (clickToStartPanel.activeInHierarchy && Input.anyKeyDown)
            {
                stateStack.PopState();
                stateStack.PushState((int)TITLE_UI_STATE.MENU);
            }
        }
        #endregion

        #region Methods
        public void PushEvent(int stateIndex)
        {
            switch ((TITLE_UI_STATE)stateIndex)
            {
                case TITLE_UI_STATE.MENU:
                    menuPanel.SetActive(true);
                    break;
                case TITLE_UI_STATE.SETTING:
                    settingComp.ShowWindow();
                    break;
                case TITLE_UI_STATE.GAMESTART:
                    mainPanel.SetActive(false);
                    startPanel.SetActive(true);
                    break;
                case TITLE_UI_STATE.LOAD:
                    mainPanel.SetActive(false);
                    loadPanel.SetActive(true);
                    break;
                case TITLE_UI_STATE.GALLERY:
                    mainPanel.SetActive(false);
                    galleryPanel.SetActive(true);
                    break;
            }
        }
        private void PopEvent(int stateIndex)
        {
            switch ((TITLE_UI_STATE)stateIndex)
            {
                case TITLE_UI_STATE.TOUCH_TO_START:
                    clickToStartPanel.SetActive(false);
                    break;
                case TITLE_UI_STATE.MENU:
                    Application.Quit();
                    break;
                case TITLE_UI_STATE.GAMESTART:
                    startPanel.SetActive(false);
                    mainPanel.SetActive(true);
                    break;
                case TITLE_UI_STATE.LOAD:
                    loadPanel.SetActive(false);
                    mainPanel.SetActive(true);
                    break;
                case TITLE_UI_STATE.GALLERY:
                    galleryPanel.SetActive(false);
                    mainPanel.SetActive(true);
                    break;
            }
        }
        #endregion
        private enum TITLE_UI_STATE
        {
            TOUCH_TO_START,
            MENU,
            SETTING,
            GAMESTART,
            LOAD,
            GALLERY
        }
    }
}