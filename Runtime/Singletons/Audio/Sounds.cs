using UnityEngine;

namespace AS.Toolbox.Singletons.Audio
{
    public class Sounds : SingletonMono<Sounds>
    {
        [SerializeField] SoundSO asteroid;
        [SerializeField] SoundSO audioEnabled;
        [SerializeField] SoundSO axe;
        [SerializeField] SoundSO cash;
        [SerializeField] SoundSO click;
        [SerializeField] SoundSO coin;
        [SerializeField] SoundSO engine;
        [SerializeField] SoundSO fart;
        [SerializeField] SoundSO flap;
        [SerializeField] SoundSO gas;
        [SerializeField] SoundSO gasGold;
        [SerializeField] SoundSO takeoff;
        [SerializeField] SoundSO unlock;
        [SerializeField] SoundSO whale;
        [SerializeField] SoundSO wow;
        [SerializeField] SoundSO musique;

        public static SoundSO Asteroid => Instance.asteroid;
        public static SoundSO AudioEnabled => Instance.audioEnabled;
        public static SoundSO Axe => Instance.axe;
        public static SoundSO Cash => Instance.cash;
        public static SoundSO Click => Instance.click;
        public static SoundSO Coin => Instance.coin;
        public static SoundSO Engine => Instance.engine;
        public static SoundSO Fart => Instance.fart;
        public static SoundSO Flap => Instance.flap;
        public static SoundSO Gas => Instance.gas;
        public static SoundSO GasGold => Instance.gasGold;
        public static SoundSO Takeoff => Instance.takeoff;
        public static SoundSO Unlock => Instance.unlock;
        public static SoundSO Whale => Instance.whale;
        public static SoundSO Wow => Instance.wow;
        public static SoundSO Musique => Instance.musique;
    }
}
