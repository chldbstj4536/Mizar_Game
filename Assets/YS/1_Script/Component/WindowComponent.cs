using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public class WindowComponent : MonoBehaviour
    {
        public Button exitBtn;

        public delegate void WindowEvent();
        public WindowEvent OnOpenEvent;
        public WindowEvent OnCloseEvent;

        protected virtual void Start()
        {
            exitBtn.onClick.AddListener(() => { CloseWindow(); });
        }
        public virtual void OpenWindow()
        {
            OnOpenEvent?.Invoke();
            gameObject.SetActive(true);
        }
        public virtual void CloseWindow()
        {
            gameObject.SetActive(false);
            OnCloseEvent?.Invoke();
        }
    }
}
