using Sirenix.OdinInspector;
using UnityEngine;
using Toolbox.Utils;

namespace UnityReusables.ScriptableObjects.Variables
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Basic Variable/Bool")]
    public class BoolVariable : BaseVariable<bool>
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