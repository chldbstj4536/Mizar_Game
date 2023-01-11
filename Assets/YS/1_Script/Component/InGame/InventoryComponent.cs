using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    public class InventoryComponent : WindowComponent
    {
        public Transform rootItemTr;
        public Image invenItemImg;
        public TMP_Text invenItemName;
        public TMP_Text invenItemDesc;
        public GameObject invenItemPrefab;

        private List<ITEM_INDEX> inven = new List<ITEM_INDEX>();

        public List<ITEM_INDEX> Items => inven;
        public override void OpenWindow()
        {
            base.OpenWindow();

            invenItemImg.sprite = null;
            invenItemName.text = "";
            invenItemDesc.text = "";
        }

        /// <summary>
        /// �������� �κ��丮�� �߰��մϴ�.
        /// </summary>
        /// <param name="itemIndex">�߰��� ������ �ε��� ��ȣ</param>
        public void AddItem(ITEM_INDEX itemIndex)
        {
            inven.Add(itemIndex);
            Item item = Instantiate(invenItemPrefab, rootItemTr).GetComponent<Item>();
            item.index = itemIndex;
            item.imageComp.sprite = GameManager.Instance.itemData[itemIndex].img;
        }
        /// <summary>
        /// ���õ� �������� ������ �κ��丮�� ������ ������ �����ݴϴ�.
        /// </summary>
        /// <param name="itemIndex">���õ� ������ �ε��� ��ȣ</param>
        public void SetItemInfo(ITEM_INDEX itemIndex)
        {
            var itemData = GameManager.Instance.itemData;

            invenItemImg.sprite = itemData[itemIndex].img;
            invenItemName.text = itemData[itemIndex].name;
            invenItemDesc.text = itemData[itemIndex].desc;
        }
    }
    public enum ITEM_INDEX
    {
        [LabelText("��")]
        RAT,
        [LabelText("��ü�� �Ϻ� �����ִ� ��")]
        EMPTY_BOTTLE,
        [LabelText("����")]
        HAT,
        [LabelText("�Ͼ� ���簡 ���� ���� õ ����")]
        BLACK_CLOTH,
        [LabelText("�μ��� ȸ�߽ð�")]
        CLOCK,
        [LabelText("�ҿ� ź ���� ��ġ")]
        BURN_DOCS,
        [LabelText("�Ͼᰡ��")]
        DRUG,
        [LabelText("����� ����")]
        ROADOTCRIME,
        MAX
    }
}