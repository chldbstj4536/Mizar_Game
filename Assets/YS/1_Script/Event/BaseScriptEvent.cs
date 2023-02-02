using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace YS
{
    /// <summary>
    /// 기본 이벤트 클래스
    /// </summary>
    [System.Serializable]
    public abstract class BaseScriptEvent
    {
        protected GameManager gm;

        public virtual void OnEnter()
        {
            gm = GameManager.Instance;
            gm.OnUpdateEvent += OnUpdate;
            Debug.Log($"{gm.scriptData.CurrentIndex} 스크립트 시작");
        }
        public virtual void OnExit()
        {
            gm.OnUpdateEvent -= OnUpdate;
        }
        protected abstract void OnUpdate();
    }
}
