using UnityEngine;

namespace Toolbox.ScriptableObjects.Variables
{
    public class StringVariableListener : VariableListenerBase<string, StringVariable>
    {
        protected override void Create(string soName) => CreateSOAsset("StringV_");
    }
}