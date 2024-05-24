using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Элемент головоломки с замком
/// </summary>
public class PuzzleLockCode : PuzzleBase
{
    [SerializeField, Range(3, 20), Tooltip("Количество граней")]
    private int facesCount = 10;
    
    [SerializeField, Range(1, 15), Tooltip("Скорость прокрутки элемента")]
    private int rotationSpeed = 5;
    
    [SerializeField, Tooltip("Число, соответствующее нулевому повороту")]
    private int zeroPoseNumber;
    
    [SerializeField, ReadOnly, Tooltip("Подсмотреть текущее число в инспекторе")]
    private int currentNumber;
    
    private bool lockCoroutine = false; //блок во время вращения
    private PuzzleLock puzzle;
    
    /// <summary>
    /// Событие смены (поворота) одного элемента замка
    /// </summary>
    public static event Action<PuzzleLock> OnPuzzleLockChanged;
    
    /// <summary>
    /// Текущая цифра элемента замка
    /// </summary>
    public int CurrentNumber => currentNumber;
    
    private void OnMouseDown()
    {
        if (!lockCoroutine)
            StartCoroutine(Rotate());
    }
    
    private void Start()
    {
        Init();
    }
    
    /// <summary>
    /// Инициализация
    /// </summary>
    private void Init()
    {
        puzzle = GetComponentInParent<PuzzleLock>();
        SetNumber(Random.Range(0, facesCount));
    }
    
    /// <summary>
    /// Установить поворот на конкретный номер
    /// </summary>
    private void SetNumber(int _number)
    {
        transform.Rotate(0f, 360f * (_number - zeroPoseNumber) / facesCount, 0f);
        currentNumber = _number;
    }
    
    /// <summary>
    /// Повернуть на один оборот
    /// </summary>
    private IEnumerator Rotate()
    {
        lockCoroutine = true;
        
        for (int i = 0; i < rotationSpeed; i++)
        {
            transform.Rotate(0f, 360f / facesCount / rotationSpeed, 0f);
            yield return new WaitForSeconds(0.05f);
        }
        
        lockCoroutine = false;
        
        if (currentNumber < facesCount - 1)
            currentNumber++;
        else
            currentNumber = 0;

        if (puzzle != null)
            OnPuzzleLockChanged?.Invoke(puzzle);
    }
}