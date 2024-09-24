using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{
    /// <summary>
    ///     Simple component that sets a TransformVar to the transform of the GameObject.
    ///     This is useful for referencing the transform of a GameObject without hard referencing it in the inspector.
    ///     This is also the only way to reference a scene transform in a prefab.
    /// </summary>
    public class TransformVariableSetter : MonoBehaviour
    {
        public TransformVar transformVar;

        void Awake()
        {
            transformVar.v = transform;
            enabled = false;
        }
    }
}
