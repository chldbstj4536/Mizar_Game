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
        [BoxGroup("å")]
        [LabelText("å ������Ʈ")]
        public Book bookComp;
        private AutoFlip bookFlipper;
        [LabelText("å �̹���")]
        public Sprite[] bookImgs;
        [BoxGroup("�г�", true, true)]
        [LabelText("�޴� �г�")]
        public GameObject menuPanel;
        [BoxGroup("�г�")]
        [LabelText("�ε� �г�")]
        public GameObject loadPanel;
        [BoxGroup("�г�")]
        [LabelText("�����ٹ� �г�")]
        public GameObject albumPanel;
        [BoxGroup("�г�")]
        [LabelText("���� ������Ʈ")]
        public SettingComponent settingComp;

        [BoxGroup("���� �޴�", true, true)]
        [LabelText("�� ���� ��ư")]
        public Button newGameBtnInMain;
        [BoxGroup("���� �޴�")]
        [LabelText("�̾��ϱ� ��ư")]
        public Button loadGameBtnInMain;
        [BoxGroup("���� �޴�")]
        [LabelText("�����ٹ� ��ư")]
        public Button albumBtnInMain;
        [BoxGroup("���� �޴�")]
        [LabelText("ȯ�漳�� ��ư")]
        public Button settingBtnInMain;
        [BoxGroup("���� �޴�")]
        [LabelText("�������� ��ư")]
        public Button gameExitBtnInMain;

        [BoxGroup("�ҷ�����", true, true)]
        [LabelText("�̾��ϱ� ��ư")]
        public LoadButton[] loadGameBtns;
        [BoxGroup("�ҷ�����")]
        [LabelText("é�� �̹��� ������Ʈ")]
        public Image chapterImage;
        [BoxGroup("�ҷ�����")]
        [LabelText("é�� �̹�����")]
        public Sprite[] chapterSprites;
        [BoxGroup("�ҷ�����")]
        [LabelText("�ε��г� ������ ��ư")]
        public Button exitLoadBtn;

        [BoxGroup("�����ٹ�", true, true)]
        [LabelText("������")]
        public Button[] albumBtns;
        [BoxGroup("�����ٹ�")]
        [LabelText("�ٹ� ������ ��ư")]
        public Button exitAlbumBtn;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            // ���� �ҷ�����
            Setting.LoadSetting();

            // ���̺� ���� �ҷ�����
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