using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace YS
{
    [RequireComponent(typeof(TMP_Text))]
    public class ButtonHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private TMP_Text text;
        private Button btn;
        private Image underline;

        public bool interactable
        {
            get { return btn.interactable; }
            set
            {
                if (interactable == value) return;
                if (!value) OnPointerExit(null);

                btn.interactable = value;
            }
        }

        private void OnDisable()
        {
            OnPointerExit(null);
        }

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            btn = GetComponent<Button>();
            underline = transform.GetChild(0).gameObject.GetComponent<Image>();

            OnPointerExit(null);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!btn.interactable) return;

            text.color = new Color(0.15f, 0.15f, 0.15f);
            underline.color = Color.white;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!btn.interactable) return;

            text.color = new Color(0.35f, 0.35f, 0.35f);
            underline.color = Color.clear;
        }
        public void AddListener(UnityEngine.Events.UnityAction call)
        {
            btn.onClick.AddListener(call);
        }
    }
}
