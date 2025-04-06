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
        [SerializeField] SoundSO swipe;
        [SerializeField] SoundSO crash;
        [SerializeField] SoundSO repair;
        [SerializeField] SoundSO buildingUpgrade;

        public static SoundSO AudioEnabled => Instance.audioEnabled;
        public static SoundSO Click => Instance.click;
        public static SoundSO Flap => Instance.flap;
        public static SoundSO Takeoff => Instance.takeoff;
        public static SoundSO Unlock => Instance.unlock;

        public static SoundSO Swipe => Instance.swipe;
        public static SoundSO Crash => Instance.crash;
        public static SoundSO Repair => Instance.repair;
        public static SoundSO BuildingUpgrade => Instance.buildingUpgrade;
    }
}
