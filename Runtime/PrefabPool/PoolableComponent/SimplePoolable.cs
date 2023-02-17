using UnityEngine;


/// <summary>
///     simple component that can be added to a prefab to make it poolable
/// </summary>
namespace AS.Toolbox.PrefabPool.PoolableComponent
{
    public class SimplePoolable : MonoBehaviour, IPoolableComponent
    {
        public bool resetScaleOnSpawn;
        Vector3 _initialScale;
        Transform _transform;

        void Awake()
        {
            _transform = transform;
            _initialScale = _transform.localScale;
        }

        public void OnSpawn()
        {
            if (resetScaleOnSpawn)
                _transform.localScale = _initialScale;
        }

        public void OnDespawn() {}
    }
}