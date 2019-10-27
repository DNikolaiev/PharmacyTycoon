using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Calendar : Panel, IPointerClickHandler
{
    public Text day;
    public Text month;
    public Text year;
    private GameDate date;
    private Animation anim;
    [SerializeField] AudioClip turnCalendarSound;
    [SerializeField] ParticleSystem turnCalendarParticle;
    private bool isShown;
    private void Awake()
    {
        anim = GetComponent<Animation>();
    }
    private void OnEnable()
    {

        EventManager.StartListening(TurnController.OnDayEvent, RefreshData);
        EventManager.StartListening("OnPause", Hide);
        EventManager.StartListening("OnUnPause", SetPanel);
        EventManager.StartListening(TurnController.OnTurnEvent, PlayNotification);
    }
    private void PlayNotification()
    {
        GameController.instance.audio.MakeSound(turnCalendarSound);
        ParticleSystem toInst = Instantiate(turnCalendarParticle, transform.parent);
        toInst.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        toInst.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
        toInst.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        toInst.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        toInst.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        toInst.Play();
    }
    private void OnDisable()
    {

        EventManager.StopListening(TurnController.OnDayEvent, RefreshData);
        EventManager.StopListening("OnPause", Hide);
        EventManager.StopListening("OnUnPause", Activate);
        EventManager.StopListening(TurnController.OnTurnEvent, PlayNotification);
    }
    private void Start()
    {
        Debug.Log(GameController.instance.time.date.day);

        RefreshData(GameController.instance.time.date);
    }
    public override void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Activate()
    {
       
        gameObject.SetActive(true);
    }
    public override void SetPanel()
    {
        Activate();
        day.text = date.day.ToString();
        month.text = date.month.ToString();
        year.text = date.year.ToString();
    }
    public void RefreshData(object date)
    {
        this.date = (GameDate)date;
        SetPanel();
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(anim);
        if (!isShown)
        {
            anim.Play("Calendar_Appear");
            isShown = true;
        }
        else
        {
            isShown = false;
            anim.Play("Calendar_Disappear");
        }
    }
}
