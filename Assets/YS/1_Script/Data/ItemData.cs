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
        [LabelText("������ ����")]
        public string choiceStr;
        [LabelText("�������� ���� ���")]
        public string resultStr;
    }
    [System.Serializable, DisableContextMenu]
    public struct ItemInfo
    {
        [HideLabel, DisableIf("@true")]
        public ITEM_INDEX index;
        [Space(5.0f)]
        [LabelText("�̹���")]
        public Sprite img;
        [LabelText("�̸�")]
        public string name;
        [LabelText("����"), TextArea]
        public string desc;
        [LabelText("������ ����")]
        public InferenceChoiceInfo[] choicesInfo;
        [LabelText("�ùٸ� ������")]
        public uint correctIndex;
    }
}