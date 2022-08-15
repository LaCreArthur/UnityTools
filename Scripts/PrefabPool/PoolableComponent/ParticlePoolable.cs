using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class ParticlePoolable : MonoBehaviour, IPoolableComponent
{
    ParticleSystem _ps;

    void Awake() => _ps = GetComponent<ParticleSystem>();

    public void OnSpawn() => _ps.Play();

    public void OnDespawn() => _ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

    public void OnParticleSystemStopped() => PrefabPoolingSystem.Despawn(gameObject);
}