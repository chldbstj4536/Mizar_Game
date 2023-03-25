using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEditorInternal;

namespace YS
{
    public class CompareEvent : BaseScriptEvent
    {
        #region Fields
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true), DisableContextMenu]
        [LabelText("���� ���� ������")]
        public CompareData[] cpDatas = new CompareData[4];
        [LabelText("���� ���� ������")]
        public CompareData answerData;
        #endregion

        #region Properties
        #endregion

        public override void OnEnter()
        {
            base.OnEnter();

            gm.cpSystem.Setup(this);
        }
        protected override void OnUpdate()
        {
            gm.cpSystem.OnUpdate();
        }
        public override void OnExit()
        {
            gm.cpSystem.Release();

            base.OnExit();
        }
    }
    [System.Serializable]
    public class CompareData
    {
        [LabelText("���� �̸�")]
        public string name;
        [LabelText("���� �̹���")]
        public Sprite fpImg;
    }
    [System.Serializable]
    public class CompareSystem
    {
        #region Fields
        [FoldoutGroup("���� UI", false)]
        [LabelText("���� �г� UI")]
        public GameObject rootObj;
        [FoldoutGroup("���� UI")]
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true), DisableContextMenu]
        [LabelText("���� ���� �̹���")]
        public Image[] pfImgs = new Image[4];
        [FoldoutGroup("���� UI")]
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true), DisableContextMenu]
        [LabelText("���� ���� TMP")]
        public TMP_Text[] pfTMPs = new TMP_Text[4];
        [FoldoutGroup("���� UI")]
        [LabelText("���� ���� �̹���")]
        public Image answerPfImg;
        [FoldoutGroup("���� UI")]
        [LabelText("���� ���� TMP")]
        public TMP_Text answerPfTMP;

        private Button[] pfBtns = new Button[4];
        private Image[] pfBGs = new Image[4];
        private int selectPf = 0;
        private GameManager gm;
        #endregion

        #region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;
            for (int i = 0; i < 4; ++i)
            {
                int n = i;
                pfBtns[i] = pfImgs[i].GetComponent<Button>();
                pfBGs[i] = pfImgs[i].transform.parent.GetComponent<Image>();
                pfBtns[i].onClick.AddListener(() =>
                {
                    Debug.Log(n);
                    if (selectPf == n)
                    {
                        pfBGs[selectPf].color = Color.white;
                        Answer(n);
                        return;
                    }

                    pfBGs[selectPf].color = Color.white;
                    pfBGs[n].color = Color.yellow;
                    selectPf = n;
                });
            }
        }
        public void Setup(CompareEvent ce)
        {
            rootObj.SetActive(true);

            for (int i = 0; i < 4; ++i)
            {
                pfImgs[i].sprite = ce.cpDatas[i].fpImg;
                pfTMPs[i].text = ce.cpDatas[i].name;
            }
            answerPfImg.sprite = ce.answerData.fpImg;
            answerPfTMP.text = ce.answerData.name;
        }
        public void OnUpdate()
        {
        }
        public void Release()
        {
            rootObj.SetActive(false);
        }
        public void Answer(int n)
        {
            gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        #endregion
    }
}