using System.Collections;
using UnityEngine;
public enum Month
{

    January, February,
    March, April, May,
    Juni, July, August,
    September, October, November,
    December
}

[System.Serializable]
public struct GameDate
{
    public int day;
    public Month month;
    public int year;
    public static GameDate CreateDate(Month month, int year, int day)
    {
        GameDate date = new GameDate();
        date.month = month;
        date.year = year;
        date.day = day;
        return date;
    }
    
}
public class TurnController : MonoBehaviour {
    public static readonly string OnTurnEvent = "OnTurnChanged";
    public static readonly string OnDayEvent = "OnDayChanged";
    public static readonly string OnYearEvent = "OnYearChanged";
    public bool isPaused;
    public GameDate date;
    public int turnDuration;
    public int DayDuration
    {
        get
        {

            return (int)turnDuration / 30;
        }
        
    }
    [SerializeField] float timer;
    public Tutorial tutorial;
    public GameObject tutorialPrefab;
    int[] daysInMonth = { 31, 28, 31, 31, 31, 31, 31, 31, 31, 31, 31, 31 };
    private void OnLoad()
    {
        EventManager.TriggerEvent(OnDayEvent, date);
        if (this != null)
        {
            StartCoroutine(TimeStart());
        }
    }
   
    public  IEnumerator TimeStart()
    {
       
        timer = date.day*DayDuration - 1;
        EventManager.TriggerEvent(OnDayEvent);
        while (!isPaused)
        {
            while (timer < turnDuration && !isPaused)
            {
                timer++;
                if (timer % DayDuration == 0)
                {
                    if (date.day < daysInMonth[(int)date.month])
                    {
                        date.day = (int)timer / (int)DayDuration;
                        EventManager.TriggerEvent(OnDayEvent, date) ;
                       
                    }
                }
                yield return new WaitForSecondsRealtime(1);
            }
            if (isPaused) break;
            EventManager.TriggerEvent(OnTurnEvent);
            timer = 0;
            StopAllCoroutines();
            StartCoroutine(TimeStart());
        }
        yield return null;
    }
    private void ShowTutorial(object date)
    {
        if(this.date.day==28)
        {
            if (!tutorial.isTutorialCompleted)
            {
                tutorial.StartTutorial();
                tutorialPrefab.gameObject.SetActive(true);
            }
        }
    }
    private void ChangeMonth()
    {
        
        if (date.month != Month.December)
            date.month++;
        else
        {
            date.month = Month.January;
            date.year++;
            EventManager.TriggerEvent(OnYearEvent);
            EventManager.TriggerEvent("OnSurviveYears",1);
        }
        date.day = 1;
    }
    public void Pause()
    {
        if (isPaused) return;
        isPaused = true;
        StopCoroutine(TimeStart());
        
        GameController.instance.StopAllCoroutines();
        EventManager.TriggerEvent("OnPause");
        
    }
    public void UnPause()
    {
        if (!isPaused) return;
        StopCoroutine(TimeStart());
        isPaused = false;
        StartCoroutine(TimeStart());
        GameController.instance.buttons.market.StartAllTimers();
        EventManager.TriggerEvent("OnUnPause");

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Pause();
        if (Input.GetKeyDown(KeyCode.U))
            UnPause();
    }
    private void Start()
    {
        SaveController.OnLoadEvent += OnLoad;
        UnPause();
    }
    private void OnEnable()
    {
        EventManager.StartListening(OnTurnEvent, ChangeMonth);
        EventManager.StartListening(OnDayEvent, ShowTutorial);
    }
    private void OnDisable()
    {
        EventManager.StopListening(OnTurnEvent, ChangeMonth);
        EventManager.StopListening(OnDayEvent, ShowTutorial);
    }
}
