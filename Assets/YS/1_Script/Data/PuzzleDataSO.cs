using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [CreateAssetMenu(fileName = "PuzzleData", menuName = ("AddData/PuzzleData"))]
    public class PuzzleDataSO : ScriptableObject
    {
        [SerializeField, Searchable, ListDrawerSettings(NumberOfItemsPerPage = 1, ShowPaging = true, HideAddButton = true, HideRemoveButton = true, DraggableItems = false)]
        private List<PuzzleData> puzzles;
        private readonly Dictionary<string, PuzzleData> map = new Dictionary<string, PuzzleData>();

        private static PuzzleDataSO instance;

        public static PuzzleDataSO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = ResourceManager.GetResource<PuzzleDataSO>(ResourcePath.PuzzleData);
                    instance.Initialize();
                }
                return instance;
            }
        }
        public PuzzleData this[string index] => map[index];
        private static string[] Names
        {
            get
            {
                string[] names = new string[Instance.puzzles.Count];
                for (int i = 0; i < Instance.puzzles.Count; ++i)
                    names[i] = Instance.puzzles[i].name;
                return names;
            }
        }

        private void Initialize()
        {
            foreach (var data in puzzles)
                map.Add(data.name, data);
        }
        public void AddPuzzle(PuzzleData data)
        {
            puzzles.Add(data);
        }
    }
    [System.Serializable, DisableContextMenu]
    public struct PuzzleData
    {
        [LabelText("���� �̸�")]
        public string name;
        [LabelText("���� ������ �̹���")]
        public Sprite puzzleFrameImg;
        [LabelText("���� ������")]
        public List<PuzzlePieceData> pieces;
    }
    [System.Serializable, DisableContextMenu]
    public struct PuzzlePieceData
    {
        [LabelText("���� ���� �̹���")]
        public Sprite pieceImg;
        [LabelText("���� ��ġ")]
        public Vector3 correctPos;
    }
}