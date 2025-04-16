using UnityEngine;

namespace AS.Toolbox.Singletons.Audio
{
    public class Sounds : SingletonMono<Sounds>
    {
        [SerializeField] SoundSO audioEnabled;
        [SerializeField] SoundSO buildingUpgrade;
        [SerializeField] SoundSO zoneUnlocked;
        [SerializeField] SoundSO click;
        [SerializeField] SoundSO crash;
        [SerializeField] SoundSO flap;
        [SerializeField] SoundSO helmetLost;
        [SerializeField] SoundSO repair;
        [SerializeField] SoundSO swipe;
        [SerializeField] SoundSO takeoff;
        [SerializeField] SoundSO bark;
        [SerializeField] SoundSO unlock;

        public static SoundSO AudioEnabled => Instance.audioEnabled;
        public static SoundSO BuildingUpgrade => Instance.buildingUpgrade;
        public static SoundSO ZoneUnlocked => Instance.zoneUnlocked;
        public static SoundSO Click => Instance.click;
        public static SoundSO Crash => Instance.crash;
        public static SoundSO Flap => Instance.flap;
        public static SoundSO HelmetLost => Instance.helmetLost;
        public static SoundSO Repair => Instance.repair;
        public static SoundSO Swipe => Instance.swipe;
        public static SoundSO Takeoff => Instance.takeoff;
        public static SoundSO Bark => Instance.bark;
        public static SoundSO Unlock => Instance.unlock;
    }
}
