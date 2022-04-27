using System.Collections.Generic;
using UnityEngine;

namespace Toolbox.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Game State")]
    public class GameStateSO : ScriptableObject
    {
        public List<GameStateSO> validNextStates = new List<GameStateSO>();
    }
}