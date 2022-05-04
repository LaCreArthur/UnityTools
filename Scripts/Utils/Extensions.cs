using System.Collections.Generic;
using UnityEngine;

namespace Toolbox.Utils
{
    public static class Extensions
    {
        public static T GetRandom<T>(this T[] array) => array[Random.Range(0, array.Length)];
        public static T GetRandom<T>(this List<T> list) => list[Random.Range(0, list.Count)];
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
        
        public static string ToTimeString(this float time) => 
            $"{Mathf.FloorToInt(time / 60):0}:{Mathf.FloorToInt(time % 60):00}";
    }
}
