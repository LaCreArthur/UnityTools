using System;
using Toolbox.ScriptableObjects;
using Toolbox.ScriptableObjects.Variables;
using Toolbox.Singletons;
using Toolbox.Utils;

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

    protected override void OnAwake() => SetState(entryState);
    public static void SetState(GameStateSO newGS) => Instance.variable.SetValue(newGS);
    public static void SetState(StateEnum newGS) => Instance.variable.SetValue(GetState(newGS));
    public static GameStateSO GetState(StateEnum gs) => Instance.gameStateDict[gs];
}