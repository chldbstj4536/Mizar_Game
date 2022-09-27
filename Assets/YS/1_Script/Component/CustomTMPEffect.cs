using System.Collections;
using UnityEngine;
using TMPro;

namespace YS
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CustomTMPEffect : MonoBehaviour
    {
        public delegate void OnChangedValue();
        public delegate void OnTypingDone();
        public static event OnChangedValue OnChangedTypingSpeedEvent;

        private static float typingSpeed = 0.05f;
        private static float fixedTypingSpeed = -1.0f;
        public static float TypingSpeed
        {
            get { return fixedTypingSpeed < 0.0f ? typingSpeed : fixedTypingSpeed; }
            set
            {
                typingSpeed = value;
                OnChangedTypingSpeedEvent?.Invoke();
            }
        }

        #region Field
        private TMP_Text textComponent;
        private TMP_TextInfo textInfo;

        private int cursor = 0;
        private bool isDoneTyping = true;

        [Header("LinkEffect Setup"), Space(5)]
        public float updateTime = 0.01f;

        public float shakePower = 0.5f;

        public float waveAmp = 10.0f;
        public float waveSpeed = 5.0f;
        [Range(-1.0f, 1.0f)]
        public float waveCycle = 0.3f;

        public float rainbowMoveSpeed = 1.0f;
        public float rainbowStrength = 10.0f;

        private bool isPerVertex = false;

        public event OnTypingDone OnTypingDoneEvent;
        #endregion

        public bool IsDoneTyping => isDoneTyping;

        #region Unity Methods
        private void Awake()
        {
            textComponent = gameObject.GetComponent<TMP_Text>();
            textInfo = textComponent.textInfo;
        }
        private void OnEnable()
        {
            // 유니티 업데이트 함수 대신 코루틴 함수를 사용
            StartCoroutine(TextUpdate());
        }
        #endregion

        #region Methods
        public void SkipTyping()
        {
            if (isDoneTyping)
                return;

            textComponent.maxVisibleCharacters = cursor = textInfo.characterCount;
            isDoneTyping = true;

            OnTypingDoneEvent?.Invoke();
        }

        /// <summary>
        /// 타이핑 효과 코루틴 함수
        /// </summary>
        IEnumerator TypingStart()
        {
            while (cursor < textInfo.characterCount)
            {
                textComponent.maxVisibleCharacters = ++cursor;

                LinkEffect();

                yield return CachedWaitForSeconds.Get(TypingSpeed);
            }

            if (!isDoneTyping)
            {
                isDoneTyping = true;
                OnTypingDoneEvent?.Invoke();
            }
        }

        /// <summary>
        /// 일정 주기마다 갱신시키기 위한 코루틴 함수
        /// </summary>
        IEnumerator TextUpdate()
        {
            WaitForSeconds wfUpdateTime = CachedWaitForSeconds.Get(updateTime);
            while (true)
            {
                LinkEffect();

                yield return wfUpdateTime;
            }
        }

        /// <summary>
        /// TMP의 text중에서 link들을 파싱해, link에 해당하는 문자들에 지정된 효과들을 적용시키는 함수
        /// </summary>
        void LinkEffect()
        {
            TMP_LinkInfo link;
            string[] linkIDs;

            textComponent.ForceMeshUpdate();

            if (textInfo.characterCount == 0)
                return;

            // Loops each link tag
            for (int i = 0; i < textComponent.textInfo.linkCount; ++i)
            {
                link = textInfo.linkInfo[i];
                linkIDs = link.GetLinkID().Split(' ');

                for (int j = 0; j < linkIDs.Length; ++j)
                {
                    for (int linkCharIndex = link.linkTextfirstCharacterIndex; linkCharIndex < link.linkTextfirstCharacterIndex + link.linkTextLength; ++linkCharIndex)
                    {
                        // Gets info on the current character
                        TMP_CharacterInfo charInfo = textInfo.characterInfo[linkCharIndex];

                        if (charInfo.character == ' ') continue;

                        // Gets the index of the current character material
                        int materialIndex = charInfo.materialReferenceIndex;
                        int charVertexIndex = charInfo.vertexIndex;

                        Color32[] newColors = textInfo.meshInfo[materialIndex].colors32;
                        Vector3[] newVertices = textInfo.meshInfo[materialIndex].vertices;

                        string[] p = linkIDs[j].Split('=');

                        switch (p[0])
                        {
                            case "v_shake":
                                isPerVertex = true;
                                ShakeText(newVertices, charVertexIndex);
                                break;
                            case "shake":
                                isPerVertex = false;
                                ShakeText(newVertices, charVertexIndex);
                                break;
                            case "v_wave":
                                isPerVertex = true;
                                WaveText(newVertices, charVertexIndex, linkCharIndex - link.linkTextfirstCharacterIndex);
                                break;
                            case "wave":
                                isPerVertex = false;
                                WaveText(newVertices, charVertexIndex, linkCharIndex - link.linkTextfirstCharacterIndex);
                                break;
                            case "rainbow":
                                RainbowText(newColors, charVertexIndex);
                                break;
                            case "ts":
                                if (link.linkTextfirstCharacterIndex < cursor && cursor <= link.linkTextfirstCharacterIndex + link.linkTextLength)
                                    fixedTypingSpeed = float.Parse(p[1]);
                                else
                                    fixedTypingSpeed = -1.0f;
                                break;
                        }
                    }
                }
            }

            // IMPORTANT! applies all vertex and color changes.
            textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }

        /// <summary>
        /// 텍스트 흔들기 효과
        /// </summary>
        /// <param name="vertices">텍스트 매쉬의 정점 위치들</param>
        /// <param name="charVertexIndex">vertices에서 효과를 줄 글자의 인덱스</param>
        void ShakeText(Vector3[] vertices, int charVertexIndex)
        {
            Vector3 power = Vector3.right * shakePower;

            // 버택스 당 랜덤값이면 버텍스당 랜덤값 초기화
            if (isPerVertex)
            {
                for (int i = 0; i < 4; ++i)
                    vertices[charVertexIndex + i] += Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * power;
            }
            else
            {
                power = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * power;
                for (int vertexIndex = charVertexIndex; vertexIndex < charVertexIndex + 4; ++vertexIndex)
                    vertices[vertexIndex] += power;
            }
        }

        /// <summary>
        /// 텍스트 물결 효과
        /// </summary>
        /// <param name="vertices">텍스트 매쉬의 정점 위치들</param>
        /// <param name="charVertexIndex">vertices에서 효과를 줄 글자의 인덱스</param>
        void WaveText(Vector3[] vertices, int charVertexIndex, int linkCharIndex)
        {
            float angle = (Time.realtimeSinceStartup * waveSpeed) + linkCharIndex * waveCycle * 90.0f * Mathf.Deg2Rad;
            Vector3 offset = Vector3.up * Mathf.Sin(angle) * waveAmp;

            if (isPerVertex)
            {
                vertices[charVertexIndex] += offset;
                vertices[charVertexIndex + 1] += offset;
                offset = Vector3.up * Mathf.Sin(angle + waveCycle * 45.0f * Mathf.Deg2Rad) * waveAmp;
                vertices[charVertexIndex + 2] += offset;
                vertices[charVertexIndex + 3] += offset;
            }
            else
            {
                for (int vertexIndex = charVertexIndex; vertexIndex < charVertexIndex + 4; ++vertexIndex)
                    vertices[vertexIndex] += offset;
            }
        }

        /// <summary>
        /// 텍스트 무지개 색깔 효과
        /// </summary>
        /// <param name="colors">텍스트 매쉬의 정점 색깔들</param>
        /// <param name="charVertexIndex">vertices에서 효과를 줄 글자의 인덱스</param>
        void RainbowText(Color32[] colors, int charVertexIndex)
        {
            float changeSpeed = Time.realtimeSinceStartup * rainbowMoveSpeed;
            for (int i = 0; i < 4; ++i)
                colors[charVertexIndex + i] = Color.HSVToRGB((changeSpeed + (charVertexIndex * 0.001f * rainbowStrength)) % 1.0f, 1.0f, 1.0f);
        }

        /// <summary>
        /// 새로운 대화를 보여주는 함수
        /// </summary>
        /// <param name="text">출력할 문자열</param>
        public void SetText(string text)
        {
            if (!gameObject.activeInHierarchy)
                return;
            textComponent.text = text;
            textComponent.maxVisibleCharacters = cursor = 0;
            isDoneTyping = false;

            textComponent.ForceMeshUpdate();
            StartCoroutine(TypingStart());
        }
        #endregion
    }
}