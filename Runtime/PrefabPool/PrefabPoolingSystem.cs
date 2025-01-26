using System.Collections.Generic;
using AS.Toolbox.PrefabPool.PoolableComponent;
using UnityEngine;

namespace AS.Toolbox.PrefabPool
{
    public static class PrefabPoolingSystem
    {
        static readonly Dictionary<GameObject, PrefabPool> s_prefabToPoolMap = new Dictionary<GameObject, PrefabPool>();
        static readonly Dictionary<GameObject, PrefabPool> s_goToPoolMap = new Dictionary<GameObject, PrefabPool>();

        /// <summary>
        ///     Use this method when loading a new scene, because all refs to pooled instances will be null
        /// </summary>
        public static void Reset()
        {
            s_prefabToPoolMap.Clear();
            s_goToPoolMap.Clear();
        }

        public static void DebugPools()
        {
            Debug.Log("Prefab Pool");
            foreach (KeyValuePair<GameObject, PrefabPool> prefabPool in s_prefabToPoolMap)
                Debug.Log($"{prefabPool.Key} - {prefabPool.Value}");

            Debug.Log("GO Pool");
            foreach (KeyValuePair<GameObject, PrefabPool> goPool in s_goToPoolMap)
                Debug.Log($"{goPool.Key} - {goPool.Value}");
        }

        /// <summary>
        ///     Use this method to prespawn numToSpawn instances and avoid instantiation on framerate critical gamreplay phases
        /// </summary>
        /// <param name="prefab">prefab to prespawn</param>
        /// <param name="numToSpawn">number of instances to spawn</param>
        public static void Prespawn(GameObject prefab, int numToSpawn)
        {
            var spawnedObjects = new List<GameObject>();
            for (int i = 0; i < numToSpawn; i++)
                spawnedObjects.Add(Spawn(prefab));

            for (int i = 0; i < numToSpawn; i++)
                Despawn(spawnedObjects[i]);

            spawnedObjects.Clear();
        }

        public static void PopulateWithInstances(GameObject prefab, GameObject root)
        {
            PrefabPool pool = GetOrCreatePool(prefab);
            var poolableComponent = prefab.GetComponent<IPoolableComponent>();
            Component[] childrenComponents = root.GetComponentsInChildren(poolableComponent.GetType(), true);

            // Debug.Log($"Adding {childrenComponents.Length} GOs to pool {pool}");
            pool.AddInstances(childrenComponents);

            // childrenComponents.ForEach(co => s_goToPoolMap.Add(co.gameObject, pool));
        }

        /// <summary>
        ///     Spawn an instance of the prefab at position and rotation and returns it
        /// </summary>
        /// <returns>the spawned instance</returns>
        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            PrefabPool pool = GetOrCreatePool(prefab);

            GameObject go = pool.Spawn(prefab, position, rotation, parent);
            s_goToPoolMap.Add(go, pool);
            return go;
        }

        static PrefabPool GetOrCreatePool(GameObject prefab)
        {
            s_prefabToPoolMap.TryAdd(prefab, new PrefabPool());
            return s_prefabToPoolMap[prefab];
        }

        /// <summary>
        ///     Spawn an instance of the prefab at 0,0,0 and returns it
        /// </summary>
        /// <returns></returns>
        public static GameObject Spawn(GameObject prefab) => Spawn(prefab, Vector3.zero, Quaternion.identity);

        public static GameObject Spawn(GameObject prefab, Vector3 position) => Spawn(prefab, position, prefab.transform.rotation);

        public static GameObject Spawn(GameObject prefab, Transform parent) =>
            Spawn(prefab, Vector3.zero, prefab.transform.rotation, parent);

        public static GameObject Spawn(GameObject prefab, Vector3 position, Transform parent) =>
            Spawn(prefab, position, prefab.transform.rotation, parent);

        /// <summary>
        ///     Despawn obj if it belongs to a pool
        /// </summary>
        /// <param name="obj">the instance to despawn</param>
        /// <returns>returns true if the object was successfully despawned</returns>
        public static bool Despawn(GameObject obj)
        {
            if (obj == null || !obj.activeSelf)
                return false;

            if (!s_goToPoolMap.TryGetValue(obj, out PrefabPool pool))
            {
                Debug.LogError($"Object {obj.name} not managed by pool system!", obj);
                return false;
            }

            if (pool.Despawn(obj))
            {
                s_goToPoolMap.Remove(obj);
                return true;
            }

            return false;
        }
    }
}
