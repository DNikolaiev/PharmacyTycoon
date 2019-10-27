using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class TalentTree  {
    [FullSerializer.fsIgnore] public List<Talent> talents; // save unlocked state
    [FullSerializer.fsIgnore] public int[] map;
    public SavedUnlockmentsData savedData;
    public void OnLoad()
    {
        foreach (Talent t in talents)
        {
            if (savedData.data1.Contains(t.id))
            {
                t.canBeUnlocked = true;
            }
            if (savedData.data2.Contains(t.id))
            {
                t.isUnlocked = true;
            }
        }
    }
    public void OnSave()
    {
        List<int> areUnlocked = new List<int>();
        List<int> canBeUnlocked = new List<int>();
        foreach (Talent t in talents.Where(t => t.isUnlocked).ToList())
        {
            if(!areUnlocked.Contains(t.id))
            areUnlocked.Add(t.id);
        }
        foreach (Talent t in talents.Where(t => t.canBeUnlocked).ToList())
        {
            if (!canBeUnlocked.Contains(t.id))
                canBeUnlocked.Add(t.id);
        }
        savedData = new SavedUnlockmentsData(canBeUnlocked, areUnlocked);
    }
}
