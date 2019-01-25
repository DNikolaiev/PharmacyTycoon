using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftController : MonoBehaviour {
    public CreationPanel cPanel;
    private Crafter crafter;
   
    private void Start()
    {
        crafter = GameController.instance.crafter;
    }
    public void DisplaySettings()
    {
       int counter = 0;
       foreach(Talent tal in crafter.selectedTalents)
        {
            if (tal.isPrimary)
                counter++;
        }
        if (crafter.selectedTalents.Count >= 1 && counter>0)
        {
            cPanel.SetPanel(crafter.GetCharacteristics());
            GameController.instance.buttons.cancel.gameObject.SetActive(false);
        }

        else return;
    }
    public void OnSelectHolder(CraftHolder holder) // on click at main talent holders
    {
        crafter.view.holderSelected = holder;
    }
    public void OnSelectHolder(RecipeHolder holder) // on click at main talent holders
    {
        crafter.view.recipeHolderSelected = holder;
    }

    public void OnAddTalent(TalentHolder talentHolder) // on click at talent's list
    {
        crafter.view.holderSelected.glowImg.gameObject.SetActive(false);
        crafter.view.craftDescriptionPanel.SetPanel(talentHolder.Talent, (CraftHolder)talentHolder);
        crafter.AssignTalent(talentHolder.Talent);
        Destroy(talentHolder.transform.parent.parent.gameObject);
    }
    public void OnAddRecipe(RecipeHolder recipeHolder)
    {
        crafter.view.holderSelected.glowImg.gameObject.SetActive(false);
        crafter.view.craftDescriptionPanel.SetPanel(recipeHolder.recipe, recipeHolder);
        crafter.AssignRecipe(recipeHolder.recipe);
   
    }
    public void OnAddRecipe(Recipe recipe)
    {
        crafter.recipeSelected = recipe;
        crafter.view.recipeHolderSelected.GetComponent<RecipeHolder>().recipe = recipe;
        crafter.view.recipeHolderSelected.GetComponent<RecipeHolder>().SetPanel();
        crafter.view.craftPanel.transform.parent.GetComponentInChildren<LaboratoryPanel>().SetValueToBar();
        
    }
    public void OnSliderChange(float value)
    {
        if (value == 0)
            crafter.isLiquid = true;
        else crafter.isLiquid = false;
    }
}
