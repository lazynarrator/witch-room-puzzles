using System;
using System.Collections.Generic;

/// <summary>
/// Машина состояний
/// </summary>
public class LevelStateMachine
{
    private Dictionary<Type, ILevelState> states;
    private ILevelState currentState;

    public LevelStateMachine()
    {
        states = new Dictionary<Type, ILevelState>()
        {
            [typeof(LoadingLevelState)] = new LoadingLevelState(this),
            [typeof(InitializeLevelState)] = new InitializeLevelState(this)
        };
    }

    public void EnterIn<TState>() where TState : ILevelState
    {
        if (states.TryGetValue(typeof(TState), out ILevelState state))
        {
            currentState?.Exit();
            currentState = state;
            currentState.Enter();
        }
    }
}