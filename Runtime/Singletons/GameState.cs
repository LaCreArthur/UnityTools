using System.Collections;
using System.Collections.Generic;
using AS.Toolbox.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AS.Toolbox.Singletons
{
    public class GameState : SingletonMono<GameState>
    {
        public GameStateVar var;
        [SerializeField] StateEnum entryState = StateEnum.Home;
        [AssetList(AutoPopulate = true), SerializeField]
        List<GameStateSO> gameStates;
        [SerializeField] bool forceState;

        public static GameStateSO CurrentState => Instance.var.v;
        public static GameStateSO Home => GetState(StateEnum.Home);
        public static GameStateSO Settings => GetState(StateEnum.Settings);
        public static GameStateSO InGame => GetState(StateEnum.InGame);
        public static GameStateSO Launch => GetState(StateEnum.Launch);
        public static GameStateSO GameOver => GetState(StateEnum.GameOver);
        public static GameStateSO Station => GetState(StateEnum.Station);

        protected override void OnAwake() => StartCoroutine(DelayedEntryState());
        public static void SetState(GameStateSO newGS) => Instance.var.SetValue(newGS);
        public static void SetState(StateEnum newGS) => Instance.var.SetValue(GetState(newGS));
        public static GameStateSO GetState(StateEnum gsEnum) => Instance.gameStates.Find(gs => gs.stateEnum == gsEnum);

        // we must delay setting the entry state so the state listeners are properly registered in the initial frame 
        IEnumerator DelayedEntryState()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            if (forceState) Instance.var.FinalizeSetValue(GetState(entryState));
            else SetState(entryState);
        }
    }
}
