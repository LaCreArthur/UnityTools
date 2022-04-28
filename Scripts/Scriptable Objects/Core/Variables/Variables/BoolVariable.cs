using Sirenix.OdinInspector;
using UnityEngine;
using Toolbox.Utils;

namespace Toolbox.ScriptableObjects.Variables
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/Bool", fileName = "BoolV_")]
    public class BoolVariable : VariableSOBase<bool>
    {
        [Button()]
        public void Toggle() => v = !v;

        public override void Save() => EncryptedPlayerPrefs.SetBool(this.name, v);

        public override bool Load()
        {
            v = EncryptedPlayerPrefs.GetBool(this.name, initialValue);
            return v;
        }
        
        public static implicit operator bool(BoolVariable b) => b.v;
    }
}