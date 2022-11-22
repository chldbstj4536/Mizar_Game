using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
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
    public class Singleton<T> where T : class, new()
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = new T();

                return instance;
            }
        }
    }
}