using AS.Toolbox.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/Bool", fileName = "bool_")]
    public class BoolVar : VariableSOBase<bool>
    {
        [Button]
        public void Toggle() => v = !v;

        public override void Save() => EncryptedPlayerPrefs.SetBool(name, v);

        public override bool Load()
        {
            v = EncryptedPlayerPrefs.GetBool(name, initialValue);
            return v;
        }

        public static implicit operator bool(BoolVar b) => b.v;
    }
}