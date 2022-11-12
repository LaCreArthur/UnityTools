using Toolbox.ScriptableObjects.Variables;
using UnityEngine;

public class TransformVariableSetter : MonoBehaviour
{
    public TransformVar transformVar;
    public bool onAwake = true;

    void Awake()
    {
        if (onAwake) Set();
    }

    public void Set()
    {
        transformVar.v = transform;
    }
}