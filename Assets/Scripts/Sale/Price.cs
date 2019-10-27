using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Price  {
   [SerializeField] private int price=0;
    public  float divider = 1;
    public int Value
    {
        get { return price; }
        set
        {
            price = value;
            price = Mathf.Clamp(price, 0, 999);
        }
    }

    public Price(Recipe recipe)
    {
        price = GetRecipePrice(recipe);
        if (price < 0)
            price = 0;
    }
    
	public int GetRecipePrice(Recipe recipe)
    {
        var market = GameController.instance.market;
        return (int)
            (
                (recipe.characteristics.healingPlantsNeeded * market.herbsPrice +
                recipe.characteristics.chemistryNeeded * market.chemsPrice +
                recipe.characteristics.plasticNeeded * market.plasticPrice -
                recipe.GetDeathRating() * (4+recipe.Talents.Count))/divider
            );

    }
}
