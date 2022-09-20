using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class PlayFXSound : BaseScriptEvent
    {
        [SerializeField, LabelText("ȿ����"), Tooltip("ȿ���� ����")]
        private AudioClip audioFX;
        [SerializeField, LabelText("����"), Range(0.0f, 1.0f)]
        private float volume = 1.0f;
        [SerializeField, LabelText("������"), Tooltip("�󸶸�ŭ�� �ð� �ڿ� �����ų��"), SuffixLabel("s"), Min(0.0f)]
        private float delay;

        public override void OnEnter()
        {
            base.OnEnter();

            if (audioFX != null)
                AudioManager.PlayFX(audioFX, volume, delay);

            gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        protected override void OnUpdate() { }
    }
}
