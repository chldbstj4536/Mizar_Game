using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class MoveScriptEvent : BaseScriptEvent
    {
        [SerializeField, LabelText("�̵��ϰ��� �ϴ� ��ũ��Ʈ ��ȣ"), Min(0)]
        private int nextIndex;

        public override void OnEnter()
        {
            base.OnEnter();

            gm.scriptData.SetScript(nextIndex);
        }
        protected override void OnUpdate() { }
    }
}
