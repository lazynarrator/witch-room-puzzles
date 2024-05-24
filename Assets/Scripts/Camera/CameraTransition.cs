using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Переключение вида камеры
/// </summary>
public class CameraTransition : MonoBehaviour
{
    [SerializeField, Tooltip("Камера режима приближения")]
    private CinemachineVirtualCameraBase zoomCam;
    [SerializeField, Tooltip("Кнопка выхода из режима приближения")]
    private Button backButton;
    [SerializeField, Tooltip("UI затемнения")]
    private UIShadow shadow;
    [SerializeField, Tooltip("Число, на которое повысится приоритет камеры")]
    private int priorityBoostAmount = 10;
    [SerializeField, Tooltip("Виртуальные камеры сцены")]
    private List<CinemachineVirtualCameraBase> cameras = new List<CinemachineVirtualCameraBase>();
    
    private bool boosted = false; //Повышен приоритет данной камеры
    
    /// <summary>
    /// Статус режима приближения: true - приближен, false - отдален
    /// </summary>
    public bool ZoomMode { get; private set; } = false;

    /// <summary>
    /// Камера приблизилась
    /// </summary>
    public static event Action OnZoomIn;
    
    /// <summary>
    /// Камера отдалилась
    /// </summary>
    public static event Action OnZoomOut;

    /// <summary>
    /// Камера, выполняющая приближение
    /// </summary>
    public static CameraTransition DynamicCamera => FindObjectOfType<CameraTransition>();

    /// <summary>
    /// Перемещение камер с эффектом затемнения
    /// </summary>
    public void SwitchWithShadow()
    {
        shadow.gameObject.SetActive(true);
        shadow.StartShadow();
        SetActivity(false);
        StartCoroutine(CamerasOn());
    }

    /// <summary>
    /// Приблизить камеру
    /// </summary>
    public void ZoomIn(Transform _target)
    {
        zoomCam.Follow = _target;
        ZoomMode = true;
        backButton.gameObject.SetActive(ZoomMode);
        OnZoomIn?.Invoke();
    }

    /// <summary>
    /// Установить активность виртуальных камер
    /// </summary>
    /// <param name="_active">Активность камер: false - выключить, true - включить</param>
    private void SetActivity(bool _active)
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].gameObject.SetActive(_active);
        }
    }

    /// <summary>
    /// Корутина включения камер в следующем кадре
    /// </summary>
    /// <returns></returns>
    private IEnumerator CamerasOn()
    {
        yield return null;
        SetActivity(true);
    }

    /// <summary>
    /// Отдалить камеру
    /// </summary>
    private void ZoomOut()
    {
        ZoomMode = false;
        backButton.gameObject.SetActive(ZoomMode);
        OnZoomOut?.Invoke();
    }

    private void Start()
    {
        backButton.onClick.AddListener(ZoomOut);
        backButton.gameObject.SetActive(ZoomMode);
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        if (zoomCam != null)
        {
            if (ZoomMode)
            {
                if (!boosted)
                {
                    zoomCam.Priority += priorityBoostAmount;
                    boosted = true;
                }
            }
            else if (boosted)
            {
                zoomCam.Priority -= priorityBoostAmount;
                boosted = false;
            }
        }
    }
    
#if UNITY_EDITOR

    private readonly string cameraButtonName = "Get cameras";

    [Button("$cameraButtonName")]
    public void GetCameraButton()
    {
        GetCameraList();
        Debug.Log(cameraButtonName + ": Cameras added!");
    }
    
    private void GetCameraList()
    {
        cameras.Clear();
        CinemachineVirtualCameraBase[] cams = FindObjectsByType<CinemachineVirtualCameraBase>(FindObjectsSortMode.None);
        foreach (var cam in cams)
            cameras.Add(cam);
    }

#endif
    
}