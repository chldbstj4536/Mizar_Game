using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YS
{
    [RequireComponent(typeof(Image))]
    public class PuzzlePieceComponent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Image imgComp;
        private Vector3 correctPos;
        private Vector2 frameSize;

        public delegate void SuccessEvent();
        public event SuccessEvent OnSuccess;

        private void Awake()
        {
            imgComp = GetComponent<Image>();
        }

        public void InitializePiece(Sprite sprite, Vector3 correctPos, Vector2 frameSize)
        {
            imgComp.sprite = sprite;
            imgComp.SetNativeSize();

            this.correctPos = correctPos;
            this.frameSize = frameSize;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 newPos = transform.position = eventData.position;

            /*newPos.x = Mathf.Clamp(transform.localPosition.x, -((frameSize.x - imgComp.rectTransform.sizeDelta.x) / 2), (frameSize.x - imgComp.rectTransform.sizeDelta.x) / 2);
            newPos.y = Mathf.Clamp(transform.localPosition.y, -((frameSize.y - imgComp.rectTransform.sizeDelta.y) / 2), (frameSize.y - imgComp.rectTransform.sizeDelta.y) / 2);*/

            transform.localPosition = newPos;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (Vector3.Distance(transform.localPosition, correctPos) <= 10.0f)
            {
                transform.localPosition = correctPos;
                imgComp.raycastTarget = false;
                OnSuccess?.Invoke();
            }
        }
    }
}