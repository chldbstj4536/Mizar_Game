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

            // ��ư�鿡 �̺�Ʈ ���
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
            // Ÿ��Ʋ ������ ���ư��� ����
            // �ʿ��ϴٸ� �����Ұ��� ����°͵� �����ؾ��ҵ�
        }
        #endregion
        #endregion
    }
}