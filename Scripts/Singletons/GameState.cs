using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolbox.ScriptableObjects;
using Toolbox.ScriptableObjects.Variables;
using Toolbox.Singletons;
using Toolbox.Utils;
using UnityEngine;

[Serializable]
public class GameStateDictionary : UnitySerializedDictionary<StateEnum, GameStateSO>
{
}

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

    protected override void OnAwake() => StartCoroutine(DelayedEntryState());
    public static void SetState(GameStateSO newGS) => Instance.variable.SetValue(newGS);
    public static void SetState(StateEnum newGS) => Instance.variable.SetValue(GetState(newGS));
    public static GameStateSO GetState(StateEnum gsEnum) => Instance.gameStates.Find(gs => gs.stateEnum == gsEnum);

    // we must delay setting the entry state so the state listeners are properly registered in the initial frame 
    IEnumerator DelayedEntryState()
    {
        yield return new WaitForEndOfFrame();
        SetState(entryState);
    }
}