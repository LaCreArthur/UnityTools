using UnityEngine;

namespace AS.Toolbox.Singletons.Audio
{
    public class Sounds : SingletonMono<Sounds>
    {
        [SerializeField] SoundSO audioEnabled;
        [SerializeField] SoundSO click;
        [SerializeField] SoundSO flap;
        [SerializeField] SoundSO takeoff;
        [SerializeField] SoundSO unlock;
        [SerializeField] SoundSO wow;
        [SerializeField] SoundSO swipe;

        public static SoundSO AudioEnabled => Instance.audioEnabled;
        public static SoundSO Click => Instance.click;
        public static SoundSO Flap => Instance.flap;
        public static SoundSO Takeoff => Instance.takeoff;
        public static SoundSO Unlock => Instance.unlock;
        public static SoundSO Wow => Instance.wow;
        public static SoundSO Swipe => Instance.swipe;
    }
}
