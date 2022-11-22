using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class AutoSaveEvent : BaseScriptEvent
    {
        public override void OnEnter()
        {
            base.OnEnter();

        }
        protected override void OnUpdate() { }
    }
}