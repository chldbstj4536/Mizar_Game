using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace YS
{
    public class PuzzleGenerator : MonoBehaviour
    {
        [HorizontalGroup("퍼즐 조각")]
        [BoxGroup("퍼즐 조각/퍼즐 프레임 변경"), HideLabel]
        public Sprite puzzleFrameSprite;
        [BoxGroup("퍼즐 조각/퍼즐 프레임 변경")]
        [Button("프레임 변경")]
        private void ChangePuzzleFrame()
        {
            if (puzzleFrameSprite == null)
                return;

            var img = GetComponent<Image>();
            img.sprite = puzzleFrameSprite;
            img.SetNativeSize();
        }
        [BoxGroup("퍼즐 조각/퍼즐 조각 생성"), HideLabel]
        public Sprite newPieceSprite;
        [BoxGroup("퍼즐 조각/퍼즐 조각 생성")]
        [Button("생성")]
        private void AddPuzzlePiece()
        {
            if (newPieceSprite == null)
                return;

            var obj = Instantiate(ResourceManager.GetResource<GameObject>(ResourcePath.PuzzlePiecePrefab));
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            var img = obj.GetComponent<Image>();
            img.sprite = newPieceSprite;
            img.SetNativeSize();
        }
        [HorizontalGroup("퍼즐 조각")]
        [BoxGroup("퍼즐 조각/퍼즐 저장"), LabelText("퍼즐 이름"), LabelWidth(60.0f), ValidateInput(nameof(ValidateHasNotKey), "존재하는 퍼즐 이름입니다.")]
        public string puzzleName;
        [BoxGroup("퍼즐 조각/퍼즐 저장")]
        [Button("저장")]
        private void SavePuzzle()
        {
            var data = new PuzzleData();
            data.name = puzzleName;
            data.puzzleFrameImg = GetComponent<Image>().sprite;
            data.pieces = new List<PuzzlePieceData>();
            for (int i = 0; i < transform.childCount; ++i)
            {
                PuzzlePieceData pieceData = new PuzzlePieceData();
                var child = transform.GetChild(i);
                pieceData.pieceImg = child.GetComponent<Image>().sprite;
                pieceData.correctPos = child.transform.localPosition;
                data.pieces.Add(pieceData);
            }
            PuzzleDataSO.Instance.AddPuzzle(data);
        }
        [Button("초기화")]
        private void ResetPuzzle()
        {
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        private bool ValidateHasNotKey(string newKey)
        {
            if (newKey == null || newKey == "")
                return false;

            try
            {
                var datas = PuzzleDataSO.Instance[newKey];
            }
            catch
            {
                return true;
            }

            return false;
        }
    }
}