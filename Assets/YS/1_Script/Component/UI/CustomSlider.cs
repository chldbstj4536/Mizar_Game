using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YS
{
    public class CustomSlider : Slider
    {
        public delegate void OnEvent();
        public OnEvent onPointerUpEvent;
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            onPointerUpEvent();
        }
    }
}
