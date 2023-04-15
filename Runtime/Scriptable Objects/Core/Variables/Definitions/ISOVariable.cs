using System;

namespace AS.Toolbox.ScriptableObjects
{
    public interface ISOVariable
    {
        public void AddOnChange(Action callback);

        public void RemoveOnChange(Action callback);

        public string ToString();
    }
}
