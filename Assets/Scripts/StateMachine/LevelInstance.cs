using UnityEngine;

/// <summary>
/// Старт уровня, entry point
/// </summary>
public class LevelInstance : MonoBehaviour
{
    private LevelStateMachine levelStateMachine;

    private void Awake()
    {
        levelStateMachine = new LevelStateMachine();
        levelStateMachine.EnterIn<LoadingLevelState>();
    }
}