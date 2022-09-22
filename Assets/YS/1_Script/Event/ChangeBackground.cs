using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class ChangeBackground : BaseScriptEvent
    {
        [ValueDropdown("@BackgroundDataSO.Names")]
        [SerializeField, LabelText("배경 선택"), Tooltip("배경에 대한 프리팹을 정해줍니다.")]
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