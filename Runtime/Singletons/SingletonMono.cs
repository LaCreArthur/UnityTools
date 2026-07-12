using System;
using UnityEngine;

namespace AS.Toolbox.Singletons
{
    static class SingletonResetRegistry
    {
        static readonly System.Collections.Generic.List<Action> Resetters = new System.Collections.Generic.List<Action>();

        internal static void Register(Action reset) => Resetters.Add(reset);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetAll()
        {
            for (int i = 0; i < Resetters.Count; i++)
                Resetters[i].Invoke();
        }
    }

    public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        static T s_instance;
        static bool s_isQuitting; // Tracks if the application is quitting
        bool _isInit;

        static SingletonMono() => SingletonResetRegistry.Register(ResetStatics);

        static void ResetStatics()
        {
            s_instance = null;
            s_isQuitting = false;
        }

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
                if (s_isQuitting)
                {
                    //  Debug.LogWarning($"Trying to access \"{typeof(T).Name}\" instance while application is quitting. Returning null.");
                    return null;
                }

                if (s_instance == null)
                {
                    s_instance = FindAnyObjectByType<T>();
                    if (s_instance == null)
                    {
                        Debug.LogWarning($"SingletonMB \"{typeof(T).Name}\" instance not found. Creating one.");
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
            if ((s_instance != null) && (s_instance != this))
            {
                // Debug.LogWarning($"An instance of \"{typeof(T).Name}\" already exists. Destroying duplicate one.",
                //     s_instance.gameObject);
                Destroy(this);
            }
            else
            {
                s_instance = this as T;
                Init();
            }
        }

        protected virtual void OnDestroy()
        {
            if ((s_instance != null) && (s_instance == this))
            {
                s_instance = null;
            }
        }

        void OnApplicationQuit() => s_isQuitting = true;

        protected virtual void OnAwake() {}

        void Init()
        {
            if (_isInit)
                return;

            _isInit = true;
            DontDestroyOnLoad(transform.root.gameObject);
            OnAwake();
        }
    }
}
