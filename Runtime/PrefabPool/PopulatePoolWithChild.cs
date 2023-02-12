using Sirenix.OdinInspector;
using UnityEngine;

namespace AS.Toolbox.PrefabPool
{
    /// <summary>
    ///     A simple component to populate a pool with the child objects of a game object
    ///     The child objects must be instances of the prefab to be pooled
    ///     Useful for populating a pool before gameplay starts to avoid instantiation spikes
    /// </summary>
    public class PopulatePoolWithChild : MonoBehaviour
    {
        [SerializeField]
        GameObject prefab;

        void Awake() => PrefabPoolingSystem.PopulateWithInstances(prefab, gameObject);

        [Button]
        public void TestSpawn()
        {
            if (Application.isPlaying)
                PrefabPoolingSystem.Spawn(prefab);
            else
                Debug.Log("Application must be playing to spawn prefab");
        }

        [Button]
        public void DebugPools()
        {
            PrefabPoolingSystem.DebugPools();
        }
    }
}