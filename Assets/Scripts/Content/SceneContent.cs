using UnityEngine;

/// <summary>
/// Содержимое сцены/комнаты
/// </summary>
public class SceneContent : MonoBehaviour
{
    private static SceneContent instance;
    public static SceneContent Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<SceneContent>();
            return instance;
        }
    }

    /// <summary>
    /// Загрузка комнаты
    /// </summary>
    public void LoadRoom()
    {
        CameraTransition.DynamicCamera.SwitchWithShadow();
        //Сюда можно добавить прочее необходимое при загрузке комнаты
    }
}