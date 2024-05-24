using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

/// <summary>
/// Головоломка с замком
/// </summary>
public class PuzzleLock : PuzzleBase
{
    [SerializeField, Tooltip("Корректный код головоломки")]
    private int[] correctCode;
    [SerializeField, Tooltip("Элементы с кодами головоломки")]
    private List<PuzzleLockCode> codes;
    
    private void Start()
    {
        PuzzleLockCode.OnPuzzleLockChanged += OnPuzzleLockChanged;
    }
    
    private void OnDestroy()
    {
        PuzzleLockCode.OnPuzzleLockChanged -= OnPuzzleLockChanged;
    }
    
    /// <summary>
    /// Cмена (поворот) одного элемента замка
    /// </summary>
    private void OnPuzzleLockChanged(PuzzleLock _puzzle)
    {
        if (_puzzle != this)
            return;
        
        bool isCorrect = false;
        IsUnlock = false;
        
        for (int i = 0; i < codes.Count; i++)
        {
            if (codes[i].CurrentNumber == correctCode[i])
            {
                isCorrect = true;
            }
            else
            {
                isCorrect = false;
                break;
            }
        }
        
        if (isCorrect)
        {
            Unlock(this, true);
        }
    }
    
#if UNITY_EDITOR
    
    private string getCodesName = "Load codes";
    
    [Button("$getCodesName")]
    public void GetCodesButton()
    {
        if (GetCodes())
            Debug.Log(getCodesName + ": Codes loaded!");
        else
            Debug.LogWarning(getCodesName + ": No <PuzzleLockCode> component in child!");
    }
    
    /// <summary>
    /// Получить элементы головоломки
    /// </summary>
    /// <returns></returns>
    private bool GetCodes()
    {
        codes.Clear();
        
        foreach (var code in GetComponentsInChildren<PuzzleLockCode>())
        {
            codes.Add(code);
        }
        
        return codes.Count > 0;
    }
    
#endif
}