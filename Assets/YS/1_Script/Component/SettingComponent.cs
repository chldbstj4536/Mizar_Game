using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public class SettingComponent : WindowComponent 
    {
        [LabelText("배경 볼륨 슬라이더")]
        public Slider bgmVolSlider;
        [LabelText("효과음 볼륨 슬라이더")]
        public CustomSlider fxVolSlider;
        [LabelText("효과음 볼륨 테스트 음악")]
        public AudioClip fxTestClip;
        [LabelText("타이핑 속도 슬라이더")]
        public Slider typingSlider;
        [LabelText("타이핑 미리보기")]
        public CustomTMPEffect previewTMP;
        [LabelText("설정 저장 버튼")]
        public ButtonHighlighter saveChangedBtn;
        [BoxGroup("경고", true, true)]
        [LabelText("설정 경고 UI")]
        public GameObject configWarningPanel;
        [BoxGroup("경고")]
        [LabelText("설정 경고 승낙 버튼")]
        public Button configWarningYesBtn;
        [BoxGroup("경고")]
        [LabelText("설정 경고 거절 버튼")]
        public Button configWarningNoBtn;

        private Coroutine coroutineTextPreview;
        private ConfigData lastConfigData;

        public delegate void OnHideWindow();
        public event OnHideWindow OnHideWindowEvent;

        protected override void Start()
        {
            base.Start();

            bgmVolSlider.value = AudioManager.BaseBGMVolume;
            fxVolSlider.value = AudioManager.BaseFXVolume;
            typingSlider.value = (float)Setting.TypingSpeed;
            lastConfigData = Setting.CurrentConfigData;

            bgmVolSlider.onValueChanged.AddListener((float value) =>
            {
                AudioManager.BaseBGMVolume = value;
                saveChangedBtn.interactable = true;
            });
            fxVolSlider.onValueChanged.AddListener((float value) =>
            {
                AudioManager.BaseFXVolume = value;
                saveChangedBtn.interactable = true;
            });
            fxVolSlider.onPointerUpEvent += () => { AudioManager.PlayFX(fxTestClip); };
            typingSlider.onValueChanged.AddListener((float value) =>
            {
                Setting.TypingSpeed = (TYPING_SPEED)value;
                saveChangedBtn.interactable = true;
            });
            saveChangedBtn.AddListener(() =>
            {
                Setting.SaveSetting();
                lastConfigData = Setting.CurrentConfigData;
                saveChangedBtn.interactable = false;
            });
            configWarningNoBtn.onClick.AddListener(() =>
            {
                Setting.SetSetting(lastConfigData);
                bgmVolSlider.value = AudioManager.BaseBGMVolume;
                fxVolSlider.value = AudioManager.BaseFXVolume;
                typingSlider.value = (float)Setting.TypingSpeed;

                saveChangedBtn.interactable = false;
                configWarningPanel.SetActive(false);
                CloseWindow();
            });
            configWarningYesBtn.onClick.AddListener(() =>
            {
                Setting.SaveSetting();
                saveChangedBtn.interactable = false;

                configWarningPanel.SetActive(false);
                CloseWindow();
            });
        }
        public override void OpenWindow()
        {
            base.OpenWindow();
            coroutineTextPreview = StartCoroutine(Setting.TextPreview(previewTMP));
            saveChangedBtn.interactable = false;
        }
        public override void CloseWindow()
        {
            if (!configWarningPanel.activeInHierarchy && saveChangedBtn.interactable)
                configWarningPanel.SetActive(true);
            else
            {
                StopCoroutine(coroutineTextPreview);
                previewTMP.SkipTyping();
                gameObject.SetActive(false);
                OnHideWindowEvent?.Invoke();
            }
        }
    }
}