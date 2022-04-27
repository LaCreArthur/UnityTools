using UnityEngine;
using Toolbox.ScriptableObjects.Variables;

public class TransformVariableSetter : MonoBehaviour
{
    public TransformVariable transformVariable;
    public bool onAwake;
    
    void Awake()
    {
        if (onAwake) Set();
    }

    public void Set()
    {
        transformVariable.v = this.transform;
    }
}