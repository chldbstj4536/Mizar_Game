using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    public class InvestigationEvent : BaseScriptEvent
    {
        [SerializeField]
        [LabelText("조사를 진행할 캐릭터")]
        private Sprite character;

        public override void OnEnter()
        {
            base.OnEnter();

            gm.ivSystem.Setup(character);
        }
        protected override void OnUpdate()
        {
            gm.ivSystem.OnUpdate();
        }
        public override void OnExit()
        {
            gm.ivSystem.Release();

            base.OnExit();
        }
    }
    [System.Serializable]
    public class InvestigationSystem
    {
        #region Fields
        [FoldoutGroup("조사 UI", false)]
        [LabelText("조사 패널 UI"), Tooltip("조사 패널 루트 게임오브젝트")]
        public GameObject investigationPanel;
        [FoldoutGroup("조사 UI")]
        [LabelText("조사하는 캐릭터 UI"), Tooltip("좌상단에 있는 캐릭터 이미지 컴포넌트")]
        public Image investigationCharacter;
        [FoldoutGroup("조사 UI")]
        [LabelText("조사 완료 효과 애니메이터"), Tooltip("조사 완료 후 재생될 애니메이터")]
        public Animator findAllItemFXAnimator;
        [FoldoutGroup("조사 UI")]
        [LabelText("좌상단 다이얼로그"), Tooltip("조사를 완료하지 않거나 하는 등 좌상단 캐릭터의 대화내용을 표시하는 다이얼로그")]
        public CustomTMPEffect investigationDialogTMP;
        [FoldoutGroup("조사 UI/아이템 획득 창 UI", false)]
        [LabelText("아이템 획득 애니메이터"), Tooltip("아이템 획득 창에 대한 애니메이터")]
        public Animator getItemAnimator;
        [FoldoutGroup("조사 UI/아이템 획득 창 UI")]
        [LabelText("획득 아이템 이미지 UI"), Tooltip("아이템 획득 창에서의 이미지 컴포넌트")]
        public Image getItemUI_ItemImg;
        [FoldoutGroup("조사 UI/아이템 획득 창 UI")]
        [LabelText("획득 아이템 이름 UI"), Tooltip("아이템 획득 창에서의 이름 TMP")]
        public TMP_Text getItemUI_ItemName;
        [FoldoutGroup("조사 UI/아이템 획득 창 UI")]
        [LabelText("획득 아이템 설명 UI"), Tooltip("아이템 획득 창에서의 설명 TMP")]
        public TMP_Text getItemUI_ItemDesc;
        [FoldoutGroup("조사 UI")]
        [LabelText("조사 마우스 커서 이미지"), Tooltip("조사 중 사용할 마우스 커서 이미지")]
        public Texture2D cursorImg;

        private GameObject investigationDialog;
        private Button clearBtn;
        /// <summary>
        /// 남은 아이템 개수
        /// </summary>
        private int findCount;
        private GameManager gm;
        private BackgroundComponent bc;
        private Coroutine CoHideDialogTMP;
        #endregion

        #region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;
            bc = BackgroundComponent.Instance;
            investigationCharacter.GetComponent<Button>().onClick.AddListener(() => { GameManager.Instance.ivSystem.OnClearBtnDown(); });
            investigationDialog = investigationDialogTMP.transform.parent.gameObject;
        }

        /// <summary>
        /// 조사 모드 설정 함수
        /// </summary>
        /// <param name="ivChar">조사를 진행할 캐릭터 인덱스</param>
        /// <param name="nextIndex">조사가 끝난 후 이동할 다음 스크립트 인덱스</param>
        public void Setup(Sprite ivChar)
        {
            investigationCharacter.sprite = ivChar;

            investigationPanel.SetActive(true);

            foreach (var item in BackgroundComponent.Instance.Items)
                item.imageComp.raycastTarget = true;

            findCount = BackgroundComponent.Instance.RemainItemCount;

            if (findCount == 0)
                findAllItemFXAnimator.SetBool("IsFindAllItem", true);

            Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.Auto);
        }
        /// <summary>
        /// 조사 모드 해제 함수
        /// </summary>
        public void Release()
        {
            investigationPanel.SetActive(false);

            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        public void OnUpdate()
        {
            if (gm.IsKeyDown())
            {
                // 화면에 아이템 획득 창이 떠있는가
                if (getItemAnimator.gameObject.activeInHierarchy)
                {
                    // 아이템 획득 창 애니메이션이 완료된 상태인가
                    if (getItemAnimator.GetCurrentAnimatorStateInfo(0).IsName("Complete"))
                    {
                        // 완료된 상태면 아이템 획득 창 비활성화 후 변수 초기화
                        getItemAnimator.gameObject.SetActive(false);
                        getItemAnimator.SetBool("Skip", false);
                    }
                    else
                        // 미완료된 상태면 스킵
                        getItemAnimator.SetBool("Skip", true);
                }
            }
        }
        public void OnFindItem(Item item)
        {
            item.gameObject.SetActive(false);
            --findCount;
            gm.invenComp.AddItem(item.index);

            getItemAnimator.gameObject.SetActive(true);
            getItemUI_ItemImg.sprite = item.ItemImage;
            getItemUI_ItemName.text = item.Name;
            getItemUI_ItemDesc.text = item.Desc;

            if (findCount == 0)
                findAllItemFXAnimator.SetBool("IsFindAllItem", true);
        }
        private void OnClearBtnDown()
        {
            if (findCount == 0)
                gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
            else
            {
                investigationDialog.SetActive(true);
                investigationDialogTMP.SetText("아직 조사가 끝나지 않았어");

                if (CoHideDialogTMP != null)
                    gm.StopCoroutine(CoHideDialogTMP);
                CoHideDialogTMP = gm.StartCoroutine(HideInferenceDialogTMP());
            }
        }
        private IEnumerator HideInferenceDialogTMP()
        {
            yield return CachedWaitForSeconds.Get(3.0f);

            investigationDialog.SetActive(false);
        }
        #endregion
    }
}