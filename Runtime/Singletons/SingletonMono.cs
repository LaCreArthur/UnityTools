using UnityEngine;

namespace AS.Toolbox.Singletons
{
    /// <summary>
    ///     SingletonMB's design intentions:
    ///     - Singleton is already instantiated in the scene at game start (preferably, but it can be instantiated on-demand).
    ///     - The singleton instance is not destroyable between scene changes.
    ///     - The instantiated singleton should be active ("FindObjectofType" will not find it if disabled).
    ///     - "OnAwake" is called once, either at the singleton MonoBehaviour's "Awake" message call or at the first get of the
    ///     instance.
    ///     - Don't call from a non-main thread.
    ///     - "Awake" should not be defined in derived classes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        static T s_instance;
        bool _alive;

        bool _isAwoken;

        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    Debug.LogError("Cannot call SingletonMB instance in Editor mode.");
                    return null;
                }
#endif

                if (s_instance == null)
                {
                    s_instance = FindObjectOfType<T>();
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

        public static bool IsAlive => s_instance != null && s_instance._alive;

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
                _alive = true;
                Init();
            }
        }

        void OnDestroy() => _alive = false;
        void OnApplicationQuit() => _alive = false;

        void Init()
        {
            if (_isAwoken)
                return;

            _isAwoken = true;
            DontDestroyOnLoad(transform.root.gameObject);
            OnAwake();
        }

        protected virtual void OnAwake() {}
    }
}
