using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    public class GameManager : SingletonMono<GameManager>
    {
        #region Field
        /// <summary>
        /// ���̾�α� �ý���
        /// </summary>
        [HideLabel]
        public DialogSystem dialogSystem;
        /// <summary>
        /// ������ �ý���
        /// </summary>
        [HideLabel]
        public ChoiceSystem choiceSystem;
        /// <summary>
        /// ���� �ý���
        /// </summary>
        [HideLabel]
        public InvestigationSystem ivSystem;
        /// <summary>
        /// �߸� �ý���
        /// </summary>
        [HideLabel]
        public InferenceSystem ifSystem;
        /// <summary>
        /// ���� �ý���
        /// </summary>
        [HideLabel]
        public ArrangeSystem arSystem;

        /// <summary>
        /// ���� �ý���
        /// </summary>
        [HideLabel]
        public PuzzleSystem puzzleSystem;

        [LabelText("�α� TMP")]
        public TMP_Text logTMP;

        [LabelText("��ũ��Ʈ ������")]
        public ScriptData scriptData;
        [LabelText("������ ������")]
        public ItemData itemData;
        [LabelText("�κ��丮 ������Ʈ")]
        public InventoryComponent invenComp;
        private BackgroundComponent bc;
        private Material bgMtrl;

        [HideInInspector]
        public Coroutine bgFXCoroutine;

        [LabelText("���� Ŀ���� ������ ����"), ShowInInspector]
        private Dictionary<string, CustomVariable> varTable = new Dictionary<string, CustomVariable>();
        private InGameUIManager um;
        private StringBuilder log = new StringBuilder();

        public delegate void OnUpdate();
        public event OnUpdate OnUpdateEvent;
        #endregion

        #region Properties
        public Dictionary<string, CustomVariable> VariablesTable => varTable;
        public InGameSaveData CurrentData
        {
            get
            {
                List<VariableData> vd = new List<VariableData>();

                foreach (var data in VariablesTable)
                    vd.Add(new VariableData() { name = data.Key, value = data.Value });
                BackgroundData bgData = new BackgroundData();
                bgData.name = bc.BackgroundName;
                bgData.img = bc.Image;
                bgData.items = new List<BackgroundItemData>(bc.RemainItemCount);
                foreach (var item in bc.Items)
                    bgData.items.Add(new BackgroundItemData(item));
                return new InGameSaveData(0, scriptData.CurrentIndex, bgData, IsBackgroundFadeOut, invenComp.Items, vd);
            }
        }
        /// <summary>
        /// ��� ���̵�ȿ�� ���൵ ����
        /// </summary>
        public float BackgroundCurrentTime
        {
            get => bgMtrl.GetFloat("_CurTime");
            set => bgMtrl.SetFloat("_CurTime", value);
        }
        /// <summary>
        /// ��� ���̵� ����
        /// </summary>
        public bool IsBackgroundFadeOut
        {
            get => bgMtrl.GetFloat("_IsOut") == 0.0f ? false : true;
            set => bgMtrl.SetFloat("_IsOut", value ? 1.0f : 0.0f);
        }
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            bgMtrl = ResourceManager.GetResource<Material>(ResourcePath.BGFXMtrl);
        }
        void Start()
        {
            um = InGameUIManager.Instance;
            bc = BackgroundComponent.Instance;

            IsBackgroundFadeOut = true;
            BackgroundCurrentTime = 0.0f;
            dialogSystem.Initialize();
            choiceSystem.Initialize();
            ivSystem.Initialize();
            ifSystem.Initialize();
            arSystem.Initialize();
            puzzleSystem.Initialize();
            
            var initData = GameObject.FindObjectOfType<InGameInitData>();
            if (initData != null)
            {
                if (initData.data.bgData.name != null && initData.data.bgData.name != "")
                    bc.SetBackground(initData.data.bgData);
                IsBackgroundFadeOut = initData.data.isFadeOut;
                BackgroundCurrentTime = 1.0f;
                foreach (var invenItem in initData.data.invenItems)
                    invenComp.AddItem(invenItem);
                foreach (var data in initData.data.variableDatas)
                    varTable.Add(data.name, data.value.Instantiate());

                scriptData = ResourceManager.GetResource<ScriptData>(ResourcePath.ScriptData);
                scriptData.SetScript(initData.data.scriptIndex);
                Destroy(initData.gameObject);
            }
            else
                scriptData.SetScript(0);
                  
        }
        void Update()
        {
            if (um.CurrentState == InGameUIManager.INGAME_UI_STATE.GAME)
                OnUpdateEvent?.Invoke();


            // Save ����Ű
            if (Input.GetKeyDown(KeyCode.S))
            {
                
            }
            // Item ����Ű
            else if (Input.GetKeyDown(KeyCode.E))
            {

            }
            // Log ����Ű
            else if (Input.GetKeyDown(KeyCode.Q))
            {

            }
            // Settings ����Ű
            else if (Input.GetKeyDown(KeyCode.R))
            {

            }
            // Menu ����Ű
            else if (Input.GetKeyDown(KeyCode.Tab))
            {

            }
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

            // GameState�̰�
            result = um.CurrentState == InGameUIManager.INGAME_UI_STATE.GAME &&
                     // �����̽� Ű�� ���Ȱų�
                     Input.GetKeyDown(KeyCode.Space) ||
                     // UI�� �ƴѰ��� ���콺 Ŭ�� �̺�Ʈ�� �߻�������
                     (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject());

            return result;
        }
        /// <summary>
        /// �α� �����
        /// </summary>
        public void Logging(string str)
        {
            log.Append(str);
            logTMP.SetText(log);
        }

        #region FX
        /// <summary>
        /// ȭ�� ȿ��
        /// </summary>
        /// <param name="screenFX">������ ȿ��</param>
        public void ScreenEffect(SCREEN_EFFECT screenFX)
        {
            switch (screenFX)
            {
                case SCREEN_EFFECT.FADE_OUT:
                    bgFXCoroutine = StartCoroutine(FadeEffect(true, 1.0f));
                    break;
                case SCREEN_EFFECT.FADE_IN:
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
        public IEnumerator FadeEffect(bool isOut, float time)
        {
            WaitForSeconds wf = CachedWaitForSeconds.Get(0.01f);
            float curTime = 0.0f;

            IsBackgroundFadeOut = isOut;
            
            while (curTime < time)
            {
                BackgroundCurrentTime = curTime / time;
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