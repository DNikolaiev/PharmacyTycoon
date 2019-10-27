using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class EpidemicGenerator  {

	public List<Talent> Generate(int number) // number of epidemic to generate
    {
        List<Talent> curableDiseases = GetCurableDiseases();
        List<Talent> epidemic = new List<Talent>();
        for(int n=0; n<number; n++)
        {
            int randomIndex = Random.Range(0, curableDiseases.Count);
            if (!epidemic.Contains(curableDiseases[randomIndex]))
                epidemic.Add(curableDiseases[randomIndex]);
            else n--;

            if (epidemic.Count == curableDiseases.Count) return epidemic;
        }
        return epidemic;
    }
    public string GetDisease(Talent talent)
    {
        if (talent.diseases.Count == 0) return string.Empty;
        int randomIndex = Random.Range(0, talent.diseases.Count);
        return talent.diseases[randomIndex];
    }
    private List<Talent> GetCurableDiseases()
    {
        if (GameController.instance == null) return null;
        return
            (
                GameController.instance.talentTree.talents.Where(x => x.isUnlocked || x.canBeUnlocked).ToList()
            );
    }
}
