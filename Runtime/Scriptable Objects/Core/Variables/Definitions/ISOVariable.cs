using System;
using Object = UnityEngine.Object;

namespace AS.Toolbox.ScriptableObjects
{
    public interface ISOVariable
    {
        public void AddOnChange(Action callback, Object listener);

        public void RemoveOnChange(Action callback, Object listener);

        public string ToString();
    }
}