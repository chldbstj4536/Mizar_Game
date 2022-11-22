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

            gm.puzzleSystem.Setup(this);
        }
        protected override void OnUpdate() { }
    }
    [System.Serializable]
    public class PuzzleSystem
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
        [FoldoutGroup("���� UI")]
        [LabelText("���� ���� ���� ����")]
        public float correctOffset = 10.0f;
        [FoldoutGroup("���� UI")]
        [LabelText("���� ���� ���� ����")]
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