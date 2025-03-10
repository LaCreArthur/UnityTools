using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AS.Toolbox.Utils
{
    public static class Extensions
    {

        public static bool CollidesWith(this LayerMask layerMask, int layer) => (1 << layer & layerMask) != 0;

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

        #region UI

        /// <summary>
        ///     Sets this RectTransform's position to match another's, with an option to center align. They must be in the same
        ///     root Canvas.
        /// </summary>
        /// <param name="target">The RectTransform to match position from.</param>
        /// <param name="centerAlign">
        ///     If true, aligns the center of this RectTransform with the center of 'target'. If false,
        ///     aligns pivots.
        /// </param>
        public static void MatchPosition(this RectTransform rt, RectTransform target, bool centerAlign = false)
        {
            if (target == null)
            {
                Debug.LogError("One or both RectTransforms are null.");
                return;
            }

            if (centerAlign)
            {
                // Calculate the offset from pivot to center for both RectTransforms
                var rtOffset = new Vector2(rt.rect.width * (0.5f - rt.pivot.x), rt.rect.height * (0.5f - rt.pivot.y));
                var targetOffset = new Vector2(target.rect.width * (0.5f - target.pivot.x), target.rect.height * (0.5f - target.pivot.y));

                // Set position to align centers
                rt.position = target.position + (Vector3)(targetOffset - rtOffset);
            }
            else
            {
                // Just match pivot positions
                rt.position = target.position;
            }
        }

        #endregion

        #region Math

        /// <summary>
        ///     Normalizes a value from the original range [a, b] to the range [0, 1].
        /// </summary>
        /// <param name="x">The value to normalize.</param>
        /// <param name="a">The minimum value of the original range.</param>
        /// <param name="b">The maximum value of the original range.</param>
        /// <param name="clamp">If true, clamps the result to [0, 1] if x is outside [a, b]. If false, allows extrapolation.</param>
        /// <returns>The normalized value in the range [0, 1], or a value outside [0, 1] if extrapolation is allowed.</returns>
        public static float Normalize(this float x, float a, float b, bool clamp = false)
        {
            if (a == b)
            {
                Debug.LogWarning("The range [a, b] is invalid because b equals a, which would cause division by zero.");
                return 0f;
            }

            float normalized = (x - a) / (b - a);

            // Clamp the result to [0, 1] if requested
            if (clamp)
            {
                normalized = Math.Clamp(normalized, 0f, 1f);
            }

            return normalized;
        }

        /// <summary>
        ///     Performs a linear transformation to map a value from the range [0, 1] to a new range [a, b].
        /// </summary>
        /// <param name="x">The value to transform, typically in the range [0, 1].</param>
        /// <param name="a">The minimum value of the target range.</param>
        /// <param name="b">The maximum value of the target range.</param>
        /// <param name="clamp">If true, clamps x to [0, 1] before transformation. If false, allows extrapolation.</param>
        /// <returns>The transformed value in the range [a, b], or a value outside [a, b] if extrapolation is allowed.</returns>
        public static float LinearTransformation(this float x, float a, float b, bool clamp = false)
        {
            // Clamp x to [0, 1] if requested
            if (clamp)
            {
                x = Math.Clamp(x, 0f, 1f);
            }

            return a + x * (b - a);
        }

        #endregion

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

        public static string FormatCurrencyValue(this float value, bool noDecimals = false)
        {
            string formattedValue = value switch
            {
                >= 10000000 => (value / 1000000).ToString("N0") + "M",
                >= 1000000 => (value / 1000000).ToString("N1") + "M",
                >= 100000 => (value / 1000).ToString("N0") + "K",
                >= 10000 => (value / 1000).ToString("N1") + "K",
                >= 1000 => value.ToString("N0"),
                >= 100 => noDecimals ? value.ToString("N0") : value.ToString("N1"),
                _ => noDecimals ? value.ToString("N0") : value.ToString("N2"),
            };

            // Remove trailing zeros if there are two or more at the end, but keep the first trailing zero if it exists
            if (formattedValue.Contains("."))
            {
                formattedValue = formattedValue.TrimEnd('0');
                if (formattedValue.Length - formattedValue.IndexOf('.') <= 2)
                    formattedValue += '0';
            }

            return formattedValue;
        }

        public static string ToCurrencyString(this float value, bool isDoge, bool escapeForSprite = false, bool noDecimals = false) =>
            $"<sprite index={(isDoge ? 0 : 1)}>{(escapeForSprite ? "\n" : " ")}{FormatCurrencyValue(value, noDecimals)}";
        public static string ToCurrencyString(this double value, bool isDoge, bool noDecimals = false) => ((float)value).ToCurrencyString(isDoge, noDecimals: noDecimals);

        public static string ToDurationString(this int seconds, bool showThreeUnits = false)
        {
            int totalSeconds = Mathf.FloorToInt(seconds);
            int days = totalSeconds / 86400;
            int hours = totalSeconds % 86400 / 3600;
            int minutes = totalSeconds % 3600 / 60;
            int secs = totalSeconds % 60;

            if (days > 0)
            {
                return $"{days} day{(days != 1 ? "s" : "")} {hours} {(showThreeUnits ? $"{minutes}m " : "")}";
            }
            if (hours > 0 || showThreeUnits)
            {
                return $"{hours}h {minutes}m {(showThreeUnits ? $"{secs}s" : "")}";
            }
            if (minutes > 0)
            {
                return $"{minutes}m {secs}s";
            }
            return $"{secs}s";
        }

        public static string ToTimeString(this float minutes) => $"{Mathf.FloorToInt(minutes / 60):0}:{Mathf.FloorToInt(minutes % 60):00}";
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
