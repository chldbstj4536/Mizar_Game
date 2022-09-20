using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class BranchEvent : BaseScriptEvent
    {
        [InfoBox("��� �б� ������ Ʋ�� �� ���� ��ũ��Ʈ ��ȣ�� �̵�")]
        [LabelText("�б�")]
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
        [LabelText("�б� ����")]
        public CompareVariableInTable[] cmps;
        [LabelText("�б� ���� ������ �̵��� �̺�Ʈ ��ȣ"), Tooltip("�б� ���� ������ �̵��� �̺�Ʈ ��ȣ")]
        public int nextIdx;
    }
}