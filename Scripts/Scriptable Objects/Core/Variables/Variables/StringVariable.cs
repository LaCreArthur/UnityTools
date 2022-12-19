using AS.Toolbox.Utils;
using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/String", fileName = "string_")]
    public class StringVariable : VariableSOBase<string>
    {
        public override void Save()
        {
            EncryptedPlayerPrefs.SetString(name, v);
        }

        public override string Load()
        {
            v = EncryptedPlayerPrefs.GetString(name, initialValue);
            return v;
        }
    }
}