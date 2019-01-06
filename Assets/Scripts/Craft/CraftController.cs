using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftController : MonoBehaviour {
    public CreationPanel cPanel;
    private Crafter crafter;
    private void Awake()
    {
        crafter = Crafter.instance;
    }
    public void DisplaySettings()
    {
       int counter = 0;
       foreach(Talent tal in Crafter.instance.selectedTalents)
        {
            if (tal.isPrimary)
                counter++;
        }
        if (crafter.selectedTalents.Count >= 1 && counter>0)
        {
            cPanel.SetPanel(crafter.GetCharacteristics());
            ButtonController.instance.cancel.gameObject.SetActive(false);
        }

        else return;
    }
    public void OnSelectHolder(CraftHolder holder) // on click in main talent holders
    {
        crafter.holderSelected = holder;
    }
   
    public void OnAddTalent(TalentHolder talentHolder) // on click in talent's list
    {
        crafter.holderSelected.glowImg.gameObject.SetActive(false);
        crafter.craftDescriptionPanel.SetPanel(talentHolder.Talent, (CraftHolder)talentHolder);
        crafter.AssignTalent(talentHolder.Talent);
        Destroy(talentHolder.transform.parent.parent.gameObject);
    }
    public void OnAddRecipe(RecipeHolder recipeHolder)
    {
        crafter.holderSelected.glowImg.gameObject.SetActive(false);
        crafter.craftDescriptionPanel.SetPanel(recipeHolder.recipe, recipeHolder);
        crafter.AssignRecipe(recipeHolder.recipe);
   
    }
    public void OnSliderChange(float value)
    {
        if (value == 0)
            crafter.isLiquid = true;
        else crafter.isLiquid = false;
    }
}
