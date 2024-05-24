using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управление затемнением на экране
/// </summary>
public class UIShadow : MonoBehaviour
{
    [SerializeField, Tooltip("Скорость изменения прозрачности в секундах")]
    private float transparencyTime = 1.0f;
    
    private Image image;
    private float currentTransparency = 1.0f;
    private float timer = 0f;
    private bool executed = false;
    
    public static event Action OnUIShadowEnd;

    /// <summary>
    /// Запуск эффекта спадающего затемнения
    /// </summary>
    public void StartShadow()
    {
        executed = true;
        timer = 0f;
        currentTransparency = 1.0f;
    }

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void OnDisable()
    {
        currentTransparency = 1.0f;
    }

    private void FixedUpdate()
    {
        if (executed)
        {
            timer += Time.deltaTime; // Увеличиваем таймер на прошедшее время
            currentTransparency = 1.0f - Mathf.Clamp01(timer / transparencyTime); // Рассчитываем текущую прозрачность в диапазоне от 1 до 0
            Color imageColor = image.color;
            imageColor.a = currentTransparency; // Применяем текущую прозрачность к цвету изображения
            image.color = imageColor;
            
            if (currentTransparency <= 0.01f)
            {
                executed = false;
                gameObject.SetActive(false); // Отключение, когда прозрачность близка к 0
                OnUIShadowEnd?.Invoke();
            }
        }
    }
}