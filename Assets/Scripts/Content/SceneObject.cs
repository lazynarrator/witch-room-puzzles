using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Объект, расположенный на сцене
/// </summary>
public class SceneObject : MonoBehaviour, IInteractable
{
    [SerializeField, Tooltip("Выполнять приближение")]
    private bool isZoomed = true;
    [SerializeField, Tooltip("Является ли пазлом")]
    private bool checkPuzzle = false;
    [SerializeField, ShowIf("checkPuzzle"), Tooltip("Компонент пазла")]
    private PuzzleBase puzzle;
    
    private QuestObject quest;
    private Animator animator;

    private void Start()
    {
        quest = GetComponent<QuestObject>();
        animator = GetComponent<Animator>();
        
        PuzzleBase.OnPuzzleUnlocked += OnPuzzleUnlocked;
    }

    private void OnDestroy()
    {
        PuzzleBase.OnPuzzleUnlocked -= OnPuzzleUnlocked;
    }

    private void OnMouseDown()
    {
        Interact();
    }

    /// <summary>
    /// Событие разблокировки пазла
    /// </summary>
    private void OnPuzzleUnlocked(PuzzleBase _puzzle, bool _forcedOpen)
    {
        if (_puzzle == puzzle && _forcedOpen)
        {
            CheckQuests();
        }
    }

    /// <summary>
    /// Выполнить взаимодействие
    /// </summary>
    public void Interact()
    {
        if (isZoomed && !CameraTransition.DynamicCamera.ZoomMode)
        {
            CameraTransition.DynamicCamera.ZoomIn(gameObject.transform);
        }
        else
        {
            CheckQuests();
            
            if (animator != null)
                animator.SetTrigger("Interact");
        }
    }
    
    /// <summary>
    /// Проверить, выполнено ли условие квеста/выполнить
    /// </summary>
    private void CheckQuests()
    {
        if (quest != null)
        {
            for (int i = 0; i < quest.baseQuests.Count; i++)
            {
                if (quest.baseQuests[i].QuestType != QuestManager.QuestType.Puzzle ||
                    quest.baseQuests[i].QuestType == QuestManager.QuestType.Puzzle && checkPuzzle && puzzle != null && puzzle.IsUnlock)
                {
                    QuestManager.Instance.Check(quest.baseQuests[i].name);
                }
            }
        }
    }
}