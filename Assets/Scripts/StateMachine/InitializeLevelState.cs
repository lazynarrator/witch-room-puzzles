using UnityEngine;

/// <summary>
/// Инициализация уровня и сервисов
/// </summary>
public class InitializeLevelState : ILevelState
{
    private readonly LevelStateMachine levelStateMachine;
    
    public InitializeLevelState(LevelStateMachine _levelStateMachine)
    {
        levelStateMachine = _levelStateMachine;
    }
    
    public void Enter()
    {
        Debug.Log("Enter initialize");
        YarnController.Instance.Init(); //Запустить действия с диалогами
    }
    
    public void Exit()
    {
        Debug.Log("Exit initialize");
    }
}