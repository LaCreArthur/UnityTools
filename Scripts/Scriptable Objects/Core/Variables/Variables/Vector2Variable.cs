using UnityEngine;

namespace Toolbox.ScriptableObjects.Variables
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Basic Variable/Vector2")]
    public class Vector2Variable : BaseVariable<Vector2>
    {
        public void SetValFromTransformPos(Transform t)
        {
            SetValue(t.position);
        }
    }
}