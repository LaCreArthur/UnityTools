using Toolbox.Utils;
using UnityEngine;
namespace Toolbox.ScriptableObjects.Variables
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/Float", fileName = "float_")]
    public class FloatVar : VariableSOBase<float>
    {
        public override void Save()
        {
            EncryptedPlayerPrefs.SetFloat(this.name, v);
        }

        public override float Load()
        {
            v = EncryptedPlayerPrefs.GetFloat(this.name, initialValue);
            return v;
        }

        public void Add(float x)
        {
            v += x;
        }

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

        public static FloatVar operator +(FloatVar a)
        {
            return a;
        }

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
    }
}
