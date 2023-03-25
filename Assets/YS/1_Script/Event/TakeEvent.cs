using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.Experimental.Rendering;

namespace YS
{
    public class TakeEvent : BaseScriptEvent
    {
        #region Fields
        [ValueDropdown("@ClueDataSO.Names")]
        [SerializeField, LabelText("�ܼ�")]
        private string clueName;
        #endregion

        public override void OnEnter()
        {
            base.OnEnter();

            gm.tkSystem.Setup(ClueDataSO.Instance[clueName]);
        }
        protected override void OnUpdate()
        {
            gm.tkSystem.OnUpdate();
        }
        public override void OnExit()
        {
            gm.tkSystem.Release();

            base.OnExit();
        }
    }
    [System.Serializable]
    public class TakeSystem
    {
        #region Fields
        [FoldoutGroup("ä�� UI", false)]
        [LabelText("ä�� �г� UI"), Tooltip("ä�� �г� ��Ʈ ���ӿ�����Ʈ")]
        public GameObject rootObj;
        [FoldoutGroup("ä�� UI")]
        [LabelText("�ܼ�"), Tooltip("�ܼ� ������")]
        public Image clueImg;
        [FoldoutGroup("ä�� UI")]
        [LabelText("ä���ϴ� ĳ���� UI"), Tooltip("�»�ܿ� �ִ� ĳ���� �̹��� ������Ʈ")]
        public Image takingCharacter;
        [FoldoutGroup("ä�� UI")]
        [LabelText("ä�� �Ϸ� ȿ�� �ִϸ�����"), Tooltip("ä�� �Ϸ� �� ����� �ִϸ�����")]
        public Animator findAllFingerprintsFXAnimator;
        [FoldoutGroup("ä�� UI")]
        [LabelText("ä�� ���콺 Ŀ�� �̹���"), Tooltip("ä�� �� ����� ���콺 Ŀ�� �̹���")]
        public Texture2D cursorImg;

        private List<Button> fingerprints = new List<Button>();
        private bool isFoundAll;
        private GameObject fpTemplate;
        private GameManager gm;
        #endregion

        #region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;
            takingCharacter.GetComponent<Button>().onClick.AddListener(() => { GameManager.Instance.tkSystem.OnClearBtnDown(); });
            fpTemplate = ResourceManager.GetResource<GameObject>(ResourcePath.FingerprintPrefab);
        }
        public void Setup(ClueData cd)
        {
            rootObj.SetActive(true);

            clueImg.sprite = cd.img;
            clueImg.SetNativeSize();

            fingerprints.Capacity = cd.fds.Count;
            foreach (var fd in cd.fds)
            {
                var fp = Object.Instantiate(fpTemplate, clueImg.transform).GetComponent<Button>();
                Image img = fp.image;

                fp.transform.localPosition = fd.pos;
                img.sprite = fd.img;
                img.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                fp.onClick.AddListener(() =>
                {
                    Color c = img.color;
                    if (c.a >= 1.0f) return;
                    c.a += 0.1f;
                    img.color = c;
                });

                fingerprints.Add(fp);
            }

            isFoundAll = false;

            Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.Auto);
        }
        public void OnUpdate()
        {
            if (isFoundAll)
                return;

            foreach (var fp in fingerprints)
                if (fp.image.color.a < 1)
                    return;

            findAllFingerprintsFXAnimator.SetBool("IsFindAllItem", true);
            isFoundAll = true;
        }
        public void Release()
        {
            rootObj.SetActive(false);

            foreach (var fp in fingerprints)
                Object.Destroy(fp.gameObject);

            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        private void OnClearBtnDown()
        {
            if (isFoundAll)
                gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        #endregion
    }
}