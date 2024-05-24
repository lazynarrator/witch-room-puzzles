using UnityEngine;

/// <summary>
/// Головоломка с часами
/// </summary>
public class PuzzleClock : PuzzleBase
{
    [SerializeField, Tooltip("Минутная стрелка")]
    private Transform minuteHand;
    [SerializeField, Tooltip("Часовая стрелка")]
    private Transform hourHand;
    
    [Header("START TIME")]
    [SerializeField, Range(0, 11), Tooltip("Стартовое значение часовой стрелки")]
    private int hourStart;
    [SerializeField, Range(0, 59), Tooltip("Стартовое значение минутной стрелки")]
    private int minuteStart;
    
    [Header("UNLOCK TIME")]
    [SerializeField, Range(0, 11), Tooltip("Необходимое значение часовой стрелки")]
    private int hourUnlock;
    [SerializeField, Range(0, 59), Tooltip("Необходимое значение минутной стрелки")]
    private int minuteUnlock;
    
    private Camera puzzleCamera;
    private PuzzleTime puzzleTime;
    private float previousAngle;
    private bool isDrag = false;
    
    /// <summary>
    /// Счётчик времени
    /// </summary>
    private class PuzzleTime
    {
        /// <summary>
        /// Часы
        /// </summary>
        public int Hours { get; private set; }
        
        /// <summary>
        /// Минуты в градусах (не в секундах - для уменьшения погрешности)
        /// </summary>
        public int Degrees { get; private set; }
        
        private readonly bool[] forwardPoints, backPoints; //контрольные точки вращения стрелки
        
        /// <summary>
        /// Создать счётчик времени
        /// </summary>
        /// <param name="_hours">Стартовое значение часовой стрелки</param>
        /// <param name="_degrees">Стартовое значение минутной стрелки</param>
        public PuzzleTime(int _hours, int _degrees)
        {
            Hours = _hours;
            Degrees = _degrees;
            
            forwardPoints = new[] { false, false };
            backPoints = new[] { false, false };
        }
        
        /// <summary>
        /// Обновить положение стрелок
        /// </summary>
        /// <param name="_forward">Направление по/против часовой стрелки</param>
        /// <param name="_angle">Угол поворота минутной стрелки</param>
        public void ChangeTime(bool _forward, int _angle)
        {
            Degrees = _angle;
            
            if (_forward)
            {
                //Проверка контрольных точек
                if (Degrees > 90f)
                    forwardPoints[0] = true;
                if (Degrees > 270f)
                    forwardPoints[1] = true;
                
                //Переход нулевой отметки
                if (forwardPoints[0] && forwardPoints[1] && Degrees < 10f)
                {
                    Hours++;
                    if (Hours > 11)
                        Hours = 0;
                    forwardPoints[0] = false;
                    forwardPoints[1] = false;
                }
            }
            else
            {
                //Проверка контрольных точек
                if (Degrees < 270f)
                    backPoints[0] = true;
                if (Degrees < 90f)
                    backPoints[1] = true;
                
                //Переход нулевой отметки
                if (backPoints[0] && backPoints[1] && (Degrees > 350f))
                {
                    Hours--;
                    if (Hours < 0)
                        Hours = 11;
                    backPoints[0] = false;
                    backPoints[1] = false;
                }
            }
        }
    }
    
    private void Start()
    {
        Init();
    }
    
    private void OnMouseDrag()
    {
        isDrag = true;
    }
    
    private void OnMouseUp()
    {
        isDrag = false;
    }
    
    private void FixedUpdate()
    {
        if (isDrag)
            Drag();
    }
    
    /// <summary>
    /// Инициализация
    /// </summary>
    private void Init()
    {
        puzzleCamera = Camera.main;
        puzzleTime = new PuzzleTime(hourStart, minuteStart);
        //360/60 (6f) для минутной стрелки; 360/12 (30f) + 360/12/60 (0.5f) - для часовой стрелки
        minuteHand.Rotate(0f, 360f - 6f * minuteStart, 0f);
        hourHand.Rotate(0f, 360f - (30f * hourStart + 0.5f * minuteStart), 0f);
    }
    
    /// <summary>
    /// Логика перетаскивания стрелки
    /// </summary>
    private void Drag()
    {
        IsUnlock = false;
        
        Vector3 mousePose = puzzleCamera.ScreenToViewportPoint(Input.mousePosition);
        Vector3 mouseOffset = mousePose - new Vector3(0.5f, 0.5f, 0f);
        
        float currentAngle = 360f - minuteHand.localRotation.eulerAngles.y;
        int roundAngle = (int)Mathf.Round(currentAngle);
        if (roundAngle == 360)
            roundAngle = 0;
        
        float delta = previousAngle - currentAngle;
        if ((delta <= 0 && delta > -180f) || delta > 180f)
        {
            puzzleTime.ChangeTime(true, roundAngle);
        }
        else
        {
            if (currentAngle <= previousAngle || delta < -180f)
            {
                puzzleTime.ChangeTime(false, roundAngle);
            }
        }
        
        float nextAngle = Mathf.Atan2(mouseOffset.y, mouseOffset.x) * Mathf.Rad2Deg - 90f;
        minuteHand.localRotation = Quaternion.AngleAxis(nextAngle, Vector3.up);
        if (puzzleTime.Degrees > 0f && puzzleTime.Degrees < 360f)
        {
            float hourAngle = 360f - (30f * puzzleTime.Hours + 30f / 360f * puzzleTime.Degrees);
            hourHand.localRotation = Quaternion.AngleAxis(hourAngle, Vector3.up);
        }
        
        previousAngle = currentAngle;
        
        if (puzzleTime.Hours == hourUnlock && Mathf.Abs(roundAngle / 6f - minuteUnlock) < 1f)
        {
            Unlock(this, false);
        }
    }
}