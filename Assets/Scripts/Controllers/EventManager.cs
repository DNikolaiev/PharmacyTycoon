using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class ObjectEvent : UnityEvent<object> { }
public class EventManager : MonoBehaviour {


    private Dictionary<string, UnityEvent> eventDictionary;
    private Dictionary<string, ObjectEvent> objectEventDictionary; 
    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
        if(objectEventDictionary==null)
        {
            objectEventDictionary = new Dictionary<string, ObjectEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }
    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    public static void StartListening(string eventName, UnityAction<object> listener)
    {
       ObjectEvent thisEvent = null;
        if (instance.objectEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new ObjectEvent();
            thisEvent.AddListener(listener);
            instance.objectEventDictionary.Add(eventName, thisEvent);
        }
    }
    public static void StopListening(string eventName, UnityAction<object> listener)
    {
        if (eventManager == null) return;
        ObjectEvent thisEvent = null;
        if (instance.objectEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
    public static void TriggerEvent(string eventName, object date)
    {
        ObjectEvent thisEvent = null;
        if (instance.objectEventDictionary.TryGetValue(eventName, out thisEvent))
        {
           
                thisEvent.Invoke(date);
            
        }
    }
}
