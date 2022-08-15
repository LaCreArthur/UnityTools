using Toolbox.Utils;
using UnityEngine;
namespace Toolbox.ScriptableObjects.Variables
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/Int", fileName = "int_")]
    public class IntVar : VariableSOBase<int>
    {
        public override void Save() => EncryptedPlayerPrefs.SetInt(this.name, v);

        public override int Load()
        {
            v = EncryptedPlayerPrefs.GetInt(this.name, initialValue);
            return v;
        }

        public static IntVar operator ++(IntVar a)
        {
            a.v++;
            return a;
        }

        public static IntVar operator --(IntVar a)
        {
            a.v--;
            return a;
        }

        public static IntVar operator +(IntVar a)
        {
            return a;
        }

        public static IntVar operator +(IntVar a, IntVar b)
        {
            var res = CreateInstance<IntVar>();
            res.v = a.v + b.v;
            return res;
        }

        public static IntVar operator -(IntVar a)
        {
            a.v = -a.v;
            return a;
        }

        public static IntVar operator -(IntVar a, IntVar b)
        {
            var res = CreateInstance<IntVar>();
            res.v = a.v - b.v;
            return res;
        }
    }
}
