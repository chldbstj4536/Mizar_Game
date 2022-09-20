using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class PlayBGM : BaseScriptEvent
    {
        [SerializeField, LabelText("배경음악"), Tooltip("배경음악 선택\nnull일 시 배경음악 재생을 멈춥니다.")]
        private AudioClip audioBGM;
        [SerializeField, LabelText("볼륨"), Range(0.0f, 1.0f)]
        private float volume = 1.0f;
        [SerializeField, LabelText("루프")]
        private bool isLoop = true;
        [SerializeField, LabelText("재생시간"), SuffixLabel("s"), HideIf("@isLoop")]
        private float playTime;
        [SerializeField, LabelText("감쇄시간"), SuffixLabel("s"), HideIf("@isLoop")]
        private float dampingTime;

        public override void OnEnter()
        {
            base.OnEnter();

            if (audioBGM != null)
                AudioManager.PlayBGM(audioBGM, isLoop, playTime, dampingTime, volume);
            else
                AudioManager.StopBGM();

            gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        protected override void OnUpdate() { }
    }
}
