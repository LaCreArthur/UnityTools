using System;
using Object = UnityEngine.Object;

namespace Toolbox.ScriptableObjects.Variables
{
    public interface IVariableSO
    {
        public void AddOnChangeCallback(Action callback, Object listener);

        public void RemoveOnChangeCallback(Action callback, Object listener);

        public string ToString();
    }
}