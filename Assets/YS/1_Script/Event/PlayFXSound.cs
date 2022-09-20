using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class PlayFXSound : BaseScriptEvent
    {
        [SerializeField, LabelText("효과음"), Tooltip("효과음 선택")]
        private AudioClip audioFX;
        [SerializeField, LabelText("볼륨"), Range(0.0f, 1.0f)]
        private float volume = 1.0f;
        [SerializeField, LabelText("딜레이"), Tooltip("얼마만큼의 시간 뒤에 재생시킬지"), SuffixLabel("s"), Min(0.0f)]
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
