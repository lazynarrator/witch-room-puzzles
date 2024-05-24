using System;
using UnityEngine;

/// <summary>
/// Базовая головоломка
/// </summary>
public class PuzzleBase : MonoBehaviour
{
    /// <summary>
    /// Головоломка разблокирована (можно выполнять дальнейшие действия)
    /// </summary>
    public bool IsUnlock { get; protected set; }

    /// <summary>
    /// Событие разблокировки головоломки
    /// </summary>
    public static event Action<PuzzleBase, bool> OnPuzzleUnlocked;

    /// <summary>
    /// Разблокирование головоломки
    /// </summary>
    /// <param name="_puzzle">Конкретная головоломка</param>
    /// <param name="_forcedOpen">true - открывается без дальнейших действий, false - требуется действие для открытия</param>
    protected void Unlock(PuzzleBase _puzzle, bool _forcedOpen)
    {
        IsUnlock = true;
        OnPuzzleUnlocked?.Invoke(_puzzle, _forcedOpen);
    }
}