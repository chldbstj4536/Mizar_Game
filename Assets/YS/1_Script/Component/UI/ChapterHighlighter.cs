using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YS
{
    public class ChapterHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Image bg;
        [SerializeField]
        private TMP_Text tmp;
        private bool isUnlock = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isUnlock)
                return;

            Highlight();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isUnlock)
                return;

            UnHighlight();
        }

        private void Highlight()
        {
            tmp.fontMaterial.SetFloat("_GlowOffset", 0.0f);
            tmp.color = bg.color = Color.white;
        }
        private void UnHighlight()
        {
            tmp.fontMaterial.SetFloat("_GlowOffset", -1.0f);
            tmp.color = bg.color = new Color(0.7f, 0.7f, 0.7f);
        }

        public void SetChapter(int chapter)
        {
            isUnlock = chapter <= SaveDataManager.Instance.UnlockChapter;
            if (isUnlock)
            {
                bg.material = ResourceManager.GetResource<Material>(ResourcePath.DefaultMtrl);
                UnHighlight();
            }
            else
            {
                bg.material = ResourceManager.GetResource<Material>(ResourcePath.MonoMtrl);
            }
        }
    }
}