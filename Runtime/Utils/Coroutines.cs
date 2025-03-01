using System;
using System.Collections;
using UnityEngine;

namespace AS.Toolbox.Utils
{
    public static class Coroutines
    {
        public static IEnumerator WaitForEndOfFrame(Action callback)
        {
            yield return new WaitForEndOfFrame();
            callback?.Invoke();
        }

        public static IEnumerator WaitForFrames(int frames, Action callback)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            callback?.Invoke();
        }

        public static IEnumerator WaitForSecond(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }

        public static IEnumerator WaitForSecondRealtime(float time, Action callback)
        {
            yield return new WaitForSecondsRealtime(time);
            callback?.Invoke();
        }
    }

}
