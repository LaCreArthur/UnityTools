using System;

namespace Toolbox.ScriptableObjects.Variables
{
    public interface IRegistrable
    {
        public void AddOnChangeCallback(Action callback);
        public void RemoveOnChangeCallback(Action callback);
    }
}