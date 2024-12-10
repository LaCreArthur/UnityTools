using System;
using AS.Toolbox.Singletons;
using UnityEngine;

namespace AS.Toolbox.Utils
{
    /// <summary>
    ///     Use this class to run coroutines from static methods.
    /// </summary>
    public class CoroutineRunner : SingletonMono<CoroutineRunner>
    {
        public static Coroutine WaitForEndOfFrame(Action callback) => Instance.StartCoroutine(Coroutines.WaitForEndOfFrame(callback));
        public static Coroutine WaitForFrames(int frames, Action callback) => Instance.StartCoroutine(Coroutines.WaitForFrames(frames, callback));
        public static Coroutine WaitForSecond(float time, Action callback) => Instance.StartCoroutine(Coroutines.WaitForSecond(time, callback));
    }
}
