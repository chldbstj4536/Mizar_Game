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
        [BoxGroup("책")]
        [LabelText("책 컴포넌트")]
        public Book bookComp;
        private AutoFlip bookFlipper;
        [LabelText("책 이미지")]
        public Sprite[] bookImgs;
        [BoxGroup("패널", true, true)]
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
        [LabelText("챕터 이미지 컴포넌트")]
        public Image chapterImage;
        [BoxGroup("불러오기")]
        [LabelText("챕터 이미지들")]
        public Sprite[] chapterSprites;
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

            // 세이브 파일 불러오기
            for (int i = 0; i < loadGameBtns.Length; ++i)
                loadGameBtns[i].SetLoadButton(i);

            bookFlipper = bookComp.GetComponent<AutoFlip>();
        }
        private void Start()
        {
            newGameBtnInMain.onClick.AddListener(() => { SaveDataManager.Instance.StartGame(); });
            loadGameBtnInMain.onClick.AddListener(() =>
            {
                menuPanel.SetActive(false);
                bookComp.bookPages[3] = bookImgs[0];
                bookComp.bookPages[4] = bookImgs[1];
                bookFlipper.FlipRightPage();
                bookComp.OnPageRelease = () =>
                {
                    loadPanel.SetActive(true);
                };
            });
            albumBtnInMain.onClick.AddListener(() =>
            {
                menuPanel.SetActive(false);
                bookComp.bookPages[3] = bookImgs[2];
                bookComp.bookPages[4] = bookImgs[3];
                bookFlipper.FlipRightPage();
                bookComp.OnPageRelease = () =>
                {
                    albumPanel.SetActive(true);
                };
            });
            gameExitBtnInMain.onClick.AddListener(() =>
            {
                menuPanel.SetActive(false);
                bookFlipper.FlipLeftPage();
                bookComp.OnPageRelease = () =>
                {
                    bookComp.GetComponent<Animator>().SetBool("bStart", false);
                };
            });
            exitLoadBtn.onClick.AddListener(() =>
            {
                loadPanel.SetActive(false);
                bookFlipper.FlipLeftPage();
                bookComp.OnPageRelease = () =>
                {
                    menuPanel.SetActive(true);
                };
            });
            exitAlbumBtn.onClick.AddListener(() =>
            {
                albumPanel.SetActive(false);
                bookFlipper.FlipLeftPage();
                bookComp.OnPageRelease = () =>
                {
                    menuPanel.SetActive(true);
                };
            });
            settingBtnInMain.onClick.AddListener(() => { settingComp.OpenWindow(); });

            chapterImage.sprite = chapterSprites[SaveDataManager.Instance.UnlockChapter];
        }
        private void Update()
        {
            if (Input.anyKeyDown && bookComp.currentPage == 0)
            {
                bookComp.GetComponent<Animator>().SetBool("bStart", true);
                bookComp.OnPageRelease = () =>
                {
                    menuPanel.SetActive(true);
                    bookComp.bookPages[0] = bookImgs[4];
                };
            }
        }
        #endregion

        #region Methods
        #endregion
    }
}