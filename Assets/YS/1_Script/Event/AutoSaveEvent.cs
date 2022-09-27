using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public class AutoSaveEvent : BaseScriptEvent
    {
        public override void OnEnter()
        {
            base.OnEnter();

            SaveLoad.OnOverwriteGame(0, gm.CurrentData);
        }
        protected override void OnUpdate() { }
    }
}