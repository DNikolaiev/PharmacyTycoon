using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ChoiceEvent : CustomEvent
{
    
    public string choice1, choice2;
    public string result1, result2;
    public List<UnityEvent> actionOne;
    public List<UnityEvent> actionTwo;
    public override void FireEvent()
    {
        if (actionOne.Count>0)
        {
            foreach(UnityEvent e in actionOne)
            {
                e.Invoke();
            }
        }
           
            
    }
    public void FireSecondEvent()
    {
        if (actionTwo.Count > 0)
        {
            foreach (UnityEvent e in actionTwo)
            {
                e.Invoke();
            }
        }
    }
    
}
