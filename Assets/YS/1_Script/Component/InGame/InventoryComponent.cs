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
        /// 아이템을 인벤토리에 추가합니다.
        /// </summary>
        /// <param name="itemIndex">추가할 아이템 인덱스 번호</param>
        public void AddItem(ITEM_INDEX itemIndex)
        {
            inven.Add(itemIndex);
            Item item = Instantiate(invenItemPrefab, rootItemTr).GetComponent<Item>();
            item.index = itemIndex;
            item.imageComp.sprite = GameManager.Instance.itemData[itemIndex].img;
        }
        /// <summary>
        /// 선택된 아이템의 정보를 인벤토리의 아이템 정보로 보여줍니다.
        /// </summary>
        /// <param name="itemIndex">선택된 아이템 인덱스 번호</param>
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
        [LabelText("쥐")]
        RAT,
        [LabelText("액체가 일부 남아있는 병")]
        EMPTY_BOTTLE,
        [LabelText("모자")]
        HAT,
        [LabelText("하얀 가루가 묻은 검은 천 조각")]
        BLACK_CLOTH,
        [LabelText("부서진 회중시계")]
        CLOCK,
        [LabelText("불에 탄 서류 뭉치")]
        BURN_DOCS,
        [LabelText("하얀가루")]
        DRUG,
        [LabelText("진흙과 솔잎")]
        ROADOTCRIME,
        MAX
    }
}