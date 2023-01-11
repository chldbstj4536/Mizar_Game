using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace YS
{
    [RequireComponent(typeof(Image))]
    public class Item : MonoBehaviour, IPointerClickHandler
    {
        #region Fields
        public ITEM_INDEX index;
        public Image imageComp;

        private ItemData itemData;
        #endregion

        public Sprite ItemImage => itemData[index].img;

        public string Name => itemData[index].name;
        public string Desc => itemData[index].desc;
        public InferenceChoiceInfo[] ChoicesInfo => itemData[index].choicesInfo;
        public uint CorrectIndex => itemData[index].correctIndex;

        private void Start()
        {
            itemData = GameManager.Instance.itemData;
            imageComp.sprite = ItemImage;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (InGameUIManager.Instance.invenComp.gameObject.activeInHierarchy)
                GameManager.Instance.invenComp.SetItemInfo(index);
            else
                GameManager.Instance.ivSystem.OnFindItem(this);
        }
    }
}