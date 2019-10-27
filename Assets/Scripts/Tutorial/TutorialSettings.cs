using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TutorialSettings : MonoBehaviour {

    public List<Tutorial> tutorials;
    public List<bool> GetTutorialsState()
    {
        List<bool> results = new List<bool>();
        foreach(Tutorial tutor in tutorials)
        {
            results.Add(tutor.isTutorialCompleted);
        }
        return results;
    }
    public bool GetTutorialState(string dialogueName)
    {
        Tutorial neededTutorial = tutorials.Where(t => t.dialogue.dialogueName == dialogueName).FirstOrDefault();
        return neededTutorial.isTutorialCompleted;
    }
    public void LoadTutorialState(List<bool> states)
    {
        for(int i=0; i<tutorials.Count; i++)
        {
            tutorials[i].isTutorialCompleted = states[i];
        }
    }
    public void DisableTutorial()
    {
        foreach(Tutorial t in tutorials)
        {
            t.isTutorialCompleted = true;
        }
    }
    public void EnableTutorial()
    {
        foreach (Tutorial t in tutorials)
        {
            if (t != tutorials[0])
            {
                t.isTutorialCompleted = false;
               
            }
            else t.dialogue.Initialize(-2);
        }
    }

}
