using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Video;

namespace YS
{
    public class TitleManager : Singleton<TitleManager>
    {
        #region Field
        [BoxGroup("�г�", true, true)]
        [LabelText("��ġ���� �г�")]
        public GameObject clickToStartPanel;
        [BoxGroup("�г�")]
        [LabelText("�޴� �г�"), Tooltip("��� �޴����� ���� �г�")]
        public GameObject menuPanel;
        [BoxGroup("�г�")]
        [LabelText("���� ������Ʈ")]
        public SettingComponent settingComp;
        [BoxGroup("�г�")]
        [LabelText("���� �г�")]
        public GameObject mainPanel;
        [BoxGroup("�г�")]
        [LabelText("���ӽ��� �г�")]
        public GameObject startPanel;
        [BoxGroup("�г�")]
        [LabelText("�ҷ����� �г�")]
        public GameObject loadPanel;
        [BoxGroup("�г�")]
        [LabelText("����ø �г�")]
        public GameObject galleryPanel;
        [BoxGroup("�г�")]
        [LabelText("���")]
        public SpriteRenderer bg;

        [BoxGroup("���� �޴�", true, true)]
        [LabelText("���� ��ư")]
        public Button settingBtn;
        [BoxGroup("���� �޴�")]
        [LabelText("�ڷΰ��� ��ư")]
        public Button backBtn;
        [BoxGroup("���� �޴�")]
        [LabelText("���ӽ��� ��ư")]
        public Button startBtn;
        [BoxGroup("���� �޴�")]
        [LabelText("�ҷ����� ��ư")]
        public Button loadBtn;
        [BoxGroup("���� �޴�")]
        [LabelText("����ø ��ư")]
        public Button galleryBtn;

        [BoxGroup("���� �޴�/���ӽ���", true, true)]
        [LabelText("����1 ��ư")]
        public Button slot1StartBtn;
        [BoxGroup("���� �޴�/���ӽ���")]
        [LabelText("����2 ��ư")]
        public Button slot2StartBtn;
        [BoxGroup("���� �޴�/���ӽ���")]
        [LabelText("����3 ��ư")]
        public Button slot3StartBtn;

        [BoxGroup("���� �޴�/�ҷ�����", true, true)]
        [LabelText("������ ��ư")]
        public Button quickSlotLoadBtn;
        [BoxGroup("���� �޴�/�ҷ�����")]
        [LabelText("����1 ��ư")]
        public Button slot1LoadBtn;
        [BoxGroup("���� �޴�/�ҷ�����")]
        [LabelText("����2 ��ư")]
        public Button slot2LoadBtn;
        [BoxGroup("���� �޴�/�ҷ�����")]
        [LabelText("����3 ��ư")]
        public Button slot3LoadBtn;

        private VideoPlayer vp;
        private Coroutine coroutineTextPreview;
        private StateStack stateStack = new StateStack();
        [ShowInInspector]
        private int currentState => stateStack.CurrentState;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            Setting.LoadSetting();
            SaveLoad.WriteSaveData(0, quickSlotLoadBtn.transform.GetChild(0).GetComponent<TMP_Text>());
            SaveLoad.WriteSaveData(1, slot1StartBtn.transform.GetChild(0).GetComponent<TMP_Text>());
            SaveLoad.WriteSaveData(1, slot1LoadBtn.transform.GetChild(0).GetComponent<TMP_Text>());
            SaveLoad.WriteSaveData(2, slot2StartBtn.transform.GetChild(0).GetComponent<TMP_Text>());
            SaveLoad.WriteSaveData(2, slot2LoadBtn.transform.GetChild(0).GetComponent<TMP_Text>());
            SaveLoad.WriteSaveData(3, slot3StartBtn.transform.GetChild(0).GetComponent<TMP_Text>());
            SaveLoad.WriteSaveData(3, slot3LoadBtn.transform.GetChild(0).GetComponent<TMP_Text>());

            vp = Camera.allCameras[0].GetComponent<VideoPlayer>();
        }
        private void Start()
        {
            stateStack.OnPushEvent += PushEvent;
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
                case TITLE_UI_STATE.TOUCH_TO_START:
                    bg.sprite = ResourceManager.GetResource<Sprite>(ResourcePath.TouchToStartBG);
                    vp.Prepare();
                    break;
                case TITLE_UI_STATE.MENU:
                    if (!vp.isPrepared)
                        vp.prepareCompleted += (VideoPlayer vp) => { vp.Play(); };
                    else
                    {
                        clickToStartPanel.SetActive(false);
                        vp.Play();
                    }

                    vp.loopPointReached += (VideoPlayer vp) =>
                    {
                        menuPanel.SetActive(true);
                    };
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