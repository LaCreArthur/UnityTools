using UnityEngine;

namespace Toolbox.Singletons
{
    /// <summary>
    /// SingletonMB's design intentions:
    /// - Singleton is already instantiated in the scene at game start (preferably, but it can be instantiated on-demand).
    /// - The singleton instance is not destroyable between scene changes.
    /// - The instantiated singleton should be active ("FindObjectofType" will not find it if disabled).
    /// - "OnAwake" is called once, either at the singleton MonoBehaviour's "Awake" message call or at the first get of the instance.
    /// - Don't call from a non-main thread.
    /// - "Awake" should not be defined in derived classes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        static T _instance;

        bool _isAwoken;
        bool _alive;

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

                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        System.Type t = typeof(T);
                        _instance = new GameObject(t.Name, t).GetComponent<T>();
                    }

                    _instance.Init();
                }

                return _instance;
            }
        }

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogWarning($"An instance of \"{typeof(T).Name}\" already exists. Destroying duplicate one.",
                    _instance.gameObject);
                Destroy(this);
            }
            else
            {
                _instance = this as T;
                Init();
            }
        }

        void Init()
        {
            if (_isAwoken)
                return;

            _isAwoken = true;
            DontDestroyOnLoad(transform.root.gameObject);
            OnAwake();
        }

        protected virtual void OnAwake() {}

        public static bool IsAlive => _instance != null && _instance._alive;

        void OnDestroy() => _alive = false;
        void OnApplicationQuit() => _alive = false;
    }
}