using AS.Toolbox.Utils;
using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/Double", fileName = "double_")]
    public class DoubleVar : SOVar<double>
    {
        public override void Save()
        {
            EncryptedPlayerPrefs.SetDouble(name, v);
        }

        public override double Load()
        {
            v = EncryptedPlayerPrefs.GetDouble(name, initialValue);
            return v;
        }

        public void Add(double x)
        {
            v += x;
        }

        public void Add(float x)// Unity doesn't serialize double
        {
            v += x;
        }

        public static DoubleVar operator ++(DoubleVar a)
        {
            a.v++;
            return a;
        }

        public static DoubleVar operator --(DoubleVar a)
        {
            a.v--;
            return a;
        }

        public static DoubleVar operator +(DoubleVar a) => a;

        public static DoubleVar operator +(DoubleVar a, DoubleVar b)
        {
            var res = CreateInstance<DoubleVar>();
            res.v = a.v + b.v;
            return res;
        }

        public static DoubleVar operator -(DoubleVar a)
        {
            a.v = -a.v;
            return a;
        }

        public static DoubleVar operator -(DoubleVar a, DoubleVar b)
        {
            var res = CreateInstance<DoubleVar>();
            res.v = a.v - b.v;
            return res;
        }
    }
}