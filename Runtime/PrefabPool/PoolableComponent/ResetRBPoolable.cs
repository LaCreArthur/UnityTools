using UnityEngine;

namespace AS.Toolbox.PrefabPool.PoolableComponent
{
    [RequireComponent(typeof(Rigidbody))]
    public class ResetRBPoolable : MonoBehaviour, IPoolableComponent
    {
        Rigidbody _rb;
        Rigidbody Rb => _rb ??= GetComponent<Rigidbody>();

        public void OnSpawn() {}

        public void OnDespawn()
        {
            if (Rb == null)
                return;
            Rb.velocity = Rb.angularVelocity = Vector3.zero;
        }
    }
}
