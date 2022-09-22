using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class ChangeBackground : BaseScriptEvent
    {
        [ValueDropdown("@BackgroundDataSO.Names")]
        [SerializeField, LabelText("��� ����"), Tooltip("��濡 ���� �������� �����ݴϴ�.")]
        private string bgName;

        public override void OnEnter()
        {
            base.OnEnter();
            BackgroundComponent.Instance.SetBackground(BackgroundDataSO.Instance[bgName]);

            gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        protected override void OnUpdate() { }
    }
}