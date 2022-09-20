using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;

        public static T Instance => instance;
        protected virtual void Awake()
        {
            if (instance == null)
                instance = this as T;
            else
                Destroy(gameObject);
        }
    }
}