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
    public class TitleManager : SingletonMono<TitleManager>
    {
        #region Field
        [BoxGroup("패널", true, true)]
        [LabelText("터치시작 패널")]
        public GameObject clickToStartPanel;
        [BoxGroup("패널")]
        [LabelText("메뉴 패널")]
        public GameObject menuPanel;
        [BoxGroup("패널")]
        [LabelText("로드 패널")]
        public GameObject loadPanel;
        [BoxGroup("패널")]
        [LabelText("사진앨범 패널")]
        public GameObject albumPanel;
        [BoxGroup("패널")]
        [LabelText("설정 컴포넌트")]
        public SettingComponent settingComp;

        [BoxGroup("메인 메뉴", true, true)]
        [LabelText("새 게임 버튼")]
        public Button newGameBtnInMain;
        [BoxGroup("메인 메뉴")]
        [LabelText("이어하기 버튼")]
        public Button loadGameBtnInMain;
        [BoxGroup("메인 메뉴")]
        [LabelText("사진앨범 버튼")]
        public Button albumBtnInMain;
        [BoxGroup("메인 메뉴")]
        [LabelText("환경설정 버튼")]
        public Button settingBtnInMain;
        [BoxGroup("메인 메뉴")]
        [LabelText("게임종료 버튼")]
        public Button gameExitBtnInMain;

        [BoxGroup("불러오기", true, true)]
        [LabelText("이어하기 버튼")]
        public LoadButton[] loadGameBtns;
        [BoxGroup("불러오기")]
        [LabelText("챕터 로드 버튼")]
        public ChapterHighlighter[] loadChapterBtns;
        [BoxGroup("불러오기")]
        [LabelText("로드패널 나가기 버튼")]
        public Button exitLoadBtn;

        [BoxGroup("사진앨범", true, true)]
        [LabelText("사진들")]
        public Button[] albumBtns;
        [BoxGroup("사진앨범")]
        [LabelText("앨범 나가기 버튼")]
        public Button exitAlbumBtn;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            // 설정 불러오기
            Setting.LoadSetting();
            SaveDataManager.Instance.LoadData();

            // 세이브 파일 불러오기
            for (int i = 0; i < loadGameBtns.Length; ++i)
                loadGameBtns[i].SetLoadButton(i);
        }
        private void Start()
        {
            newGameBtnInMain.onClick.AddListener(() => { SaveDataManager.Instance.StartGameWithChapter(1); });
            loadGameBtnInMain.onClick.AddListener(() => { menuPanel.SetActive(false); loadPanel.SetActive(true); });
            albumBtnInMain.onClick.AddListener(() => { menuPanel.SetActive(false); albumPanel.SetActive(true); });
            gameExitBtnInMain.onClick.AddListener(() => {  });
            exitLoadBtn.onClick.AddListener(() => { loadPanel.SetActive(false); menuPanel.SetActive(true); });
            exitAlbumBtn.onClick.AddListener(() => { albumPanel.SetActive(false); menuPanel.SetActive(true); });
            settingBtnInMain.onClick.AddListener(() => { settingComp.ShowWindow(); });
        }
        private void Update()
        {
            if (Input.anyKeyDown && clickToStartPanel.activeInHierarchy)
            {
                clickToStartPanel.SetActive(false);
                menuPanel.SetActive(true);
            }
        }
        #endregion

        #region Methods
        #endregion
    }
}