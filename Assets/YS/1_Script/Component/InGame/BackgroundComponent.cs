using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public class BackgroundComponent : SingletonMono<BackgroundComponent>
    {
        private List<Item> items = new List<Item>();
        private string bgName;
        private Image imgComp;

        private GameObject itemTemplate;

        public List<Item> Items => items;
        public int RemainItemCount => items.Count;
        public string BackgroundName => bgName;
        public Sprite Image => imgComp.sprite;

        protected override void Awake()
        {
            base.Awake();

            imgComp = GetComponent<Image>();
            itemTemplate = ResourceManager.GetResource<GameObject>(ResourcePath.BGItemPrefab);
        }

        public void SetBackground(BackgroundData bgData)
        {
            bgName = bgData.name;
            imgComp.sprite = bgData.img;

            foreach (var item in items)
                GameObject.Destroy(item.gameObject);

            items.Clear();
            items.Capacity = bgData.items.Count;
            
            foreach (var itemData in bgData.items)
            {
                var item = Instantiate(itemTemplate, transform).GetComponent<Item>();
                item.index = itemData.index;
                item.transform.localPosition = itemData.pos;
                items.Add(item);
            }

            //newBGTr.SetParent(canvasTr, true);
            //newBGTr.SetAsFirstSibling();
            //newBGTr.anchoredPosition = Vector2.zero;
            //newBGTr.anchorMin = Vector2.zero;
            //newBGTr.anchorMax = Vector2.one;
            //newBGTr.sizeDelta = Vector2.zero;
            //newBGTr.localScale = Vector3.one;
        }
    }
}