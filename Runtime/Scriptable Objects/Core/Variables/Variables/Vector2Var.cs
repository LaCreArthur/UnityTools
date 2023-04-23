using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/Vector2", fileName = "vector2_")]
    public class Vector2Var : SOVar<Vector2>
    {
        public void SetValFromTransformPos(Transform t) => SetValue(t.position);
    }
}
