using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class Recipe {
    public Description description;
    public Characteristics characteristics;
    public bool isLiquid = true;
    public bool isDeleted;
    public Price price;
    public int soldAmount;
    public int deadAmount;
   [SerializeField] private float deathRating;
   public SavedRecipeData saveData;
   [FullSerializer.fsIgnore] [SerializeField] private List<Talent> talents = new List<Talent>();
    private List<Talent> primaryTalents;
    private List<Talent> secondaryTalents;

    public Recipe(string Name, List<Talent> talents, Characteristics characteristics, bool isLiquid, Sprite avatar)
    {
        description = new Description();
        description.Name = Name;
        this.characteristics=characteristics;
        if(isLiquid)
            this.characteristics.healingRate += GameController.instance.player.skills.liquidHealing;
        else
            this.characteristics.healingRate += GameController.instance.player.skills.pillsHealing;
        deathRating = 0;
        this.isLiquid = isLiquid;
        this.talents.AddRange(talents);
        primaryTalents = this.talents.Where(x => x.isPrimary).ToList();
        secondaryTalents = this.talents.Where(x => !x.isPrimary).ToList();
        description.sprite = avatar;
        price = new Price(this);
     
    }

    public float GetDeathRating()
    { return float.Parse((deathRating * 10).ToString("0.###")); }
    public void RefreshDeathRating()
    {
        if (soldAmount != 0)
        {
            deathRating = Mathf.Round(((float)deadAmount / (float)soldAmount)*100)/100;
            if(deathRating>=0.1)
            {
                EventManager.TriggerEvent("OnOverkill", 1);
            }
            
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
    
    public void RecalculatePrice()
    {
        RefreshDeathRating();
        
        price.Value = price.GetRecipePrice(this);
        
    }
    public void OnLoad()
    {
        talents = new List<Talent>();

        foreach (int id in saveData.talentsID)
        {
            Debug.Log(id);
            talents.AddRange(GameController.instance.talentTree.talents.Where(x => x.id == id).ToList());
            
        }
        description.sprite= Resources.Load<Sprite>(saveData.spritePath);
        soldAmount = saveData.soldAmount;
        deadAmount = saveData.deadAmount;
        price.Value = saveData.price;
        primaryTalents = this.talents.Where(x => x.isPrimary).ToList();
        secondaryTalents = this.talents.Where(x => !x.isPrimary).ToList();
        RefreshDeathRating();
    }
    public void OnSave()
    {

        saveData = new SavedRecipeData(talents,description.sprite.name,deadAmount,soldAmount, price.Value);
    }
    
}
[System.Serializable]
public class SavedRecipeData
{
    public List<int> talentsID = new List<int>();
    public string spritePath = string.Empty;
    public int soldAmount;
    public int deadAmount;
    public int price;
        
    public SavedRecipeData(List<Talent> talents, string path, int dead, int sold, int price)
    {
        for (int i = 0; i < talents.Count; i++)
            talentsID.Add(talents[i].id);
        spritePath = "Icons/Recipe/"+path;
        soldAmount = sold;
        deadAmount = dead;
        this.price = price;
    }
}
