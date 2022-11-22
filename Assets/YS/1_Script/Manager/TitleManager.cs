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
        [BoxGroup("�г�", true, true)]
        [LabelText("��ġ���� �г�")]
        public GameObject clickToStartPanel;
        [BoxGroup("�г�")]
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
        [LabelText("é�� �ε� ��ư")]
        public ChapterHighlighter[] loadChapterBtns;
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
            SaveDataManager.Instance.LoadData();

            // ���̺� ���� �ҷ�����
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