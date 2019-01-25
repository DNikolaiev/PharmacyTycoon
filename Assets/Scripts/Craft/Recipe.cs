using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class Recipe {
    public Description description;
    public Characteristics characteristics;
    public bool isLiquid = true;
    private int deathRating;
    private List<Talent> talents = new List<Talent>();
    private List<Talent> primaryTalents;
    private List<Talent> secondaryTalents;

    public Recipe(string Name, List<Talent> talents, Characteristics characteristics, bool isLiquid)
    {
        description = new Description();
        description.Name = Name;
        this.characteristics=characteristics;
        deathRating = 0;
        this.isLiquid = isLiquid;
        this.talents.AddRange(talents);
        primaryTalents = this.talents.Where(x => x.isPrimary).ToList();
        secondaryTalents = this.talents.Where(x => !x.isPrimary).ToList(); 
    }

   
    public int DeathRating
    {
        get
        {
            return deathRating;
        }
        set
        {
            deathRating = value;
        }
    }
    public List<Talent> Talents
    {
        get
        {
            return talents;
        }
    }
    public List<Talent> PTalents
    {
        get
        {
            return primaryTalents;
        }
    }
    public List<Talent> STalents
    {
        get
        {
            return secondaryTalents;
        }
    }

}
