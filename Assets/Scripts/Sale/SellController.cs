using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellController : MonoBehaviour {
    [SerializeField] Seller seller;
	public void ConfirmSale()
    {
        if (RecipeSelector.recipeHolderSelected == null || RecipeSelector.recipeHolderSelected.recipe == null) return;
        Invoice invoice = new Invoice(seller.transferCost, (int)(RecipeSelector.recipeHolderSelected.recipe.price.Value*seller.area.sellMultiplier), 1, (float)seller.EpidemiesCured/4+1);
        seller.view.invoicePanel.SetPanel(invoice, seller.area);
        if (!seller.tutorial.isTutorialCompleted) seller.tutorial.ContinueTutorial();
    }
}
