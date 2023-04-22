using System.Collections;
using AS.Toolbox.ScriptableObjects;
using UnityEngine;

namespace AS.Toolbox.Singletons
{
    public class GameState : SingletonMono<GameState>
    {
        [Header("General"), SerializeField]
        GameStateVar var;
        [SerializeField] GameStateSO entryState;
        [SerializeField] bool forceState;
        [Header("States"), SerializeField]
        GameStateSO initial;
        [SerializeField] GameStateSO home;
        [SerializeField] GameStateSO launch;
        [SerializeField] GameStateSO inGame;
        [SerializeField] GameStateSO gameOver;
        [SerializeField] GameStateSO station;
        [SerializeField] GameStateSO settings;

        public static GameStateVar Var => Instance.var;
        public static GameStateSO CurrentState => Instance.var.v;
        public static GameStateSO Initial => Instance.initial;
        public static GameStateSO Home => Instance.home;
        public static GameStateSO Launch => Instance.launch;
        public static GameStateSO InGame => Instance.inGame;
        public static GameStateSO GameOver => Instance.gameOver;
        public static GameStateSO Station => Instance.station;
        public static GameStateSO Settings => Instance.settings;

        protected override void OnAwake() => StartCoroutine(DelayedEntryState());
        public static void SetState(GameStateSO newGS) => Instance.var.SetValue(newGS);

        // we must delay setting the entry state so the state listeners are properly registered in the initial frame 
        IEnumerator DelayedEntryState()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            if (forceState) Instance.var.FinalizeSetValue(entryState);
            else SetState(entryState);
        }
    }
}
