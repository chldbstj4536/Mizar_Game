using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "ItemData", menuName = ("AddData/ItemData"))]
    public class ItemData : ScriptableObject
    {
        [SerializeField, Searchable, ListDrawerSettings(NumberOfItemsPerPage = 1, ShowPaging = true, HideAddButton = true, HideRemoveButton = true, DraggableItems = false), OnInspectorInit("@SetItemDataList()")]
        private List<ItemInfo> items;

        public ItemInfo this[ITEM_INDEX index]
        {
            get { return items[(int)index]; }
        }
        private void SetItemDataList()
        {
            if (items == null)
                items = new List<ItemInfo>();

            if (items.Count < (int)ITEM_INDEX.MAX)
                for (int i = items.Count; i < (int)ITEM_INDEX.MAX; ++i)
                    items.Add(new ItemInfo() { index = (ITEM_INDEX)i });
            else
                for (int i = items.Count; i > (int)ITEM_INDEX.MAX; ++i)
                    items.RemoveAt(i);
        }
    }
    [System.Serializable]
    public struct InferenceChoiceInfo
    {
        [LabelText("선택지 내용")]
        public string choiceStr;
        [LabelText("선택지에 대한 대답")]
        public string resultStr;
    }
    [System.Serializable, DisableContextMenu]
    public struct ItemInfo
    {
        [HideLabel, DisableIf("@true")]
        public ITEM_INDEX index;
        [Space(5.0f)]
        [LabelText("이미지")]
        public Sprite img;
        [LabelText("이름")]
        public string name;
        [LabelText("설명"), TextArea]
        public string desc;
        [LabelText("선택지 정보")]
        public InferenceChoiceInfo[] choicesInfo;
        [LabelText("올바른 선택지")]
        public uint correctIndex;
    }
}