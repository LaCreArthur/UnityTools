using System.Collections.Generic;
using AS.Toolbox.PrefabPool.PoolableComponent;
using UnityEngine;

namespace AS.Toolbox.PrefabPool
{
    public struct PoolableInstances
    {
        public GameObject instance;
        public IPoolableComponent[] poolableComponents;
    }

    public class PrefabPool
    {
        readonly Dictionary<GameObject, PoolableInstances> _activeList = new Dictionary<GameObject, PoolableInstances>();
        readonly Queue<PoolableInstances> _inactiveList = new Queue<PoolableInstances>();

        public bool UseRectTransform { get; }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            PoolableInstances data;

            // if there is an inactive go available, respawn it
            if (_inactiveList.Count > 0)
            {
                data = _inactiveList.Dequeue();
                data.instance.transform.SetParent(parent);
            }
            else
            {
                // instantiate a new go and add it to the list
                var newGO = Object.Instantiate(prefab, parent);
                data = new PoolableInstances
                {
                    instance = newGO,
                    poolableComponents = newGO.GetComponentsInChildren<IPoolableComponent>()
                };
            }

            var spawnedGO = data.instance;
            if (UseRectTransform)
                spawnedGO.GetComponent<RectTransform>().anchoredPosition = position;
            else
                spawnedGO.transform.position = position;
            spawnedGO.transform.rotation = rotation;
            spawnedGO.SetActive(true);

            foreach (var pc in data.poolableComponents)
                pc.OnSpawn();
            _activeList.Add(spawnedGO, data);
            return spawnedGO;
        }

        public bool Despawn(GameObject objToDespawn)
        {
            if (!_activeList.ContainsKey(objToDespawn))
            {
                Debug.LogError("This object is not managed by this object pool!", objToDespawn);
                return false;
            }

            var data = _activeList[objToDespawn];

            foreach (var pc in data.poolableComponents)
                pc.OnDespawn();

            data.instance.SetActive(false);
            _activeList.Remove(objToDespawn);
            _inactiveList.Enqueue(data);
            return true;
        }

        public void AddInstances(Component[] components)
        {
            foreach (var co in components)
            {
                var go = co.gameObject;
                var data = new PoolableInstances
                {
                    instance = go,
                    poolableComponents = new[] { co as IPoolableComponent }
                };
                go.SetActive(false);
                _activeList.Remove(go);
                _inactiveList.Enqueue(data);
            }

            // Debug.Log($"added {components.Length} instances to the inactive list");
        }
    }
}
