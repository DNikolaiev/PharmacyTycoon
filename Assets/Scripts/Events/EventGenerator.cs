using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class EventGenerator : MonoBehaviour {
    
    [Range(0,1)] public float clickChoiceRatio;
    [SerializeField] float ratioChanger;
    [SerializeField] List<CustomEvent> events;
    [SerializeField] EventPanel ePanel;
    private void Start()
    {
        EventManager.StartListening(TurnController.OnTurnEvent, CalculateEventProbability);
    }
    private void CalculateEventProbability()
    {
        if ( (int)GameController.instance.time.date.month % 2==0)
        {
            GenerateEvent();
        }
    }
    private void GenerateEvent()
    {
        //ClickerEvent click = new ClickerEvent(); ChoiceEvent choice = new ChoiceEvent();
        float probability = Random.Range(0f, 1f);
        if(probability <= clickChoiceRatio)
        {
            clickChoiceRatio -= ratioChanger;
            if (GameController.instance.roomOverseer.GetAllSceneObjects().Count > 0)
            {
                ePanel.SetPanel((ClickerEvent)GetEvent<ClickerEvent>());
            }
            else ePanel.SetPanel((ChoiceEvent)GetEvent<ChoiceEvent>());
        }
        else if (probability > clickChoiceRatio)
        {
            clickChoiceRatio += ratioChanger;
            ePanel.SetPanel((ChoiceEvent)GetEvent<ChoiceEvent>());
        }
        clickChoiceRatio = Mathf.Clamp01(clickChoiceRatio);
    }
    private CustomEvent GetEvent<T>()
    {
        

        if (typeof(T) == typeof(ClickerEvent))
        {
           

            List<CustomEvent> clickEvents =
                events.
                Where(x => x.GetComponent<ClickerEvent>() != null).ToList();

            return (clickEvents.Count > 0) ? clickEvents[Random.Range(0, clickEvents.Count)] : null;

        }
        else if (typeof(T) == typeof(ChoiceEvent))
        {
           
            List<CustomEvent> choiceEvents =
                events.
                Where(x => x.GetComponent<ChoiceEvent>() != null).ToList();

            return (choiceEvents.Count > 0) ? choiceEvents[Random.Range(0, choiceEvents.Count)] : null;
        }
        
        else {  return null; }
    }
   
}
