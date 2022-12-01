using System;
using System.Collections;
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
    public GameStateDictionary gameStateDict;

    public static GameStateSO CurrentState => Instance.variable.v;
    public static GameStateSO InGame => GetState(StateEnum.InGame);
    public static GameStateSO Home => GetState(StateEnum.Home);
    public static GameStateSO GameOver => GetState(StateEnum.GameOver);
    public static GameStateSO Settings => GetState(StateEnum.Settings);

    protected override void OnAwake() => StartCoroutine(DelayedEntryState());
    public static void SetState(GameStateSO newGS) => Instance.variable.SetValue(newGS);
    public static void SetState(StateEnum newGS) => Instance.variable.SetValue(GetState(newGS));
    public static GameStateSO GetState(StateEnum gs) => Instance.gameStateDict[gs];

    // we must delay setting the entry state so the state listeners are properly registered in the initial frame 
    IEnumerator DelayedEntryState()
    {
        yield return new WaitForEndOfFrame();
        SetState(entryState);
    }
}