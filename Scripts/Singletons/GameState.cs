using System;
using Toolbox.ScriptableObjects;
using Toolbox.ScriptableObjects.Variables;
using Toolbox.Singletons;
using UnityEngine.Serialization;

[Serializable]
public class GameStateDictionary : UnitySerializedDictionary<GameStateEnum, GameStateSO>
{
}

public class GameState : SingletonMono<GameState>
{
    public GameStateEnum entryState = GameStateEnum.Home;
    [FormerlySerializedAs("currentState")] public GameStateVariable variable;
    public GameStateDictionary gameStateDict;

    public static GameStateSO CurrentState => Instance.variable.v;
    public static GameStateSO InGame => GetState(GameStateEnum.InGame);
    public static GameStateSO Home => GetState(GameStateEnum.Home);
    public static GameStateSO GameOver => GetState(GameStateEnum.GameOver);
    public static GameStateSO Settings => GetState(GameStateEnum.Settings);

    protected override void OnAwake() => SetState(entryState);
    public static void SetState(GameStateSO newGS) => Instance.variable.SetValue(newGS);
    public static void SetState(GameStateEnum newGS) => Instance.variable.SetValue(GetState(newGS));
    public static GameStateSO GetState(GameStateEnum gs) => Instance.gameStateDict[gs];
}