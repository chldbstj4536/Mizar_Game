using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEditor;
using System.Threading;

namespace YS
{
    public class AlbumComponent : MonoBehaviour
    {
        [LabelText("���� ������ ��ư")]
        public Button prevPageBtn;
        [LabelText("���� ������ ��ư")]
        public Button nextPageBtn;
        [BoxGroup("�ٹ� ������", true, true)]
        [LabelText("�ٹ� ������ �г�")]
        public GameObject albumWindowPanel;
        [BoxGroup("�ٹ� ������")]
        [LabelText("�ٹ� ������ �̹���")]
        public Image awImg;
        [BoxGroup("�ٹ� ������")]
        [LabelText("�ٹ� ������ �ݱ� ��ư")]
        public Button awExitBtn;
        [BoxGroup("�ٹ� ������")]
        [LabelText("�ٹ� ������ �ٹ� �̸� TMP")]
        public TMP_Text awNameTmp;

        private Sprite[] imgs;
        private Button[] btns;
        private int curPage = 0;
        private int maxPage;
        private int maxAlbumCount;

        private void Start()
        {
            maxAlbumCount = transform.childCount;
            imgs = new Sprite[maxAlbumCount];
            btns = new Button[maxAlbumCount];
            maxPage = (maxAlbumCount / 4) + ((maxAlbumCount % 4) == 0 ? 0 : 1);

            for (int i = 0; i < maxAlbumCount; ++i)
            {
                RectTransform child = transform.GetChild(i) as RectTransform;
                imgs[i] = child.GetComponent<Image>().sprite;
                btns[i] = child.GetComponent<Button>();
                string name = child.GetChild(0).GetComponent<TMP_Text>().text;

                child.anchorMax = child.anchorMin = child.pivot = new Vector2((i % 4) < 2 ? 0 : 1, (i % 4) % 2 == 0 ? 1 : 0);
                child.anchoredPosition = Vector3.zero;

                int index = i;
                btns[i].onClick.AddListener(() =>
                {
                    albumWindowPanel.SetActive(true);
                    awImg.sprite = imgs[index];
                    awNameTmp.text = name;
                });
            }
            awExitBtn.onClick.AddListener(() => { albumWindowPanel.SetActive(false); });

            for (int i = 0; i < 4; ++i)
                transform.GetChild(i).gameObject.SetActive(true);

            prevPageBtn.onClick.AddListener(() => { PreviousPage(); });
            nextPageBtn.onClick.AddListener(() => { NextPage(); });
            prevPageBtn.gameObject.SetActive(false);
        }
        private void NextPage()
        {
            ++curPage;
            prevPageBtn.gameObject.SetActive(true);
            if (curPage + 1 == maxPage)
                nextPageBtn.gameObject.SetActive(false);

            for (int i = 0; i < 4 && i + curPage * 4 < maxAlbumCount; ++i)
            {
                transform.GetChild(i + (curPage - 1) * 4).gameObject.SetActive(false);
                transform.GetChild(i + curPage * 4).gameObject.SetActive(true);
            }
        }
        private void PreviousPage()
        {
            --curPage;
            nextPageBtn.gameObject.SetActive(true);
            if (curPage == 0)
                prevPageBtn.gameObject.SetActive(false);

            for (int i = 0; i < 4 && i + (curPage + 1) * 4 < maxAlbumCount; ++i)
            {
                transform.GetChild(i + (curPage + 1) * 4).gameObject.SetActive(false);
                transform.GetChild(i + curPage * 4).gameObject.SetActive(true);
            }
        }
    }
}
