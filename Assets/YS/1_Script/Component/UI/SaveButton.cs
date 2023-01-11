using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    public class SaveButton : MonoBehaviour
    {
        public Image previewBG;
        public TMP_Text chTMP;
        public TMP_Text saveTime;
        public Button saveButton;
        public Button removeButton;

        private int slotIndex;

        private void Start()
        {
            saveButton.onClick.AddListener(() =>
            {
                SaveDataManager.Instance.SaveInGameData(slotIndex, GameManager.Instance.CurrentData);
                SetSaveButton(slotIndex);
            });
            removeButton.onClick.AddListener(() =>
            {
                SaveDataManager.Instance.SaveInGameData(slotIndex, InGameSaveData.NewSaveData);
                SetSaveButton(slotIndex);
            });
        }

        public void SetSaveButton(int index)
        {
            slotIndex = index;

            var saveData = SaveDataManager.Instance.GetInGameSaveData(slotIndex);
            if (saveData.bgData.img != null)
            {
                previewBG.sprite = saveData.bgData.img;
                chTMP.text = "Chapter n";
                saveButton.GetComponent<TMP_Text>().text = "덮어쓰기";
                saveTime.text = saveData.saveTime;
            }
            else
            {
                previewBG.sprite = ResourceManager.GetResource<Sprite>(ResourcePath.LoadPreviewDefaultImg);
                chTMP.text = "";
                saveButton.GetComponent<TMP_Text>().text = "저장하기";
                saveTime.text = "----.--.-- --:--";
            }
        }
    }
}
