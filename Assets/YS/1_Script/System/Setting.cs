using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace YS
{
    public enum TYPING_SPEED
    {
        SLOW,
        NORMAL,
        FAST
    }
    public class Setting
    {
        private const string AesKey = "Yqmgp21ySH+WX6BdQCbuG1Weu0IXmBhM9gQsK7jxQoY=";
        private const string AesIV = "IFw2csnrFl4ZmXe/XihYrA==";

        public static ConfigData CurrentConfigData => new ConfigData() { bgmVol = BGMAudioVolume, fxVol = FXAudioVolume, typingSpeed = TypingSpeed };
        public static float BGMAudioVolume
        {
            get => AudioManager.BaseBGMVolume;
            set => AudioManager.BaseBGMVolume = value;
        }
        public static float FXAudioVolume
        {
            get => AudioManager.BaseFXVolume;
            set => AudioManager.BaseFXVolume = value;
        }
        public static TYPING_SPEED TypingSpeed
        {
            get
            {
                if (CustomTMPEffect.TypingSpeed == 0.1f)
                    return TYPING_SPEED.SLOW;
                else if (CustomTMPEffect.TypingSpeed == 0.05f)
                    return TYPING_SPEED.NORMAL;
                else
                    return TYPING_SPEED.FAST;
            }
            set
            {
                switch (value)
                {
                    case TYPING_SPEED.SLOW:
                        CustomTMPEffect.TypingSpeed = 0.1f;
                        break;
                    case TYPING_SPEED.NORMAL:
                        CustomTMPEffect.TypingSpeed = 0.05f;
                        break;
                    case TYPING_SPEED.FAST:
                        CustomTMPEffect.TypingSpeed = 0.025f;
                        break;
                }
            }
        }
        /// <summary>
        /// 설정에서 텍스트 미리보기 기능
        /// </summary>
        public static IEnumerator TextPreview(CustomTMPEffect previewTMP)
        {
            WaitForSeconds wf100ms = CachedWaitForSeconds.Get(0.1f);
            WaitForSeconds wf1s = CachedWaitForSeconds.Get(1.0f);

            while (true)
            {
                yield return wf100ms;

                if (previewTMP.IsDoneTyping)
                {
                    yield return wf1s;
                    previewTMP.SetText("안녕하세요. 텍스트 타이핑 속도 미리보기입니다.\n<link=v_wave>물결치는 글자</link>와 <link=shake>흔들리는 글자</link>입니다.");
                }
            }
        }
        public static void SetSetting(ConfigData setting)
        {
            BGMAudioVolume = setting.bgmVol;
            FXAudioVolume = setting.fxVol;
            TypingSpeed = setting.typingSpeed;
        }
        public static void SaveSetting()
        {
            try
            {
                if (!Directory.Exists(Path.AppDataFolder))
                    Directory.CreateDirectory(Path.AppDataFolder);
                if (!File.Exists(Path.ConfigData))
                    File.Create(Path.ConfigData).Close();

                AES256CBC aes = new AES256CBC(AesKey, AesIV);

                StreamWriter sw = new StreamWriter(Path.ConfigData, false);
                sw.WriteLine(aes.Encrypt(JsonUtility.ToJson(CurrentConfigData)));
                sw.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        public static void LoadSetting()
        {
            try
            {
                if (!Directory.Exists(Path.AppDataFolder))
                    Directory.CreateDirectory(Path.AppDataFolder);
                if (!File.Exists(Path.ConfigData))
                    SaveSetting();

                StreamReader sr = new StreamReader(Path.ConfigData);
                string line = sr.ReadLine();
                Debug.Log(line);
                var aes = new AES256CBC(AesKey, AesIV);
                line = aes.Decrypt(line);
                Debug.Log(line);

                SetSetting(JsonUtility.FromJson<ConfigData>(line));
                sr.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    [Serializable]
    public struct ConfigData
    {
        // 볼륨 설정 정보
        public float bgmVol;
        public float fxVol;
        // 타이핑 속도 정보
        public TYPING_SPEED typingSpeed;
    }
}