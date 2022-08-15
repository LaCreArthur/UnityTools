using UnityEngine;
namespace Toolbox.ScriptableObjects.Variables
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/Vector2", fileName = "vector2_")]
    public class Vector2Variable : VariableSOBase<Vector2>
    {
        public void SetValFromTransformPos(Transform t)
        {
            SetValue(t.position);
        }
    }
}
