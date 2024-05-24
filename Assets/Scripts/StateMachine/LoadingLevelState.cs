using UnityEngine;

/// <summary>
/// Загрузка уровня и ресурсов
/// </summary>
public class LoadingLevelState : ILevelState
{
    private readonly LevelStateMachine levelStateMachine;

    public LoadingLevelState(LevelStateMachine _levelStateMachine)
    {
        levelStateMachine = _levelStateMachine;
    }
    
    public void Enter()
    {
        Debug.Log("Enter loading");
        //Тут можно прочесть имеющееся сохранение, подгрузить итемы
        SceneContent.Instance.LoadRoom(); //Подгрузить инфо о комнате
        QuestManager.Instance.LoadQuests(); //Загрузить инфо о квестах
        levelStateMachine.EnterIn<InitializeLevelState>();
    }

    public void Exit()
    {
        Debug.Log("Exit loading");
    }
}