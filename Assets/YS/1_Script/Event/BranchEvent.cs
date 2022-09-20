using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class BranchEvent : BaseScriptEvent
    {
        [InfoBox("모든 분기 조건이 틀릴 시 다음 스크립트 번호로 이동")]
        [LabelText("분기")]
        public BranchData[] branches = new BranchData[0];

        public override void OnEnter()
        {
            base.OnEnter();

            int nextIdx = gm.scriptData.CurrentIndex + 1;

            foreach (var branch in branches)
            {
                if (CompareVariableInTable.Compare(branch.cmps))
                {
                    nextIdx = branch.nextIdx;
                    break;
                }
            }

            gm.scriptData.SetScript(nextIdx);
        }
        protected override void OnUpdate() { }
    }
    [System.Serializable]
    public struct BranchData
    {
        [LabelText("분기 조건")]
        public CompareVariableInTable[] cmps;
        [LabelText("분기 조건 충족시 이동할 이벤트 번호"), Tooltip("분기 조건 충족시 이동할 이벤트 번호")]
        public int nextIdx;
    }
}