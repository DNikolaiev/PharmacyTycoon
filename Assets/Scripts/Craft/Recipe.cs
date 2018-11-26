using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Recipe  {
    public string Name;
    public int healingPlants;
    public int chemistry;
    public int plastic;
    public int toxicity;
    public bool isLiquid = true;

    
    private List<Talent> talents;
	public Recipe(List<Talent> talents, Characteristics characteristics, bool isLiquid)
    {
        healingPlants = characteristics.healingPlantsNeeded;
        chemistry = characteristics.chemistryNeeded;
        plastic = characteristics.plasticNeeded;
        toxicity = characteristics.toxicity;
        this.isLiquid = isLiquid;
        foreach(Talent tal in talents)
        {
            
                talents.Add(tal);
           
        }
        
    }

 

    public List<Talent> Talents
    {
        get
        {
            return talents;
        }
    }

}
