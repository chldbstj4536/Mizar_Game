using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using TMPro;

namespace YS
{
    public class InGameUIManager : Singleton<InGameUIManager>
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
        [BoxGroup("�г�", true, true)]
        [LabelText("�κ��丮 UI")]
        public InventoryComponent invenComp;
        [BoxGroup("�г�")]
        [LabelText("���θ޴� UI")]
        public GameObject ui;
        [BoxGroup("�г�")]
        [LabelText("�޴� �г� UI")]
        public SlideEffect menuPanel;
        [BoxGroup("�г�")]
        [LabelText("���̺� �г� UI")]
        public SlideEffect savePanel;
        [BoxGroup("�г�")]
        [LabelText("�ε� �г� UI")]
        public SlideEffect loadPanel;
        [BoxGroup("�г�")]
        [LabelText("�α� �г� UI")]
        public GameObject logPanel;

        [BoxGroup("UI", true, true)]
        [LabelText("�޴� ��ư")]
        public Button menuBtn;
        [BoxGroup("UI")]
        [LabelText("�κ��丮 ��ư")]
        public Button invenBtn;
        [BoxGroup("UI/�κ��丮", true, true)]
        [LabelText("�κ��丮 �ݱ� ��ư")]
        public Button invenExitBtn;
        [BoxGroup("UI/�޴�", true, true)]
        [LabelText("���� ��ư")]
        public Button saveBtn;
        [BoxGroup("UI/�޴�/����", true, true)]
        [LabelText("����1")]
        public Button slot1SaveBtn;
        [BoxGroup("UI/�޴�/����")]
        [LabelText("����2")]
        public Button slot2SaveBtn;
        [BoxGroup("UI/�޴�/����")]
        [LabelText("����3")]
        public Button slot3SaveBtn;
        [BoxGroup("UI/�޴�")]
        [LabelText("�ҷ����� ��ư")]
        public Button loadBtn;
        [BoxGroup("UI/�޴�/�ҷ�����", true, true)]
        [LabelText("������")]
        public Button quickSlotLoadBtn;
        [BoxGroup("UI/�޴�/�ҷ�����")]
        [LabelText("����1")]
        public Button slot1LoadBtn;
        [BoxGroup("UI/�޴�/�ҷ�����")]
        [LabelText("����2")]
        public Button slot2LoadBtn;
        [BoxGroup("UI/�޴�/�ҷ�����")]
        [LabelText("����3")]
        public Button slot3LoadBtn;
        [BoxGroup("UI/�޴�")]
        [LabelText("���� ��ư")]
        public Button settingBtn;
        [BoxGroup("UI/�޴�")]
        [LabelText("���� ������Ʈ")]
        public SettingComponent settingComp;
        [BoxGroup("UI/�޴�")]
        [LabelText("�α� ��ư")]
        public Button logBtn;
        [BoxGroup("UI/�޴�")]
        [LabelText("���� ��ư")]
        public Button exitBtn;

        // UI ���� ����
        private StateStack stateStack = new StateStack();
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
            SaveLoadData saveData;
            saveData = SaveLoad.LoadData(0);
            if (saveData.saveTime != null && saveData.saveTime != "")
                quickSlotLoadBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = saveData.saveTime;

            saveData = SaveLoad.LoadData(1);
            if (saveData.saveTime != null && saveData.saveTime != "")
            {
                slot1SaveBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = saveData.saveTime;
                slot1LoadBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = saveData.saveTime;
            }
            saveData = SaveLoad.LoadData(2);
            if (saveData.saveTime != null && saveData.saveTime != "")
            {
                slot2SaveBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = saveData.saveTime;
                slot2LoadBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = saveData.saveTime;
            }
            saveData = SaveLoad.LoadData(3);
            if (saveData.saveTime != null && saveData.saveTime != "")
            {
                slot3SaveBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = saveData.saveTime;
                slot3LoadBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = saveData.saveTime;
            }
        }
        private void Start()
        {
            stateStack.OnBeforePushEvent += BeforePushEvent;
            stateStack.OnAfterPushEvent += AfterPushEvent;
            stateStack.OnPopEvent += PopEvent;

            stateStack.PushState((int)INGAME_UI_STATE.GAME);

            // ��ư�鿡 �̺�Ʈ ���
            menuBtn.onClick.AddListener(() => { stateStack.PushState((int)INGAME_UI_STATE.MENU); });

            invenBtn.onClick.AddListener(() => { stateStack.PushState((int)INGAME_UI_STATE.INVEN); });
            invenExitBtn.onClick.AddListener(() => { stateStack.PopState(); });

            saveBtn.onClick.AddListener(() => { stateStack.PushState((int)INGAME_UI_STATE.SAVE); });
            slot1SaveBtn.onClick.AddListener(() => { SaveLoad.OnOverwriteGame(1, GameManager.Instance.CurrentData); });
            slot2SaveBtn.onClick.AddListener(() => { SaveLoad.OnOverwriteGame(2, GameManager.Instance.CurrentData); });
            slot3SaveBtn.onClick.AddListener(() => { SaveLoad.OnOverwriteGame(3, GameManager.Instance.CurrentData); });

            loadBtn.onClick.AddListener(() => { stateStack.PushState((int)INGAME_UI_STATE.LOAD); });
            quickSlotLoadBtn.onClick.AddListener(() => { SaveLoad.OnStartGame(0); });
            slot1LoadBtn.onClick.AddListener(() => { SaveLoad.OnStartGame(1); });
            slot2LoadBtn.onClick.AddListener(() => { SaveLoad.OnStartGame(2); });
            slot3LoadBtn.onClick.AddListener(() => { SaveLoad.OnStartGame(3); });

            settingBtn.onClick.AddListener(() => { stateStack.PushState((int)INGAME_UI_STATE.SETTING); });
            settingComp.OnHideWindowEvent += () => { stateStack.PopState(); };

            logBtn.onClick.AddListener(() => { stateStack.PushState((int)INGAME_UI_STATE.LOG); });
            exitBtn.onClick.AddListener(() => { stateStack.PushState((int)INGAME_UI_STATE.EXIT); });
        }

        private void Update()
        {
            // escŰ ������ UI Pop
            if (stateStack.CurrentState != (int)INGAME_UI_STATE.GAME && Input.GetKeyDown(KeyCode.Escape))
                stateStack.PopState();
        }
        #endregion

        #region Methods
        #region State Methods
        /// <summary>
        /// UI ���� �߰� �� �̺�Ʈ
        /// </summary>
        /// <param name="pushState">�߰��� ����</param>
        private void BeforePushEvent(int pushState)
        {
            if (CurrentState == INGAME_UI_STATE.MENU)
                return;

            switch ((INGAME_UI_STATE)pushState)
            {
                case INGAME_UI_STATE.MENU:
                    stateStack.PopState((uint)INGAME_UI_STATE.GAME, false);
                    break;
                case INGAME_UI_STATE.SAVE:
                case INGAME_UI_STATE.LOAD:
                case INGAME_UI_STATE.SETTING:
                case INGAME_UI_STATE.LOG:
                    stateStack.PopState((uint)INGAME_UI_STATE.MENU, false);
                    break;
            }
        }
        public void AfterPushEvent(int pushState)
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
        /// UI���� ���ܰ�� ����
        /// </summary>
        /// <param name="popState">���ŵ� ����</param>
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
                case INGAME_UI_STATE.LOAD:
                    savePanel.SetSlide(new Vector2(-500.0f, -340.0f), true);
                    break;
                case INGAME_UI_STATE.SETTING:
                    settingComp.HideWindow();
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
            // Ÿ��Ʋ ������ ���ư��� ����
            // �ʿ��ϴٸ� �����Ұ��� ����°͵� �����ؾ��ҵ�
        }
        #endregion
        #endregion
    }
}