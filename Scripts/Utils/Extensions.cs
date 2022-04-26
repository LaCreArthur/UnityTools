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
    }
}
