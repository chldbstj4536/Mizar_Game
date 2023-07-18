using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class UnlockChapterEvent : BaseScriptEvent
    {
        [SerializeField, LabelText("¾ð¶ôÇÒ Ã©ÅÍ")]
        private uint unlockChapter;

        public override void OnEnter()
        {
            base.OnEnter();

            SaveDataManager.Instance.UnlockChapter = unlockChapter;

            gm.scriptData.SetScript(gm.scriptData.CurrentIndex + 1);
        }
        protected override void OnUpdate() { }
    }
}