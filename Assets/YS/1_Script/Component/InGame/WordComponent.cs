using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace YS
{
    public class WordComponent : MonoBehaviour
    {
        public TMP_Text fixedTMP;
        public Button upBtn;
        public Button downBtn;
        public TMP_Text choiceTMP;
        public TMP_Text choiceUpTMP;
        public TMP_Text choiceDownTMP;

        private string[] words;
        private bool isMoving;
        private float wordHeight;
        private int maxIndex;
        private int index = 0;

        public int Index => index + 1;

        private void Start()
        {
            wordHeight = GetComponent<RectTransform>().rect.height;
            upBtn.onClick.AddListener(() => { ChangeWord(true); });
            downBtn.onClick.AddListener(() => { ChangeWord(false); });
        }
        private void Update()
        {
            if (choiceTMP.transform.localPosition.y != 0.0f)
            {
                Vector3 pos = choiceTMP.transform.localPosition;
                pos.y = Mathf.Lerp(pos.y, 0.0f, Time.deltaTime * 8.0f);
                
                if (Mathf.Abs(pos.y) <= 0.1f)
                    pos.y = 0.0f;

                choiceTMP.transform.localPosition = pos;
            }
        }

        public void SetSetting(ArrangeEvent.Word word)
        {
            index = 0;
            words = word.choiceWords;
            maxIndex = word.choiceWords.Length;
            fixedTMP.gameObject.SetActive(word.isFixedWord);
            upBtn.transform.parent.gameObject.SetActive(!word.isFixedWord);

            if (word.isFixedWord)
            {
                fixedTMP.text = word.fixedWord;
            }
            else
            {
                choiceTMP.text = word.choiceWords[0];
                choiceUpTMP.text = word.choiceWords[maxIndex - 1];
                choiceDownTMP.text = word.choiceWords[1];
            }
        }
        public void ChangeWord(bool isUp)
        {
            Vector3 newPos = choiceTMP.transform.localPosition;

            if (isUp)
            {
                index = (index + 1) % maxIndex;
                newPos.y -= wordHeight;
            }
            else
            {
                index = index == 0 ? maxIndex - 1 : index - 1;
                newPos.y += wordHeight;
            }

            choiceTMP.transform.localPosition = newPos;

            choiceUpTMP.text = words[index == 0 ? maxIndex - 1 : index - 1];
            choiceTMP.text = words[index];
            choiceDownTMP.text = words[(index + 1) % maxIndex];
        }
    }
}