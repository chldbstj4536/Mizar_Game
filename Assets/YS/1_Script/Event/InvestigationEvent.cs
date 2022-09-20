using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    public class InvestigationEvent : BaseScriptEvent
    {
        [SerializeField]
        [LabelText("���縦 ������ ĳ����")]
        private Sprite character;

        public override void OnEnter()
        {
            base.OnEnter();

            gm.ivSystem.Setup(character);
        }
        protected override void OnUpdate()
        {
            gm.ivSystem.OnUpdate();
        }
        public override void OnExit()
        {
            gm.ivSystem.Release();

            base.OnExit();
        }
    }
    [System.Serializable]
    public class InvestigationSystem
    {
        #region Fields
        [FoldoutGroup("���� UI", false)]
        [LabelText("���� �г� UI"), Tooltip("���� �г� ��Ʈ ���ӿ�����Ʈ")]
        public GameObject investigationPanel;
        [FoldoutGroup("���� UI")]
        [LabelText("�����ϴ� ĳ���� UI"), Tooltip("�»�ܿ� �ִ� ĳ���� �̹��� ������Ʈ")]
        public Image investigationCharacter;
        [FoldoutGroup("���� UI")]
        [LabelText("���� �Ϸ� ȿ�� �ִϸ�����"), Tooltip("���� �Ϸ� �� ����� �ִϸ�����")]
        public Animator findAllItemFXAnimator;
        [FoldoutGroup("���� UI")]
        [LabelText("�»�� ���̾�α�"), Tooltip("���縦 �Ϸ����� �ʰų� �ϴ� �� �»�� ĳ������ ��ȭ������ ǥ���ϴ� ���̾�α�")]
        public CustomTMPEffect investigationDialogTMP;
        [FoldoutGroup("���� UI/������ ȹ�� â UI", false)]
        [LabelText("������ ȹ�� �ִϸ�����"), Tooltip("������ ȹ�� â�� ���� �ִϸ�����")]
        public Animator getItemAnimator;
        [FoldoutGroup("���� UI/������ ȹ�� â UI")]
        [LabelText("ȹ�� ������ �̹��� UI"), Tooltip("������ ȹ�� â������ �̹��� ������Ʈ")]
        public Image getItemUI_ItemImg;
        [FoldoutGroup("���� UI/������ ȹ�� â UI")]
        [LabelText("ȹ�� ������ �̸� UI"), Tooltip("������ ȹ�� â������ �̸� TMP")]
        public TMP_Text getItemUI_ItemName;
        [FoldoutGroup("���� UI/������ ȹ�� â UI")]
        [LabelText("ȹ�� ������ ���� UI"), Tooltip("������ ȹ�� â������ ���� TMP")]
        public TMP_Text getItemUI_ItemDesc;

        private GameObject investigationDialog;
        private Button clearBtn;
        /// <summary>
        /// ���� ������ ����
        /// </summary>
        private int findCount;
        private GameManager gm;
        #endregion

        #region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;
            investigationCharacter.GetComponent<Button>().onClick.AddListener(() => { GameManager.Instance.ivSystem.OnClearBtnDown(); });
            investigationDialog = investigationDialogTMP.transform.parent.gameObject;
        }

        /// <summary>
        /// ���� ��� ���� �Լ�
        /// </summary>
        /// <param name="ivChar">���縦 ������ ĳ���� �ε���</param>
        /// <param name="nextIndex">���簡 ���� �� �̵��� ���� ��ũ��Ʈ �ε���</param>
        public void Setup(Sprite ivChar)
        {
            investigationCharacter.sprite = ivChar;

            investigationPanel.SetActive(true);

            var items = new Item[gm.ItemCount];
            for (int i = 0; i < items.Length; ++i)
            {
                items[i] = gm.GetItem(i);
                items[i].imageComp.raycastTarget = true;
            }

            findCount = items.Length;
            if (findCount == 0)
                findAllItemFXAnimator.SetBool("IsFindAllItem", true);
        }
        /// <summary>
        /// ���� ��� ���� �Լ�
        /// </summary>
        public void Release()
        {
            investigationPanel.SetActive(false);
        }
        public void OnUpdate()
        {
            if (gm.IsKeyDown())
            {
                // ȭ�鿡 ������ ȹ�� â�� ���ִ°�
                if (getItemAnimator.gameObject.activeInHierarchy)
                {
                    // ������ ȹ�� â �ִϸ��̼��� �Ϸ�� �����ΰ�
                    if (getItemAnimator.GetCurrentAnimatorStateInfo(0).IsName("Complete"))
                    {
                        // �Ϸ�� ���¸� ������ ȹ�� â ��Ȱ��ȭ �� ���� �ʱ�ȭ
                        getItemAnimator.gameObject.SetActive(false);
                        getItemAnimator.SetBool("Skip", false);
                    }
                    else
                        // �̿Ϸ�� ���¸� ��ŵ
                        getItemAnimator.SetBool("Skip", true);
                }
            }
        }
        public void OnFindItem(Item item)
        {
            item.gameObject.SetActive(false);
            --findCount;
            gm.invenComp.AddItem(item.index);

            getItemAnimator.gameObject.SetActive(true);
            getItemUI_ItemImg.sprite = item.ItemImage;
            getItemUI_ItemName.text = item.Name;
            getItemUI_ItemDesc.text = item.Desc;

            if (findCount == 0)
                findAllItemFXAnimator.SetBool("IsFindAllItem", true);
        }
        private void OnClearBtnDown()
        {
            if (findCount == 0)
                gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
            else
            {
                investigationDialog.SetActive(true);
                gm.CancelInvoke(nameof(HideInferenceDialogTMP));
                investigationDialogTMP.SetText("���� ���簡 ������ �ʾҾ�");
                gm.Invoke(nameof(HideInferenceDialogTMP), 3.0f);
            }
        }
        private void HideInferenceDialogTMP()
        {
            investigationDialog.SetActive(false);
        }
        #endregion
    }
}