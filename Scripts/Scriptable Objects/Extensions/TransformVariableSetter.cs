using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{
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
}