using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    public class InventoryComponent : MonoBehaviour
    {
        public Transform rootItemTr;
        public Image invenItemImg;
        public TMP_Text invenItemName;
        public TMP_Text invenItemDesc;
        public GameObject invenItemPrefab;

        private List<ITEM_INDEX> inven = new List<ITEM_INDEX>();

        public void OpenInventory()
        {
            gameObject.SetActive(true);
            invenItemImg.sprite = null;
            invenItemName.text = "";
            invenItemDesc.text = "";
        }
        public void CloseInventory()
        {
            gameObject.SetActive(false);
        }

        public void AddItem(ITEM_INDEX itemIndex)
        {
            inven.Add(itemIndex);
            Item item = Instantiate(invenItemPrefab, rootItemTr).GetComponent<Item>();
            item.index = itemIndex;
            item.imageComp.sprite = GameManager.Instance.itemData[itemIndex].img;
        }
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