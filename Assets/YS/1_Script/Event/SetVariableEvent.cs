using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class SetVariableEvent : BaseScriptEvent
    {
        [SerializeField, LabelText("변경할 변수들")]
        private ChangeVariableInTable[] changeVars = new ChangeVariableInTable[0];

        public override void OnEnter()
        {
            base.OnEnter();

            var table = gm.VariablesTable;

            foreach (var changeVar in changeVars)
                changeVar.Calculate();

            gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        protected override void OnUpdate() { }
    }
}