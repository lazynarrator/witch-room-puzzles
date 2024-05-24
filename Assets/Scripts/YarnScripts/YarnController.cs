using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Управление диалогами
/// </summary>
public class YarnController : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    private bool isCurrentConversation = false;
    
    private static YarnController instance;
    public static YarnController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<YarnController>();
            return instance;
        }
    }
    
    #region Base
    
    private void Awake()
    {
        InitDialogue();
        UIShadow.OnUIShadowEnd += GetQuestDialog;
    }
    
    private void OnDestroy()
    {
        dialogueRunner.onDialogueComplete.RemoveAllListeners();
        UIShadow.OnUIShadowEnd -= GetQuestDialog;
    }
    
    #endregion Base
    
    #region Init
    
    /// <summary>
    /// Действия с диалогами при запуске игры (через стейт-машину)
    /// </summary>
    public void Init()
    {
        InitDialogue();
    }
    
    /// <summary>
    /// Определить диалоговый компонент
    /// </summary>
    private void InitDialogue()
    {
        if (dialogueRunner == null)
        {
            dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        }
    }
    
    /// <summary>
    /// Проверить доступный диалог в менеджере квестов
    /// </summary>
    private void GetQuestDialog()
    {
        QuestManager.Instance.Check();
    }
    
    #endregion Init
    
    #region Yarn
    
    /// <summary>
    /// Попробовать запустить диалог
    /// </summary>
    /// <param name="_node">Узел диалога</param>
    public void TryStartConversation(string _node)
    {
        if (!dialogueRunner.IsDialogueRunning)
        {
            StartConversation(_node);
        }
    }
    
    /// <summary>
    /// Запустить диалог
    /// </summary>
    /// <param name="_node">Узел диалога</param>
    private void StartConversation(string _node)
    {
        Debug.Log($"Started conversation with {name}.");
        isCurrentConversation = true;
        dialogueRunner.StartDialogue(_node);
    }
    
    /// <summary>
    /// Завершить диалог
    /// </summary>
    private void EndConversation()
    {
        if (isCurrentConversation)
        {
            isCurrentConversation = false;
            Debug.Log($"End conversation with {name}.");
        }
    }
    
    #endregion Yarn
}