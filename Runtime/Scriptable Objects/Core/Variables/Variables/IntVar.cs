using AS.Toolbox.Utils;
using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/Int", fileName = "int_")]
    public class IntVar : SOVar<int>
    {
        public override void Save() => EncryptedPlayerPrefs.SetInt(name, v);

        public override int Load()
        {
            v = EncryptedPlayerPrefs.GetInt(name, initialValue);
            return v;
        }

        public void Add(int x) => v += x;
        public void Subtract(int x) => v -= x;
        public void Multiply(int x) => v *= x;
        public void Divide(int x) => v /= x;


        #region Operators Overloads
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

        public static IntVar operator +(IntVar a) => a;

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

        public static IntVar operator *(IntVar a, IntVar b)
        {
            var res = CreateInstance<IntVar>();
            res.v = a.v * b.v;
            return res;
        }

        public static IntVar operator /(IntVar a, IntVar b)
        {
            var res = CreateInstance<IntVar>();
            res.v = a.v / b.v;
            return res;
        }
        #endregion
    }
}
