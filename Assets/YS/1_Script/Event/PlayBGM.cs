using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class PlayBGM : BaseScriptEvent
    {
        [SerializeField, LabelText("�������"), Tooltip("������� ����\nnull�� �� ������� ����� ����ϴ�.")]
        private AudioClip audioBGM;
        [SerializeField, LabelText("����"), Range(0.0f, 1.0f)]
        private float volume = 1.0f;
        [SerializeField, LabelText("����")]
        private bool isLoop = true;
        [SerializeField, LabelText("����ð�"), SuffixLabel("s"), HideIf("@isLoop")]
        private float playTime;
        [SerializeField, LabelText("����ð�"), SuffixLabel("s"), HideIf("@isLoop")]
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
