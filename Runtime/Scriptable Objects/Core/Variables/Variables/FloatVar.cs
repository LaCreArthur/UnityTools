using AS.Toolbox.Utils;
using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/Float", fileName = "float_")]
    public class FloatVar : SOVar<float>
    {
        public override void Save() => EncryptedPlayerPrefs.SetFloat(name, v);

        public override float Load()
        {
            v = EncryptedPlayerPrefs.GetFloat(name, initialValue);
            return v;
        }

        public void Add(float x) => v += x;
        public void Subtract(float x) => v -= x;
        public void Multiply(float x) => v *= x;
        public void Divide(float x) => v /= x;


        #region Operators Overloads

        public static FloatVar operator ++(FloatVar a)
        {
            a.v++;
            return a;
        }

        public static FloatVar operator --(FloatVar a)
        {
            a.v--;
            return a;
        }

        public static FloatVar operator +(FloatVar a) => a;

        public static FloatVar operator +(FloatVar a, FloatVar b)
        {
            var res = CreateInstance<FloatVar>();
            res.v = a.v + b.v;
            return res;
        }

        public static FloatVar operator -(FloatVar a)
        {
            a.v = -a.v;
            return a;
        }

        public static FloatVar operator -(FloatVar a, FloatVar b)
        {
            var res = CreateInstance<FloatVar>();
            res.v = a.v - b.v;
            return res;
        }

        #endregion
    }
}
