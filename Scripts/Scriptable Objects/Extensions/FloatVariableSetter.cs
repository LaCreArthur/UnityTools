using System;
using NaughtyAttributes;
using UnityEngine;
using UnityReusables.ScriptableObjects.Variables;

public class FloatVariableSetter : MonoBehaviour
{
    public FloatVariable floatVariable;
    public bool onAwake;
    
    public bool useTransformPos;
    enum TransformPos {X, Y, Z}
    [SerializeField, ShowIf("useTransformPos")]
    TransformPos transformPos;
    
    void Awake()
    {
        if (onAwake) Set();
    }

    public void Set()
    {
        if (!useTransformPos) return;
        
        var position = transform.position;
        floatVariable.v = transformPos switch
        {
            TransformPos.X => position.x,
            TransformPos.Y => position.y,
            TransformPos.Z => position.z,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}