using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class ChangeBackground : BaseScriptEvent
    {
        [SerializeField, LabelText("배경 선택"), Tooltip("배경에 대한 프리팹을 정해줍니다.")]
        private GameObject bgPrefab;

        public override void OnEnter()
        {
            base.OnEnter();

            gm.ChangeBackground(Object.Instantiate(bgPrefab));

            gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        protected override void OnUpdate() { }
    }
}
