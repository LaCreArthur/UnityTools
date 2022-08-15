using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ResetRBPoolable : MonoBehaviour, IPoolableComponent
{
    Rigidbody _rb;

    public void OnSpawn() { }

    public void OnDespawn()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody>(); // lazy init
        if (_rb == null)
            return; // no rb
        _rb.velocity = _rb.angularVelocity = Vector3.zero;
    }
}