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
        #region Field
        [BoxGroup("������", true, true)]
        [LabelText("�κ��丮")]
        public WindowComponent invenComp;
        [BoxGroup("�г�")]
        [LabelText("���̺�")]
        public WindowComponent saveComp;
        [BoxGroup("�г�")]
        [LabelText("�α�")]
        public WindowComponent logComp;
        [BoxGroup("�г�")]
        [LabelText("����")]
        public WindowComponent settingComp;

        [BoxGroup("UI", true, true)]
        [LabelText("UI �г�")]
        public SlideEffect uiSE;
        [BoxGroup("UI")]
        [LabelText("UI ��� ��ư")]
        public Button togBtn;
        [BoxGroup("UI")]
        [LabelText("�κ��丮 ��ư")]
        public Button invenBtn;
        [BoxGroup("UI")]
        [LabelText("���� ��ư")]
        public Button saveBtn;
        [BoxGroup("UI")]
        [LabelText("�α� ��ư")]
        public Button logBtn;
        [BoxGroup("UI")]
        [LabelText("���� ��ư")]
        public Button settingBtn;
        [BoxGroup("UI")]
        [LabelText("���� ��ư")]
        public Button exitBtn;

        public SaveButton[] saveGameBtns;

        private bool isInGame = true;
        private bool uiOn = true;
        #endregion

        #region Properties
        public bool IsInGame => isInGame;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            Setting.LoadSetting();

            // ���̺� ���� �ҷ�����
            for (int i = 0; i < saveGameBtns.Length; ++i)
                saveGameBtns[i].SetSaveButton(i);
        }
        private void Start()
        {
            togBtn.onClick.AddListener(() =>
            {
                if (uiOn)
                {
                    uiOn = false;
                    uiSE.SetSlide(Vector2.zero, false);
                }
                else
                {
                    uiOn = true;
                    uiSE.SetSlide(Vector2.down * 150, false);
                }
            });

            invenComp.OnOpenEvent = () => { isInGame = false; };
            invenComp.OnCloseEvent = () => { isInGame = true; };
            saveComp.OnOpenEvent = () => { isInGame = false; };
            saveComp.OnCloseEvent = () => { isInGame = true; };
            logComp.OnOpenEvent = () => { isInGame = false; };
            logComp.OnCloseEvent = () => { isInGame = true; };
            settingComp.OnOpenEvent = () => { isInGame = false; };
            settingComp.OnCloseEvent = () => { isInGame = true; };

            invenBtn.onClick.AddListener(() => { invenComp.OpenWindow(); });
            saveBtn.onClick.AddListener(() => { saveComp.OpenWindow(); });
            logBtn.onClick.AddListener(() => { logComp.OpenWindow(); });
            settingBtn.onClick.AddListener(() => { settingComp.OpenWindow(); });
            exitBtn.onClick.AddListener(() => { ExitGame(); });
        }
        private void Update()
        {
            if (!isInGame)
                return;

            // Inventory ����Ű
            if (Input.GetKeyDown(KeyCode.E))
                invenComp.OpenWindow();
            // Save ����Ű
            else if (Input.GetKeyDown(KeyCode.S))
                saveComp.OpenWindow();
            // Log ����Ű
            else if (Input.GetKeyDown(KeyCode.Q))
                logComp.OpenWindow();
            // Settings ����Ű
            else if (Input.GetKeyDown(KeyCode.R))
                settingComp.OpenWindow();
        }
        #endregion

        #region Methods
        public void ExitGame()
        {
            SceneManager.LoadScene(0);
            // Ÿ��Ʋ ������ ���ư��� ����
        }
        #endregion
    }
}