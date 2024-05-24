using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Упрощенный переход между сценами
/// </summary>
public class UIPreloader : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}