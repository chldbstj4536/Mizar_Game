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
        private Button removeBtn;
        private int slotIndex;

        private InGameSaveData loadData;

        private void Start()
        {
            preview.GetComponent<Button>().onClick.AddListener(() => { SaveDataManager.Instance.StartGameWithSave(slotIndex); });
            removeBtn.onClick.AddListener(() =>
            {
                SaveDataManager.Instance.SaveInGameData(slotIndex, InGameSaveData.NewSaveData);
                SetLoadButton(slotIndex);
            });
        }

        public void SetLoadButton(int index)
        {
            slotIndex = index;
            loadData = SaveDataManager.Instance.GetInGameSaveData(index);
            if (loadData.bgData.img == null)
                preview.sprite = ResourceManager.GetResource<Sprite>(ResourcePath.LoadPreviewDefaultImg);
            else
                preview.sprite = loadData.bgData.img;
            text.text = loadData.saveTime;
        }
    }
}