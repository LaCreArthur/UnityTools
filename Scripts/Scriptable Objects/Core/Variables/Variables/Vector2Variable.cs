using UnityEngine;

namespace Toolbox.ScriptableObjects.Variables
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/Vector2", fileName = "Vector2V_")]
    public class Vector2Variable : VariableSOBase<Vector2>
    {
        public void SetValFromTransformPos(Transform t)
        {
            SetValue(t.position);
        }
    }
}