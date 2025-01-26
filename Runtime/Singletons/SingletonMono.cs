using System;
using UnityEngine;

namespace AS.Toolbox.Singletons
{
    public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        static T s_instance;
        static bool _isDestroyed;
        bool _isAwoken;

        public static T Instance
        {
            get {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    Debug.LogError("Cannot call SingletonMB instance in Editor mode.");
                    return null;
                }
#endif

                if (s_instance == null)
                {
                    if (_isDestroyed)
                    {
                        Debug.Log($"SingletonMono {typeof(T).Name} destroyed. Returning null.");
                        return null;
                    }
                    s_instance = FindAnyObjectByType<T>();
                    if (s_instance == null)
                    {
                        Type t = typeof(T);
                        s_instance = new GameObject(t.Name, t).GetComponent<T>();
                    }

                    s_instance.Init();
                }

                return s_instance;
            }
        }

        void Awake()
        {
            if (s_instance != null && s_instance != this)
            {
                Debug.LogWarning($"An instance of \"{typeof(T).Name}\" already exists. Destroying duplicate one.",
                    s_instance.gameObject);
                Destroy(this);
            }
            else
            {
                s_instance = this as T;
                Init();
            }
            Debug.Log($"SingletonMono {typeof(T).Name} awake.");
        }

        protected virtual void OnAwake() {}

        void OnDestroy()
        {
            if (s_instance == this)
            {
                s_instance = null;
                _isDestroyed = true;
                Debug.Log($"SingletonMono {typeof(T).Name} destroyed.");
            }
        }

        void Init()
        {
            if (_isAwoken)
                return;

            _isAwoken = true;
            _isDestroyed = false;
            DontDestroyOnLoad(transform.root.gameObject);
            OnAwake();
        }
    }
}
