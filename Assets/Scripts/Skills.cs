using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class Skills  {

    public int toxicityReducer;
    public int enhancementCounter;
    public int pillsHealing;
    public int liquidHealing;
    [FullSerializer.fsIgnore] public Player player;
    public void ReduceToxicity(Recipe recipe, int cost, out bool result)
    {
        if (!GameController.instance.player.inventory.CheckIfWarehouseContains(recipe.description.Name)
            || GameController.instance.player.resources.money-cost < 0 )
        {
            result = false;
            return;
        }
        List<Recipe> prescription = player.inventory.recipes.Where(x => x.description.Name == recipe.description.Name && !x.isDeleted).ToList();
        
        if (prescription[0].characteristics.toxicity - toxicityReducer > -toxicityReducer)
        {
            GameController.instance.player.resources.ChangeBalance(-cost);
        }
        result = true;
        prescription[0].characteristics.toxicity -= toxicityReducer;
        prescription[0].characteristics.toxicity = Mathf.Clamp(prescription[0].characteristics.toxicity, 0, 100);
        player.GainExperience(toxicityReducer * 10 + (recipe.Talents.Count)*50);
        if(prescription.LastOrDefault().characteristics.toxicity==0)
        {
            EventManager.TriggerEvent("OnReduceToxicity", 1);
        }
    }
    public void EnhanceHealing(bool isLiquid, int amount, int price, out bool result)
    {

        if (GameController.instance.player.resources.money < price)
        {
            result = false;
            return;
        }
        else GameController.instance.player.resources.ChangeBalance(-price);
        if (isLiquid)
            liquidHealing += amount;
        else pillsHealing += amount;
        
        foreach (Recipe recipe in player.inventory.recipes.Where(x => x.isLiquid == isLiquid))
        {
            recipe.characteristics.healingRate += amount;
        }
        result = true;
    }
}
