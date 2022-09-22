using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class StateStack
    {
        #region Fields
        // UI 상태 변수
        private Stack<int> stateStack = new Stack<int>();

        public delegate void PushPopEvent(int index);
        #endregion

        #region Properties
        public int CurrentState => stateStack.Count == 0 ? -1 : stateStack.Peek();
        public int Count => stateStack.Count;
        #endregion

        #region Events
        public event PushPopEvent OnPushEvent;
        public event PushPopEvent OnPopEvent;
        #endregion

        #region Methods
        public void PushState(uint stateIndex)
        {
            stateStack.Push((int)stateIndex);
            OnPushEvent?.Invoke((int)stateIndex);
        }

        public int PopState()
        {
            int index = stateStack.Pop();

            OnPopEvent?.Invoke(index);

            return index;
        }
        public void PopState(uint dest, bool includeDest = true)
        {
            if (!stateStack.Contains((int)dest))
                throw new System.ArgumentException("Do not exist dest");

            if (includeDest)
                while (PopState() != dest) ;
            else
                while (stateStack.Peek() != dest)
                    PopState();

            return;
        }
        public bool Contains(int state)
        {
            return stateStack.Contains(state);
        }
        #endregion
    }
}