using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class MoveScriptEvent : BaseScriptEvent
    {
        [SerializeField, LabelText("이동하고자 하는 스크립트 번호"), Min(0)]
        private int nextIndex;

        public override void OnEnter()
        {
            base.OnEnter();

            gm.scriptData.SetScript(nextIndex);
        }
        protected override void OnUpdate() { }
    }
}
