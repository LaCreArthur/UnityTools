using System;
using System.Collections;
using System.Collections.Generic;
using AS.Toolbox.ScriptableObjects;
using AS.Toolbox.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AS.Toolbox.Singletons
{
    [Serializable]
    public class GameStateDictionary : UnitySerializedDictionary<StateEnum, GameStateSO> {}

    public class GameState : SingletonMono<GameState>
    {
        public StateEnum entryState = StateEnum.Home;
        public GameStateVariable variable;
        [AssetList(AutoPopulate = true)]
        public List<GameStateSO> gameStates;

        public static GameStateSO CurrentState => Instance.variable.v;
        public static GameStateSO Login => GetState(StateEnum.Login);
        public static GameStateSO Home => GetState(StateEnum.Home);
        public static GameStateSO Settings => GetState(StateEnum.Settings);
        public static GameStateSO InGame => GetState(StateEnum.InGame);
        public static GameStateSO GameOver => GetState(StateEnum.GameOver);
        public static GameStateSO LevelSelection => GetState(StateEnum.LevelSelection);

        protected override void OnAwake() => StartCoroutine(DelayedEntryState());
        public static void SetState(GameStateSO newGS) => Instance.variable.v = newGS;
        public static void SetState(StateEnum newGS) => Instance.variable.v = GetState(newGS);
        public static GameStateSO GetState(StateEnum gsEnum) => Instance.gameStates.Find(gs => gs.stateEnum == gsEnum);

        // we must delay setting the entry state so the state listeners are properly registered in the initial frame 
        IEnumerator DelayedEntryState()
        {
            yield return new WaitForEndOfFrame();
            SetState(entryState);
        }
    }
}