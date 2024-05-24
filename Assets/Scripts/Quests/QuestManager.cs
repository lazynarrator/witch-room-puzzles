using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Управление квестами
/// </summary>
public class QuestManager : MonoBehaviour
{
    /// <summary>
    /// Квест в базовом виде
    /// </summary>
    [Serializable]
    public class BaseQuest
    {
        [SerializeField, Tooltip("Имя квеста")]
        public string name = "Quest";
        
        [SerializeField, Tooltip("Состояние квеста")]
        private QuestState state;
        
        [SerializeField, Tooltip("Тип квеста")]
        private QuestType questType;
        
        [SerializeField, Tooltip("Для выполнения нужно выдать итем"), ShowIf("questType", QuestType.GiveItem),
         ValueDropdown("ItemsList")]
        private Item giveItem;
        
        [SerializeField, Tooltip("Для выполнения нужно иметь итем"), ShowIf("questType", QuestType.NeedUnlockItem),
         ValueDropdown("ItemsList")]
        private Item unlockItem;
        
        [SerializeField, Tooltip("Для выполнения нужно дослушать диалог"), ShowIf("questType", QuestType.Conversation)]
        private string dialogueNode;
        
        [SerializeField, Tooltip("Выполнение условия"), ShowIf("questType", QuestType.Condition)]
        private UnityEvent condition;
        
        [SerializeField, Tooltip("Квесты, разблокирующиеся по завершению текущего")]
        [ValueDropdown("SetUnlockQuests", ExpandAllMenuItems = true)]
        [OnValueChanged(nameof(UnlockQuests), InvokeOnInitialize = true)]
        private List<string> unlockByComplete;
        [NonSerialized]
        private List<BaseQuest> unlockQuests = new List<BaseQuest>();
        
        [SerializeField, Tooltip("")]
        [ValueDropdown("SetUnlockQuests", ExpandAllMenuItems = true)]
        private List<string> completeByComplete;
        
        /// <summary>
        /// Состояние квеста
        /// </summary>
        public QuestState State
        {
            get => state;
            set => RefreshState(value);
        }
        
        /// <summary>
        /// Тип квеста
        /// </summary>
        public QuestType QuestType => questType;
        
        /// <summary>
        /// Инициализация с параметрами
        /// </summary>
        /// <param name="_newState">Состояние квеста</param>
        public void Init(QuestState _newState)
        {
            RefreshState(_newState);
        }
        
        /// <summary>
        /// Выполнить квест
        /// </summary>
        public void Execute()
        {
            switch (questType)
            {
                case QuestType.Condition:
                    condition?.Invoke();
                    break;
                case QuestType.GiveItem:
                    //Выдать итем
                    break;
                case QuestType.NeedUnlockItem:
                    //Забрать итем
                    break;
                case QuestType.Conversation:
                    YarnController.Instance.TryStartConversation(dialogueNode);
                    break;
                case QuestType.Puzzle:
                    break;
                default:
                    break;
            }
            
            RefreshState(QuestState.Complete);
            ExecuteUnlock();
        }
        
        /// <summary>
        /// Обновить состояние квеста
        /// </summary>
        /// <param name="_newState">Состояние квеста</param>
        private void RefreshState(QuestState _newState)
        {
            state = _newState;
            OnStateChanged?.Invoke(this);
        }
        
        /// <summary>
        /// Разблокировать смежные квесты
        /// </summary>
        private void ExecuteUnlock()
        {
            UnlockQuests();
            
            foreach (var unlock in unlockQuests)
            {
                unlock.RefreshState(unlock.QuestType == QuestType.Conversation ? QuestState.Pending : QuestState.Ready);
            }
            
            for (int i = 0; i < Instance.questsList.Count; i++)
            {
                for (int j = 0; j < completeByComplete.Count; j++)
                {
                    if (Instance.questsList[i].name == completeByComplete[j])
                    {
                        Instance.questsList[i].RefreshState(QuestState.Complete);
                    }
                }
            }
        }
        
        /// <summary>
        /// Задать фактический список квестов на разблокировку
        /// </summary>
        private void UnlockQuests()
        {
            unlockQuests.Clear();
            List<BaseQuest> list = Instance.questsList;
            
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < unlockByComplete.Count; j++)
                {
                    if (list[i].name == unlockByComplete[j])
                    {
                        unlockQuests.Add(list[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Воспроизвести повторяющийся/в ожидании квест
        /// </summary>
        public void Play()
        {
            switch (questType)
            {
                case QuestType.Conversation:
                    YarnController.Instance.TryStartConversation(dialogueNode);
                    break;
                case QuestType.Puzzle:
                    RefreshState(QuestState.Ready);
                    break;
                default:
                    break;
            }
        }

        #region Editor

        /// <summary>
        /// Установить список квестов на разблокировку
        /// </summary>
        /// <returns></returns>
        private IEnumerable SetUnlockQuests()
        {
            ValueDropdownList<string> drop = new ValueDropdownList<string>();
            
            for (int i = 0; i < Instance.questsList.Count; i++)
            {
                drop.Add(Instance.questsList[i].name);
            }
            
            IEnumerable treeView = drop;
            return treeView;
        }
        
        /// <summary>
        /// Установить список квестов на завершение
        /// </summary>
        /// <returns></returns>
        private IEnumerable SetCompleteQuests()
        {
            ValueDropdownList<string> drop = new ValueDropdownList<string>();
            
            for (int i = 0; i < Instance.questsList.Count; i++)
            {
                drop.Add(Instance.questsList[i].name);
            }
            
            IEnumerable treeView = drop;
            return treeView;
        }

        /// <summary>
        /// Массив доступных итемов (для выбора)
        /// </summary>
        /// <returns></returns>
        private Item[] ItemsList()
        {
            return ItemsController.Instance.GetItemsList();
        }

        #endregion Editor
    }

    #region FieldsProperties
    
    [SerializeField, Tooltip("Список квестов")]
    public List<BaseQuest> questsList = new List<BaseQuest>();
    
    private static QuestManager instance;
    public static QuestManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<QuestManager>();
            return instance;
        }
    }
    
    /// <summary>
    /// Изменилось состояние квеста
    /// </summary>
    public static event Action<BaseQuest> OnStateChanged;
    
    #endregion FieldsProperties

    #region Enums
    
    /// <summary>
    /// Режимы квеста
    /// </summary>
    public enum QuestState : byte
    {
        NotActive, //Квест не начинался
        Pending, //Квест начался и в ожидании выполнения
        Ready, //Квест готов к сдаче
        Complete //Квест завершен
    }
    
    /// <summary>
    /// Тип квеста
    /// </summary>
    public enum QuestType : byte
    {
        NoType,
        Condition,
        GiveItem,
        NeedUnlockItem,
        Conversation,
        Puzzle
    }
    
    #endregion Enums
    
    #region Methods
    
    /// <summary>
    /// Загрузка инфо о квестах
    /// </summary>
    public void LoadQuests()
    {
        //Здесь можно загружать данные из сохранения
    }
    
    /// <summary>
    /// Проверить квест на готовность к выполнению
    /// </summary>
    public void Check(string _questName)
    {
        for (int i = 0; i < questsList.Count; i++)
        {
            if (questsList[i].name == _questName)
            {
                if (questsList[i].State == QuestState.Pending)
                    questsList[i].Play();
                    
                if (questsList[i].State == QuestState.Ready)
                    questsList[i].Execute();
            }
        }
    }
    
    /// <summary>
    /// Проверить список квестов
    /// </summary>
    public void Check()
    {
        for (int i = 0; i < questsList.Count; i++)
        {
            if (questsList[i].State == QuestState.Ready)
                questsList[i].Execute();
        }
    }
    
    #endregion Methods
}