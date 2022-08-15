using Sirenix.OdinInspector;
using Toolbox.Utils;
using UnityEngine;
namespace Toolbox.ScriptableObjects.Variables
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/Bool", fileName = "bool_")]
    public class BoolVar : VariableSOBase<bool>
    {
        [Button]
        public void Toggle() => v = !v;

        public override void Save() => EncryptedPlayerPrefs.SetBool(this.name, v);

        public override bool Load()
        {
            v = EncryptedPlayerPrefs.GetBool(this.name, initialValue);
            return v;
        }

        public static implicit operator bool(BoolVar b) => b.v;
    }
}
