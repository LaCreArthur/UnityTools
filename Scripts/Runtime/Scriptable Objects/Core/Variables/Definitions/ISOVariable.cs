using System;
using Object = UnityEngine.Object;

namespace AS.Toolbox.ScriptableObjects
{
    public interface ISOVariable
    {
        public void AddOnChangeCallback(Action callback, Object listener);

        public void RemoveOnChangeCallback(Action callback, Object listener);

        public string ToString();
    }
}