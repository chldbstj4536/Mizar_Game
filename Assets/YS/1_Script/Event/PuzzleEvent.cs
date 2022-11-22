using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

namespace YS
{
    [System.Serializable]
    public class PuzzleEvent : BaseScriptEvent
    {
        [LabelText("퍼즐"), ValueDropdown("@PuzzleDataSO.Names")]
        public string puzzleName;

        public override void OnEnter()
        {
            base.OnEnter();

            gm.puzzleSystem.Setup(this);
        }
        protected override void OnUpdate() { }
    }
    [System.Serializable]
    public class PuzzleSystem
    {
        #region Fields
        [FoldoutGroup("퍼즐 UI", false)]
        [LabelText("퍼즐 패널 UI"), Tooltip("퍼즐 패널 루트 게임오브젝트")]
        public GameObject rootObj;
        [FoldoutGroup("퍼즐 UI")]
        [LabelText("퍼즐 프레임 이미지 컴포넌트"), Tooltip("퍼즐의 프레임이 보여질 이미지 컴포넌트")]
        public Image frameImg;
        [FoldoutGroup("퍼즐 UI")]
        [LabelText("퍼즐 조각들이 배치될 오브젝트")]
        public Transform rootPieceTr;
        [FoldoutGroup("퍼즐 UI")]
        [LabelText("퍼즐 정답 오차 범위")]
        public float correctOffset = 10.0f;
        [FoldoutGroup("퍼즐 UI")]
        [LabelText("퍼즐 생성 랜덤 범위")]
        public float randomRange = 100.0f;
        
        private GameManager gm;
        private int successCount;
        #endregion

        #region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;
        }
        public void Setup(PuzzleEvent pe)
        {
            rootObj.SetActive(true);

            successCount = 0;

            PuzzleData data = PuzzleDataSO.Instance[pe.puzzleName];

            frameImg.sprite = data.puzzleFrameImg;
            for (int i = 0; i < data.pieces.Count; ++i)
            {
                var ppc = GameObject.Instantiate(ResourceManager.GetResource<GameObject>(ResourcePath.PuzzlePiecePrefab), frameImg.transform).GetComponent<PuzzlePieceComponent>();
                ppc.transform.position = rootPieceTr.position + new Vector3(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange), 0.0f);
                ppc.InitializePiece(data.pieces[i].pieceImg, data.pieces[i].correctPos, frameImg.rectTransform.sizeDelta);
                ppc.OnSuccess += () =>
                {
                    if (++successCount == data.pieces.Count)
                        Clear();
                };
            }
        }
        private void Clear()
        {
            gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        #endregion
    }
}