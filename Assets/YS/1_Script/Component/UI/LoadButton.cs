using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public class LoadButton : MonoBehaviour
    {
        [SerializeField]
        private Image preview;
        [SerializeField]
        private TMP_Text text;
        [SerializeField]
        private Button exitBtn;
        private int index;

        private InGameSaveData loadData;

        public void SetLoadButton(int index)
        {
            this.index = index;
            loadData = SaveDataManager.Instance.GetInGameSaveData(index);
            if (!loadData.InvalidData)
            {
                preview.sprite = loadData.bgData.img;
                text.text = loadData.saveTime;
            }
        }
    }
}