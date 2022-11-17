using System;
using Toolbox.ScriptableObjects;
using Toolbox.ScriptableObjects.Variables;
using Toolbox.Singletons;
using Toolbox.Utils;
using UnityEngine.Serialization;

[Serializable]
public class GameStateDictionary : UnitySerializedDictionary<State, GameStateSO>
{
}

public class GameState : SingletonMono<GameState>
{
    public State entryState = State.Home;
    [FormerlySerializedAs("currentState")] public GameStateVariable variable;
    public GameStateDictionary gameStateDict;

    public static GameStateSO CurrentState => Instance.variable.v;
    public static GameStateSO InGame => GetState(State.InGame);
    public static GameStateSO Home => GetState(State.Home);
    public static GameStateSO GameOver => GetState(State.GameOver);
    public static GameStateSO Settings => GetState(State.Settings);

    protected override void OnAwake() => SetState(entryState);
    public static void SetState(GameStateSO newGS) => Instance.variable.SetValue(newGS);
    public static void SetState(State newGS) => Instance.variable.SetValue(GetState(newGS));
    public static GameStateSO GetState(State gs) => Instance.gameStateDict[gs];
}