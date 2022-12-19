using Sirenix.OdinInspector;
using UnityEngine;

namespace AS.Toolbox.PrefabPool
{
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