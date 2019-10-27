using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Dialogue")]
public class Dialogue:ScriptableObject  {
    public string dialogueName;
    public List<string> phrases;
    [SerializeField] private int counter = -1;
    public void Initialize(int start=-1)
    {
        counter = start;
    }
    public void Return()
    {
        counter--;
    }
    public string GetNextPhrase()
    {
        if (counter < phrases.Count-1)
        {
            counter++;
            return phrases[counter];
        }
        else
        {
            return "Final";
        }
    }

}
