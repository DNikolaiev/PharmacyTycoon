using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class Skills  {

    public int toxicityReducer;
    public int pillsHealing;
    public int liquidHealing;

    public void ReduceToxicity(Recipe recipe, int cost)
    {
        if (!GameController.instance.crafter.inventory.CheckIfWarehouseContains(recipe.description.Name))
             return;
        List<Recipe> prescription = GameController.instance.crafter.inventory.recipes.Where(x => x.description.Name == recipe.description.Name).ToList();
        Debug.Log(prescription[0].characteristics.toxicity - toxicityReducer);
        if (prescription[0].characteristics.toxicity - toxicityReducer > -toxicityReducer)
        {
            GameController.instance.player.resources.ChangeBalance(-cost);
        }
        prescription[0].characteristics.toxicity -= toxicityReducer;
        prescription[0].characteristics.toxicity = Mathf.Clamp(prescription[0].characteristics.toxicity, 0, 100);
        
        



    }
   
}
