using System;
using System.Collections;
using UnityEngine;

namespace AS.Toolbox.Utils
{
    public static class Coroutines
    {
        public static IEnumerator WaitAFrameAnd(Action callback)
        {
            yield return new WaitForEndOfFrame();
            callback?.Invoke();
        }
    }
}
