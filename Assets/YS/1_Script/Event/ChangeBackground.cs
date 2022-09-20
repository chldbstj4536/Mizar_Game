using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class ChangeBackground : BaseScriptEvent
    {
        [SerializeField, LabelText("��� ����"), Tooltip("��濡 ���� �������� �����ݴϴ�.")]
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
