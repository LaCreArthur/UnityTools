using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AS.Toolbox.Utils
{
    public static class Extensions
    {

        #region Math
        public static float Normalize(this float x, float min, float max) => (x - min) / (max - min);
        #endregion

        public static bool CollidesWith(this LayerMask layerMask, int layer) => ((1 << layer) & layerMask) != 0;

        public static List<T> Except<T>(this List<T> first, List<T> second)
        {
            var firstCopy = new List<T>(first);
            foreach (T t in second)
            {
                if (first.Contains(t))
                    firstCopy.Remove(t);
            }

            return firstCopy;
        }

        public static void SetSizeWithCurrentAnchors(this RectTransform rectTransform, RectTransform otherRectTransform)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, otherRectTransform.rect.size.x);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, otherRectTransform.rect.size.y);
            rectTransform.anchoredPosition = otherRectTransform.anchoredPosition;
        }
        #region Random
        public static T GetRandom<T>(this T[] array) => array[Random.Range(0, array.Length)];

        public static T GetRandom<T>(this List<T> list) => list[Random.Range(0, list.Count)];

        public static KeyValuePair<TKey, Tvalue> GetRandom<TKey, Tvalue>(this Dictionary<TKey, Tvalue> dict) =>
            dict.ElementAt(Random.Range(0, dict.Count));

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
        #endregion

        #region Component
        public static bool LazyGetComponent<T, C>(this C c, ref T component) where C : Component
        {
            if (component != null)
                return true;
            component = c.GetComponent<T>();
            if (component != null)
                return true;
            Debug.LogWarning($"Component of type {typeof(T)} not found on {c.gameObject.name}");
            return false;
        }

        public static T GetOrAddComponent<T>(this Component c) where T : Component
        {
            var component = c.GetComponent<T>();
            if (component == null)
                component = c.gameObject.AddComponent<T>();
            return component;
        }
        #endregion

        #region Arrays
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
            if (newIndex < 0)
                newIndex = maxIndex - 1;
            index = newIndex;
        }

        public static T AtModIndex<T>(this T[] array, int index)
        {
            int arrayLength = array.Length;
            if (arrayLength <= 0)
            {
                Debug.LogWarning("AtModIndex called on empty array");
                return default(T);
            }

            ModIndex(ref index, arrayLength);
            return array[index];
        }
        #endregion

        #region Strings
        static string FormatCurrencyValue(float value) => value switch
        {
            >= 1000000 => (value / 1000000).ToString("N1") + "M",
            >= 100000 => (value / 1000).ToString("N0") + "K",
            >= 10000 => (value / 1000).ToString("N1") + "K",
            >= 1000 => value.ToString("N0"),
            >= 100 => value.ToString("N1"),
            _ => value.ToString("N2")
        };

        public static string ToCurrencyString(this float value, bool isDoge) =>
            $"{FormatCurrencyValue(value)}<sprite index={(isDoge ? 0 : 1)}>";
        public static string ToCurrencyString(this double value, bool isDoge) => ((float)value).ToCurrencyString(isDoge);

        public static string ToTimeString(this float time) => $"{Mathf.FloorToInt(time / 60):0}:{Mathf.FloorToInt(time % 60):00}";
        public static string ToTimeString(this TimeSpan time, bool showZeroDay = false) =>
            $"{(showZeroDay || time.TotalDays >= 1 ? $"{time.Days:D}D " : "")}{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";

        public static string ToStringWithNoNamespace<T>(this T obj)
        {
            string objString = obj.ToString();
            string typeString = obj.GetType().Name;
            int lastIndex = objString.LastIndexOf('(');
            return lastIndex >= 0 ? $"{objString[..lastIndex]}({typeString})" : objString;
        }
        public static string TypeAndNameToString(this ScriptableObject so) => $"{so.GetType().Name} [<color=#00FFFF>{so.name}</color>]";
        #endregion
    }
}
