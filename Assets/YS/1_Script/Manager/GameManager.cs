using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    public class GameManager : Singleton<GameManager>
    {
        #region Field
        /// <summary>
        /// 다이얼로그 시스템
        /// </summary>
        [HideLabel]
        public DialogSystem dialogSystem;
        /// <summary>
        /// 선택지 시스템
        /// </summary>
        [HideLabel]
        public ChoiceSystem choiceSystem;
        /// <summary>
        /// 조사 시스템
        /// </summary>
        [HideLabel]
        public InvestigationSystem ivSystem;
        /// <summary>
        /// 추리 시스템
        /// </summary>
        [HideLabel]
        public InferenceSystem ifSystem;
        /// <summary>
        /// 정리 시스템
        /// </summary>
        [HideLabel]
        public ArrangeSystem arSystem;

        [LabelText("로그 TMP")]
        public TMP_Text logTMP;

        [LabelText("스크립트 데이터")]
        public ScriptData scriptData;
        [LabelText("아이템 데이터")]
        public ItemData itemData;
        [LabelText("인벤토리 컴포넌트")]
        public InventoryComponent invenComp;
        [LabelText("배경오브젝트")]
        public GameObject bgUI;
        private Material bgMtrl;

        [HideInInspector]
        public Coroutine bgFXCoroutine;

        [LabelText("현재 커스텀 변수들 상태"), ShowInInspector]
        private Dictionary<string, CustomVariable> varTable = new Dictionary<string, CustomVariable>();
        private InGameUIManager um;
        private StringBuilder log = new StringBuilder();

        public delegate void OnUpdate();
        public event OnUpdate OnUpdateEvent;
        #endregion

        #region Properties
        public int ItemCount => bgUI.transform.childCount;
        public Dictionary<string, CustomVariable> VariablesTable => varTable;
        public SaveLoadData CurrentData
        {
            get
            {
                List<VariableData> vd = new List<VariableData>();

                foreach (var data in VariablesTable)
                    vd.Add(new VariableData() { name = data.Key, value = data.Value });

                return new SaveLoadData(scriptData.CurrentIndex, vd);
            }
        }
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            bgMtrl = ResourceManager.GetResource<Material>("BGFXShader");
        }
        void Start()
        {
            um = InGameUIManager.Instance;

            SetBGFadeInOut(true);
            SetBGCurTime(0.0f);
            dialogSystem.Initialize();
            choiceSystem.Initialize();
            ivSystem.Initialize();
            ifSystem.Initialize();
            arSystem.Initialize();

            var initData = GameObject.FindObjectOfType<InGameInitData>();
            foreach (var data in initData.data.variableDatas)
                varTable.Add(data.name, data.value.Instantiate());
            scriptData.SetScript(initData.data.scriptIndex);
            Destroy(initData.gameObject);
        }
        void Update()
        {
            if (um.CurrentState != InGameUIManager.INGAME_UI_STATE.GAME && IsKeyDown())
                OnUpdateEvent?.Invoke();
        }
        private void OnDestroy()
        {
            scriptData.Clear();
        }
        #endregion

        #region Methods
        public bool IsKeyDown()
        {
            bool result;

            // GameState이고
            result = um.CurrentState == InGameUIManager.INGAME_UI_STATE.GAME &&
                     // 스페이스 키가 눌렸거나
                     Input.GetKeyDown(KeyCode.Space) ||
                     // UI가 아닌곳에 마우스 클릭 이벤트가 발생했을때
                     (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject());

            return result;
        }
        public Item GetItem(int index)
        {
            return bgUI.transform.GetChild(index).GetComponent<Item>();
        }
        /// <summary>
        /// 배경 페이드효과 진행도 설정
        /// </summary>
        /// <param name="curTime">페이드 효과 진행도 변경 (0 ~ 1)</param>
        public void SetBGCurTime(float curTime)
        {
            bgMtrl.SetFloat("_CurTime", curTime);
        }
        /// <summary>
        /// 배경 페이드 설정
        /// </summary>
        /// <param name="fadeCondition">true면 FadeIn, false면 FadeOut</param>
        public void SetBGFadeInOut(bool fadeCondition)
        {
            bgMtrl.SetFloat("_IsIn", fadeCondition ? 1.0f : 0.0f);
        }
        /// <summary>
        /// 로그 남기기
        /// </summary>
        public void Logging(string str)
        {
            log.Append(str);
            logTMP.SetText(log);
        }
        public void ChangeBackground(GameObject newBG)
        {
            Transform canvasTr = bgUI.transform.parent;
            RectTransform newBGTr = newBG.GetComponent<RectTransform>();

            newBGTr.SetParent(canvasTr, true);
            newBGTr.SetAsFirstSibling();
            newBGTr.anchoredPosition = Vector2.zero;
            newBGTr.anchorMin = Vector2.zero;
            newBGTr.anchorMax = Vector2.one;
            newBGTr.sizeDelta = Vector2.zero;
            newBGTr.localScale = Vector3.one;

            Destroy(bgUI);

            bgUI = newBG;
        }

        #region FX
        /// <summary>
        /// 화면 효과
        /// </summary>
        /// <param name="screenFX">적용할 효과</param>
        public void ScreenEffect(SCREEN_EFFECT screenFX)
        {
            switch (screenFX)
            {
                case SCREEN_EFFECT.FADE_IN:
                    bgFXCoroutine = StartCoroutine(FadeEffect(true, 1.0f));
                    break;
                case SCREEN_EFFECT.FADE_OUT:
                    bgFXCoroutine = StartCoroutine(FadeEffect(false, 1.0f));
                    break;
                case SCREEN_EFFECT.RED_FLASH:
                    bgMtrl.SetColor("_AddColor", Color.red);
                    Invoke(nameof(ResetFlash), 0.25f);
                    break;
            }
        }
        public IEnumerator ShakeEffect(Transform target, float intensity, float time, float intervalTime, CHARACTER_EFFECT_INDEX type)
        {
            WaitForSeconds interval = CachedWaitForSeconds.Get(intervalTime);

            Vector3 curShakeVector = Vector3.zero;
            Vector3 dir = new Vector3(0.0f, 0.0f, 0.0f);
            float remainingTime = time;
            float curIntensity;

            switch (type)
            {
                case CHARACTER_EFFECT_INDEX.SHAKE_VERTICAL:
                    dir.x = 1.0f;
                    break;
                case CHARACTER_EFFECT_INDEX.SHAKE_HORIZONTAL:
                    dir.y = 1.0f;
                    break;
            }

            while (remainingTime > 0.0f)
            {
                if (type == CHARACTER_EFFECT_INDEX.SHAKE_RANDOM)
                    dir = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector3.right;
                else
                    dir = -dir;

                curIntensity = intensity * (remainingTime / time);

                target.position -= curShakeVector;
                curShakeVector = dir * curIntensity;
                target.position += curShakeVector;

                remainingTime -= intervalTime;
                yield return interval;
            }

            target.position -= curShakeVector;
        }
        public IEnumerator BounceEffect(Transform target, float time, float height)
        {
            float t = 0.0f;
            WaitForSeconds wf = CachedWaitForSeconds.Get(0.01f);
            Bezier bezier = new Bezier();
            bezier.bezierPos = new Vector3[3]
            {
                target.position,
                target.position + Vector3.up * height,
                target.position
            };

            while (t <= 1.0f)
            {
                t += time * 0.01f;
                target.position = bezier.GetBezierPosition(t);
                yield return wf;
            }
        }
        public IEnumerator FadeEffect(bool isIn, float time)
        {
            WaitForSeconds wf = CachedWaitForSeconds.Get(0.01f);
            float curTime = 0.0f;

            bgMtrl.SetFloat("_IsIn", isIn ? 1.0f : 0.0f);
            
            while (curTime < time)
            {
                bgMtrl.SetFloat("_CurTime", curTime / time);
                yield return wf;
                curTime += 0.01f;
            }
        }
        public void ResetFlash()
        {
            bgMtrl.SetColor("_AddColor", Color.black);
        }
        #endregion
        #endregion
    }
}