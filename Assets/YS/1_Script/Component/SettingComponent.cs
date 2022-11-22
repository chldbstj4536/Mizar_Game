using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public class SettingComponent : MonoBehaviour
    {
        [LabelText("배경 볼륨 슬라이더")]
        public Slider bgmVolSlider;
        [LabelText("효과음 볼륨 슬라이더")]
        public Slider fxVolSlider;
        [LabelText("타이핑 속도 슬라이더")]
        public Slider typingSlider;
        [LabelText("타이핑 미리보기")]
        public CustomTMPEffect previewTMP;
        [LabelText("설정 저장 버튼")]
        public ButtonHighlighter saveChangedBtn;
        [LabelText("설정 닫기 버튼")]
        public Button closeSettingBtn;
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

        public void ShowWindow()
        {
            gameObject.SetActive(true);
            coroutineTextPreview = StartCoroutine(Setting.TextPreview(previewTMP));
            lastConfigData = Setting.CurrentConfigData;
            saveChangedBtn.interactable = false;
        }
        public void HideWindow()
        {
            StopCoroutine(coroutineTextPreview);
            previewTMP.SkipTyping();
            gameObject.SetActive(false);
            OnHideWindowEvent?.Invoke();
        }
        private void Start()
        {
            bgmVolSlider.value = AudioManager.BaseBGMVolume;
            fxVolSlider.value = AudioManager.BaseFXVolume;
            typingSlider.value = (float)Setting.TypingSpeed;

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
            closeSettingBtn.onClick.AddListener(() =>
            {
                if (saveChangedBtn.interactable)
                    configWarningPanel.SetActive(true);
                else
                    HideWindow();
            });
            configWarningNoBtn.onClick.AddListener(() =>
            {
                Setting.SetSetting(lastConfigData);
                bgmVolSlider.value = AudioManager.BaseBGMVolume;
                fxVolSlider.value = AudioManager.BaseFXVolume;
                typingSlider.value = (float)Setting.TypingSpeed;
                configWarningPanel.SetActive(false);
                HideWindow();
            });
            configWarningYesBtn.onClick.AddListener(() =>
            {
                Setting.SaveSetting();
                saveChangedBtn.interactable = false;

                configWarningPanel.SetActive(false);
                HideWindow();
            });
        }
    }
}