using System;
using UnityEngine;

namespace AS.Toolbox.Utils
{
    public class CoroutineRunner : MonoBehaviour
    {
        static CoroutineRunner Instance { get; set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void WaitForEndOfFrame(Action callback) => Instance.StartCoroutine(Coroutines.WaitForEndOfFrame(callback));
        public static void WaitForSecond(float time, Action callback) => Instance.StartCoroutine(Coroutines.WaitForSecond(time, callback));
    }
}
