using UnityEngine;
using Toolbox.Utils;

namespace Toolbox.ScriptableObjects.Variables
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/String", fileName = "StringV_")]
    public class StringVariable : VariableSOBase<string>
    {
        public override void Save()
        {
            EncryptedPlayerPrefs.SetString(this.name, v);
        }

        public override string Load()
        {
            v = EncryptedPlayerPrefs.GetString(this.name, initialValue);
            return v;
        }
    }
}