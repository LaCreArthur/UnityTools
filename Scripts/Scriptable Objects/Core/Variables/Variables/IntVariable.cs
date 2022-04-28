using UnityEngine;
using Toolbox.Utils;

namespace Toolbox.ScriptableObjects.Variables
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/Int", fileName = "IntV_")]
    public class IntVariable : VariableSOBase<int>
    {
        public override void Save() => EncryptedPlayerPrefs.SetInt(this.name, v);

        public override int Load()
        {
            v = EncryptedPlayerPrefs.GetInt(this.name, initialValue);
            return v;
        }

        public static IntVariable operator ++(IntVariable a)
        {
            a.v++;
            return a;
        }

        public static IntVariable operator --(IntVariable a)
        {
            a.v--;
            return a;
        }

        public static IntVariable operator +(IntVariable a)
        {
            return a;
        }

        public static IntVariable operator +(IntVariable a, IntVariable b)
        {
            var res = CreateInstance<IntVariable>();
            res.v = a.v + b.v;
            return res;
        }

        public static IntVariable operator -(IntVariable a)
        {
            a.v = -a.v;
            return a;
        }

        public static IntVariable operator -(IntVariable a, IntVariable b)
        {
            var res = CreateInstance<IntVariable>();
            res.v = a.v - b.v;
            return res;
        }
    }
}