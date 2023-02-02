using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public class AudioManager : SingletonMono<AudioManager>
    {
        #region Field
        private AudioSource audioBGM;
        private AudioSource audioFX;

        private float audioBGMVolume;
        private float audioFXVolume;
        private float audioBGMTempVolume = 1.0f;
        private float audioFXTempVolume = 1.0f;
        private float audioBGMPlayTime;
        private float audioBGMDampingTime;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            audioBGM = transform.GetChild(0).GetComponent<AudioSource>();
            audioFX = transform.GetChild(1).GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        #region Properties
        public static float TotalBGMVolume => Instance.audioBGM.volume;
        public static float TotalFXVolume => Instance.audioFX.volume;
        public static float BaseBGMVolume
        {
            get => Instance.audioBGMVolume;
            set
            {
                Instance.audioBGMVolume = value;
                Instance.audioBGM.volume = value * TempBGMVolume;
            }
        }
        private static float TempBGMVolume
        {
            get => Instance.audioBGMTempVolume;
            set
            {
                Instance.audioBGMTempVolume = value;
                Instance.audioBGM.volume = value * BaseBGMVolume;
            }
        }
        public static float BaseFXVolume
        {
            get => Instance.audioFXVolume;
            set
            {
                Instance.audioFXVolume = value;
                Instance.audioFX.volume = value * TempFXVolume;
            }
        }       
        private static float TempFXVolume
        {
            get => Instance.audioFXTempVolume;
            set
            {
                Instance.audioFXTempVolume = value;
                Instance.audioFX.volume = value * BaseFXVolume;
            }
        }
        #endregion

        #region Methods
        public static void PlayBGM(AudioClip newBGM, bool isLoop = true, float playTime = 0.0f, float dampingTime = 1.0f, float tempVolume = 1.0f)
        {
            var am = Instance;

            am.audioBGM.Stop();
            TempBGMVolume = tempVolume;
            am.audioBGM.clip = newBGM;
            am.audioBGM.loop = isLoop;
            am.audioBGM.Play();
            am.audioBGMPlayTime = playTime;
            am.audioBGMDampingTime = dampingTime;

            if (!isLoop)
                am.StartCoroutine(am.StopTimer(playTime, dampingTime));
        }
        public static void PlayFX(AudioClip newFX, float tempVolume = 1.0f, float delay = 0.0f)
        {
            var am = Instance;

            am.audioFX.Stop();
            TempFXVolume = tempVolume;
            am.audioFX.clip = newFX;
            if (delay == 0.0f)
                am.audioFX.Play();
            else
                am.audioFX.PlayDelayed(delay);
        }
        public static void StopBGM()
        {
            Instance.audioBGM.Stop();
        }
        public static void StopFX()
        {
            Instance.audioFX.Stop();
        }
        private IEnumerator StopTimer(float playTime, float dampingTime)
        {
            yield return CachedWaitForSeconds.Get(playTime);

            float dTime = (0.1f / dampingTime) * audioBGM.volume;
            while (dampingTime > 0.0f)
            {
                yield return CachedWaitForSeconds.Get(0.1f);

                dampingTime -= 0.1f;
                audioBGM.volume -= dTime;
            }
            audioBGM.volume = 0.0f;
        }
        public static BGMData GetBgmData()
        {
            var am = AudioManager.Instance;

            var bgmData = new BGMData();
            bgmData.bgm = am.audioBGM.clip;
            bgmData.vol = AudioManager.TempBGMVolume;
            bgmData.bLoop = am.audioBGM.loop;
            bgmData.playTime = am.audioBGMPlayTime;
            bgmData.dampingTime = am.audioBGMDampingTime;

            return bgmData;
        }
        #endregion
    }
}