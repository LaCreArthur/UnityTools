using System;
using AS.Toolbox.Singletons;

namespace AS.Toolbox.Utils
{
    /// <summary>
    ///     Use this class to run coroutines from static methods.
    /// </summary>
    public class CoroutineRunner : SingletonMono<CoroutineRunner>
    {
        public static void WaitForEndOfFrame(Action callback) => Instance.StartCoroutine(Coroutines.WaitForEndOfFrame(callback));
        public static void WaitForSecond(float time, Action callback) => Instance.StartCoroutine(Coroutines.WaitForSecond(time, callback));
    }
}
