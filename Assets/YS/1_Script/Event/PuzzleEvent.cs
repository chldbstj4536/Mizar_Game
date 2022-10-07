using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

namespace YS
{
    [System.Serializable]
    public class PuzzleEvent : BaseScriptEvent
    {
        [LabelText("����"), ValueDropdown("@PuzzleDataSO.Names")]
        public string puzzleName;

        public override void OnEnter()
        {
            base.OnEnter();
        }
        protected override void OnUpdate() { }
    }
    public class PuzzleSyste
    {
        #region Fields
        [FoldoutGroup("���� UI", false)]
        [LabelText("���� �г� UI"), Tooltip("���� �г� ��Ʈ ���ӿ�����Ʈ")]
        public GameObject rootObj;
        [FoldoutGroup("���� UI")]
        [LabelText("���� ������ �̹��� ������Ʈ"), Tooltip("������ �������� ������ �̹��� ������Ʈ")]
        public Image frameImg;
        [FoldoutGroup("���� UI")]
        [LabelText("���� �������� ��ġ�� ������Ʈ")]
        public Transform rootPieceTr;

        private GameManager gm;
        private int successCount;
        #endregion

        #region Methods
        public void Initialize()
        {
            gm = GameManager.Instance;
            successCount = 0;
        }
        public void Setup(PuzzleEvent pe)
        {
            rootObj.SetActive(true);

            PuzzleData data = PuzzleDataSO.Instance[pe.puzzleName];

            frameImg.sprite = data.puzzleFrameImg;
            for (int i = 0; i < data.pieces.Count; ++i)
            {
                var ppc = GameObject.Instantiate(ResourceManager.GetResource<GameObject>(ResourcePath.PuzzlePiecePrefabPath), frameImg.transform).GetComponent<PuzzlePieceComponent>();
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

        }
        #endregion
    }
}