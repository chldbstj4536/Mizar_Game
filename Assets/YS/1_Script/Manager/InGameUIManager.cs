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
        [BoxGroup("윈도우", true, true)]
        [LabelText("인벤토리")]
        public WindowComponent invenComp;
        [BoxGroup("패널")]
        [LabelText("세이브")]
        public WindowComponent saveComp;
        [BoxGroup("패널")]
        [LabelText("로그")]
        public WindowComponent logComp;
        [BoxGroup("패널")]
        [LabelText("설정")]
        public WindowComponent settingComp;
        [BoxGroup("패널")]
        [LabelText("종료")]
        public WindowComponent exitWin;

        [BoxGroup("UI", true, true)]
        [LabelText("UI 패널")]
        public SlideEffect uiSE;
        [BoxGroup("UI")]
        [LabelText("UI 토글 버튼")]
        public Button togBtn;
        [BoxGroup("UI")]
        [LabelText("인벤토리 버튼")]
        public Button invenBtn;
        [BoxGroup("UI")]
        [LabelText("저장 버튼")]
        public Button saveBtn;
        [BoxGroup("UI")]
        [LabelText("로그 버튼")]
        public Button logBtn;
        [BoxGroup("UI")]
        [LabelText("설정 버튼")]
        public Button settingBtn;
        [BoxGroup("UI")]
        [LabelText("종료 버튼")]
        public Button exitBtn;

        public SaveButton[] saveGameBtns;
        public Button exitYesBtn;
        public float logNameSize = 36;
        public float logDescSize = 30;

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

            // 세이브 파일 불러오기
            for (int i = 0; i < saveGameBtns.Length; ++i)
                saveGameBtns[i].SetSaveButton(i);
        }
        private void Start()
        {
            togBtn.onClick.AddListener(() =>
            {
                if (uiOn = !uiOn)
                    uiSE.SetSlide(Vector2.zero, false);
                else
                    uiSE.SetSlide(Vector2.down * 150, false);
            });

            invenComp.OnOpenEvent = () => { isInGame = false; };
            invenComp.OnCloseEvent = () => { isInGame = true; };
            saveComp.OnOpenEvent = () => { isInGame = false; };
            saveComp.OnCloseEvent = () => { isInGame = true; };
            logComp.OnOpenEvent = () => { isInGame = false; };
            logComp.OnCloseEvent = () => { isInGame = true; };
            settingComp.OnOpenEvent = () => { isInGame = false; };
            settingComp.OnCloseEvent = () => { isInGame = true; };
            exitWin.OnOpenEvent = () => { isInGame = false; };
            exitWin.OnCloseEvent = () => { isInGame = true; };

            invenBtn.onClick.AddListener(() => { invenComp.OpenWindow(); });
            saveBtn.onClick.AddListener(() => { saveComp.OpenWindow(); });
            logBtn.onClick.AddListener(() => { logComp.OpenWindow(); });
            settingBtn.onClick.AddListener(() => { settingComp.OpenWindow(); });
            exitBtn.onClick.AddListener(() => { exitWin.OpenWindow(); });
            exitYesBtn.onClick.AddListener(ExitGame);
        }
        private void Update()
        {
            if (!isInGame)
                return;

            // Inventory 단축키
            if (Input.GetKeyDown(KeyCode.E))
                invenComp.OpenWindow();
            // Save 단축키
            else if (Input.GetKeyDown(KeyCode.S))
                saveComp.OpenWindow();
            // Log 단축키
            else if (Input.GetKeyDown(KeyCode.Q))
                logComp.OpenWindow();
            // Settings 단축키
            else if (Input.GetKeyDown(KeyCode.R))
                settingComp.OpenWindow();
        }
        #endregion

        #region Methods
        public void ExitGame()
        {
            SceneManager.LoadScene(0);
            // 타이틀 씬으로 돌아가게 구현
        }
        #endregion
    }
}