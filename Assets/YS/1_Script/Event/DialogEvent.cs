using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

namespace YS
{
    [System.Serializable]
    public class DialogEvent : BaseScriptEvent
    {
        [SerializeField]
        [LabelText("ȭ�� ȿ��"), Tooltip("ȭ�� ȿ��\nNONE : ȭ�� ȿ�� ����\nFADE_IN : ���� ȭ�鿡�� ���� ��� ȭ������ ��ȯ\nFADE_OUT : ��� ȭ�鿡�� ���� ���� ȭ������ ��ȯ\nRED_FLASH : ȭ�� ���������� ������")]
        private SCREEN_EFFECT screenEffect;

        [FoldoutGroup("���� ĳ����"), SerializeField]
        [HideLabel]
        private CharacterStruct leftCharacter;
        [FoldoutGroup("�߾� ĳ����"), SerializeField]
        [HideLabel]
        private CharacterStruct centerCharacter;
        [FoldoutGroup("������ ĳ����"), SerializeField]
        [HideLabel]
        private CharacterStruct rightCharacter;

        [SerializeField]
        [LabelText("�ڵ� �ѱ� ���")]
        private bool isAuto;
        [SerializeField, Indent, ShowIf("isAuto")]
        [LabelText("�ڵ� �ѱ� �ð�")]
        private float autoTime = 1.0f;
        [BoxGroup("���̾�α� UI", true, true), SerializeField]
        [LabelText("����"), Tooltip("��ȭ ������ �̸�\n��ĭ�� �� �̸� ĭ UI ����")]
        private string name;
        [BoxGroup("���̾�α� UI"), SerializeField, TextArea]
        [LabelText("��ũ��Ʈ ����"), Tooltip("��ȭ ������ ����\n�̸��� ���� ��� ��ĭ�� �� ��ȭ ���� ����")]
        private string script;

#region Properties
        public SCREEN_EFFECT ScreenEffect => screenEffect;
        public CharacterStruct LeftCharacter => leftCharacter;
        public CharacterStruct CenterCharacter => centerCharacter;
        public CharacterStruct RightCharacter => rightCharacter;
        public bool IsAuto => isAuto;
        public float AutoTime => autoTime;
        public string Name => name;
        public string Script => script;
#endregion

        public override void OnEnter()
        {
            base.OnEnter();

            gm.dialogSystem.Setup(this);
        }
        protected override void OnUpdate()
        {
            if (gm.IsKeyDown())
                gm.dialogSystem.OnDialogEvent(this);
        }
        public override void OnExit()
        {
            gm.dialogSystem.Release();

            base.OnExit();
        }
        [System.Serializable]
        public struct CharacterStruct
        {
            [SerializeField]
            [LabelText("�̹���"), Tooltip("ĳ���� �̹���")]
            public Sprite image;
            [SerializeField, DisableIf("@image == null")]
            [LabelText("�¿� ����"), Tooltip("ĳ���� �̹����� �¿� ���� ��ų���ΰ�")]
            public bool isMirror;
            [SerializeField, DisableIf("@image == null")]
            [LabelText("���� ����"), Tooltip("ĳ���Ͱ� ȭ���ΰ�")]
            public bool isHighlight;
            [SerializeField, DisableIf("@image == null")]
            [LabelText("ȿ��"), Tooltip("�̹��� ȿ��\nNONE : ȿ�� ����\nSHAKE_VERTICAL : �̹��� ���� ����\nSHAKE_HORIZONTAL : �̹��� �¿� ����\nSHAKE_RANDOM : �̹��� ������ �������� ����\nBOUNCE : �پ������")]
            public CHARACTER_EFFECT_INDEX effect;
        }
    }
    [System.Serializable]
    public class DialogSystem
    {
#region Fields
        [FoldoutGroup("���̾�α� UI", false)]
        [LabelText("���̾�α� �г� UI"), Tooltip("���̾�α� ��Ʈ ���ӿ�����Ʈ")]
        public GameObject dialogUI;
        [FoldoutGroup("���̾�α� UI"), ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, DraggableItems = false, OnBeginListElementGUI = nameof(BeginDrawListElement)), DisableContextMenu]
        [LabelText("�̹��� ��ġ"), Tooltip("���̾�α� ����/�߾�/������ Image ������Ʈ")]
        public Image[] sideImg = new Image[3];
        [FoldoutGroup("���̾�α� UI")]
        [LabelText("�̸� TMP"), Tooltip("���̾�α� â �̸� TMP")]
        public TMP_Text nameTMP;
        [FoldoutGroup("���̾�α� UI")]
        [LabelText("��ũ��Ʈ TMP"), Tooltip("���̾�α� â ��ũ��Ʈ TMP")]
        public CustomTMPEffect scriptTMP;
        [FoldoutGroup("���̾�α� UI/ĳ���� ���� ȿ��", false)]
        [LabelText("����"), Tooltip("ĳ���� ������ ����")]
        public float shakeIntensity = 5.0f;
        [FoldoutGroup("���̾�α� UI/ĳ���� ���� ȿ��", false)]
        [LabelText("���� �ð�"), Tooltip("ĳ���� ���� ȿ�� �ð�")]
        public float shakeTime = 0.5f;
        [FoldoutGroup("���̾�α� UI/ĳ���� ���� ȿ��", false)]
        [LabelText("���� ����"), Tooltip("ĳ���Ͱ� ��ŭ�� �ð��������� �������� ���� ��ġ")]
        public float shakeIntervalTime = 0.01f;
        [FoldoutGroup("���̾�α� UI/ĳ���� �ٿ ȿ��", false)]
        [LabelText("���� �ð�"), Tooltip("ĳ���Ͱ� �ٴ� �ð�\n'1 / ���ӽð�'sec\nex)1 = 1sec, 2 = 0.5sec, 0.5 = 2sec")]
        public float bounceTime = 3.0f;
        [FoldoutGroup("���̾�α� UI/ĳ���� �ٿ ȿ��", false)]
        [LabelText("����"), Tooltip("ĳ���Ͱ� �پ������ ����")]
        public float bounceHeight = 100.0f;

        // sideImg�� �ʱ� ��ġ (FX�ʱ�ȭ �� �� ���)
        private Vector3[] sidePos;
        // sideFX �ڷ�ƾ ���� (��ŵ �� ���)
        private Coroutine[] sideFXCoroutine;
        private GameManager gm;
        private InGameUIManager uim;
#endregion

#region Properties
        public Vector3[] SidePosition => sidePos;
#endregion

#region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;
            uim = InGameUIManager.Instance;

            sidePos = new Vector3[3];
            // �ʱ�ȭ�� ���� ó���� Left, Right ���̵� �̹����� ��ġ ���
            for (int i = 0; i < 3; ++i)
                sidePos[i] = sideImg[i].transform.position;

            sideFXCoroutine = new Coroutine[2];
        }
        /// <summary>
        /// ���̾�α� ����
        /// </summary>
        public void Setup(DialogEvent de)
        {
            dialogUI.SetActive(true);
            gm.ScreenEffect(de.ScreenEffect);

            if (de.Name == null || de.Name == "")
            {
                if (de.Script == null || de.Script == "")
                {
                    dialogUI.SetActive(false);
                    return;
                }

                nameTMP.transform.parent.gameObject.SetActive(false);
            }
            else
                nameTMP.transform.parent.gameObject.SetActive(true);

            gm.Logging($"<b><size={uim.logNameSize}>{de.Name}</size></b>\n<size={uim.logDescSize}>        {de.Script}</size>\n");

            nameTMP.SetText(de.Name);
            scriptTMP.SetText(de.Script);

            SetCharSetting(SIDE_IMAGE.LEFT_SIDE, de.LeftCharacter);
            SetCharSetting(SIDE_IMAGE.CENTER_SIDE, de.CenterCharacter);
            SetCharSetting(SIDE_IMAGE.RIGHT_SIDE, de.RightCharacter);

            if (de.IsAuto)
                gm.StartCoroutine(AutoPaging(de.AutoTime));
        }
        public void Release()
        {
            dialogUI.SetActive(false);
            ResetEffects();
        }
        /// <summary>
        /// ���̾�α� �̺�Ʈ �߻��� ȣ��Ǵ� �Լ�
        /// </summary>
        public void OnDialogEvent(DialogEvent de)
        {
            if (de.IsAuto) return;

            // ���콺 Ŭ���� Ÿ������ �ȳ����ٸ� Ÿ���� ������, Ÿ������ �� �Ǿ��ִ� ���¶�� ���� ���̾�α� ����
            if (!scriptTMP.IsDoneTyping)
                scriptTMP.SkipTyping();
            else
                gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        /// <summary>
        /// ĳ���� �̹��� ����
        /// </summary>
        /// <param name="side">���� ���̵����� ������ ���̵����� ����</param>
        /// <param name="charImgIdx">������ �̹���</param>
        /// <param name="isHighlight">���̶���Ʈ ����</param>
        /// <param name="charFX">���̵� �̹����� �� ȿ��</param>
        private void SetCharSetting(SIDE_IMAGE side, DialogEvent.CharacterStruct character)
        {
            sideImg[(int)side].sprite = character.image;
            sideImg[(int)side].transform.localScale = new Vector3(character.isMirror ? -1.0f : 1.0f, 1.0f, 1.0f);

            if (character.image == null)
                sideImg[(int)side].color = Color.clear;
            else
            {
                sideImg[(int)side].color = character.isHighlight ? Color.white : Color.gray;
                sideImg[(int)side].SetNativeSize();
            }

            switch (character.effect)
            {
                case CHARACTER_EFFECT_INDEX.SHAKE_HORIZONTAL:
                case CHARACTER_EFFECT_INDEX.SHAKE_VERTICAL:
                case CHARACTER_EFFECT_INDEX.SHAKE_RANDOM:
                    sideFXCoroutine[(int)side] = gm.StartCoroutine(gm.ShakeEffect(sideImg[(int)side].gameObject.transform, shakeIntensity, shakeTime, shakeIntervalTime, character.effect));
                    break;

                case CHARACTER_EFFECT_INDEX.BOUNCE:
                    sideFXCoroutine[(int)side] = gm.StartCoroutine(gm.BounceEffect(sideImg[(int)side].gameObject.transform, bounceTime, bounceHeight));
                    break;
            }
        }
        /// <summary>
        /// ȿ�� �߰��� ��ŵ�� �� �����Ƿ�, ȿ������ ������̶�� �����Ű�� �̹������� ��ġ �����·� �����ϴ� �Լ�
        /// </summary>
        private void ResetEffects()
        {
            // �۵����� ȿ�� �ڷ�ƾ ���߱�
            if (sideFXCoroutine[0] != null) gm.StopCoroutine(sideFXCoroutine[0]);
            if (sideFXCoroutine[1] != null) gm.StopCoroutine(sideFXCoroutine[1]);
            if (gm.bgFXCoroutine != null) gm.StopCoroutine(gm.bgFXCoroutine);

            for (int i = 0; i < 3; ++i)
                sideImg[i].transform.position = SidePosition[i];

            gm.BackgroundCurrentTime = 1.0f;
            gm.ResetFlash();
        }
        private IEnumerator AutoPaging(float time)
        {
            yield return CachedWaitForSeconds.Get(time);

            gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }

        private void BeginDrawListElement(int index)
        {
            string title = "";
            switch (index)
            {
                case 0:
                    title = "����";
                    break;
                case 1:
                    title = "�߾�";
                    break;
                case 2:
                    title = "������";
                    break;
            }
#if UNITY_EDITOR
            SirenixEditorGUI.Title(title, null, TextAlignment.Center, false);
#endif
        }
#endregion
    }
    public enum SIDE_IMAGE
    {
        LEFT_SIDE,
        CENTER_SIDE,
        RIGHT_SIDE
    }
    public enum SCREEN_EFFECT
    {
        NONE,
        FADE_OUT,
        FADE_IN,
        RED_FLASH,
    }
    public enum CHARACTER_EFFECT_INDEX
    {
        NONE,
        SHAKE_VERTICAL,
        SHAKE_HORIZONTAL,
        SHAKE_RANDOM,
        BOUNCE
    }
}