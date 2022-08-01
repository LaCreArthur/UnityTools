using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Toolbox.Utils
{
    public static class Extensions
    {
        public static T GetRandom<T>(this T[] array) => array[Random.Range(0, array.Length)];
        public static T GetRandom<T>(this List<T> list) => list[Random.Range(0, list.Count)];
        public static KeyValuePair<TKey, Tvalue> GetRandom<TKey, Tvalue>(this Dictionary<TKey, Tvalue> dict) 
            => dict.ElementAt(Random.Range(0, dict.Count));
        public static float RandomInside(this Vector2 v) => Random.Range(v.x, v.y);
        public static int RandomInside(this Vector2Int v) => Random.Range(v.x, v.y);
        public static void Shuffle<T>(this IList<T> lst)
        {
            int n = lst.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                (lst[k], lst[n]) = (lst[n], lst[k]);
            }
        }
        
        public static bool LazyGetComponent<T, C>(this C c, ref T component) where C : Component
        {
            if (component != null) return true;
            component = c.GetComponent<T>();
            if (component != null) return true;
            Debug.LogWarning($"Component of type {typeof(T)} not found on {c.gameObject.name}");
            return false;
        }
        
        public static bool CollidesWith(this LayerMask layerMask, int layer) => ((1 << layer) & layerMask) != 0;
        
        public static void MoveIndex(this ref int index, bool left, int maxIndex)
        {
            if (maxIndex < 1)
            {
                Debug.LogWarning($"MoveIndex: invalid maxIndex < 1 ({maxIndex})");
                return;
            }
            int newIndex = index + (left ? -1 : 1);
            newIndex.ModIndex(maxIndex);
            index = newIndex;
        }
        
        static void ModIndex(this ref int index, int maxIndex)
        {
            if (maxIndex < 1)
            {
                Debug.LogWarning($"ModIndex: invalid maxIndex < 1 ({maxIndex})");
                return;
            }
            int newIndex = index % maxIndex;
            if (newIndex < 0) newIndex = maxIndex - 1;
            index = newIndex;
        }
        
        public static T AtModIndex<T>(this T[] array, int index)
        {
            int arrayLength = array.Length;
            if (arrayLength <= 0)
            {
                Debug.LogWarning("AtModIndex called on empty array");
                return default;
            }
            ModIndex(ref index, arrayLength);
            return array[index];
        }
        
        public static string ToTimeString(this float time) => 
            $"{Mathf.FloorToInt(time / 60):0}:{Mathf.FloorToInt(time % 60):00}";
        
        public static string TypeAndNameToString(this ScriptableObject so) => $"{so.GetType().Name} [<color=cyan>{so.name}</color>]";

        public static float Normalize(this float x, float min, float max) => (x - min) / (max - min);
        
        public static string Encrypt(string pass)
        {
            var x = new MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(pass);
            bs = x.ComputeHash(bs);
            var s = new StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }

            return s.ToString();
        }

        public static List<T> Except<T>(this List<T> first, List<T> second)
        {
            List<T> firstCopy = new List<T>(first);
            foreach (T t in second)
            {
                if (first.Contains(t)) firstCopy.Remove(t);
            }
        
            return firstCopy;
        }
    }
}
